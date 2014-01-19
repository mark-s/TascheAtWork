using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TascheAtWork.PocketAPI.Helpers;
using TascheAtWork.PocketAPI.Models.Parameters;
using TascheAtWork.PocketAPI.Models.Response;

namespace TascheAtWork.PocketAPI
{
    public partial class ClientCore :IPocketClient
    {

        /// <summary>
        /// REST client used for the API communication
        /// </summary>
        private readonly HttpClient _webClient;

        /// <summary>
        /// Caches HTTP headers from last response
        /// </summary>
        private HttpResponseHeaders _lastHeaders;

        /// <summary>
        /// The base URL for the Pocket API
        /// </summary>
        private readonly Uri _pocketAPIBaseUri = new Uri("https://getpocket.com/v3/");

        /// <summary>
        /// The authentification URL
        /// </summary>
        protected string authentificationUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}";

        /// <summary>
        /// callback URL for API calls
        /// </summary>
        public string CallbackUri { get; set; }

        /// <summary>
        /// Accessor for the Pocket API key
        /// see: http://getpocket.com/developer
        /// </summary>
        public string PlatformConsumerKey { get; set; }

        /// <summary>
        /// Code retrieved on authentification
        /// </summary>
        public string RequestCode { get; set; }

        /// <summary>
        /// Code retrieved on authentification-success
        /// </summary>
        public string AccessCode { get; set; }


        /// <param name="platformConsumerKey">The API key</param>
        /// <param name="accessCode">Provide an access code if the user is already authenticated</param>
        /// <param name="callbackUri">The callback URL is called by Pocket after authentication</param>
        public ClientCore(string platformConsumerKey, string accessCode = null, string callbackUri = null)
        {
            // assign public properties
            PlatformConsumerKey = platformConsumerKey;

            // assign access code if submitted
            if (accessCode != null)
                AccessCode = accessCode;

            // assign callback uri if submitted
            if (callbackUri != null)
                CallbackUri = Uri.EscapeUriString(callbackUri);

            var httpClientHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip };

            // initialize REST client
            _webClient = new HttpClient(httpClientHandler);

            // set the base uri
            _webClient.BaseAddress = _pocketAPIBaseUri;

            // Pocket needs this specific Accept header 
            _webClient.DefaultRequestHeaders.Add("Accept", "*/*");

            // defines the response format (according to the Pocket docs)
            _webClient.DefaultRequestHeaders.Add("X-Accept", "application/json");

        }


        /// <summary>
        /// Fetches a typed resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">Requested method (path after /v3/)</param>
        /// <param name="parameters">Additional POST parameters</param>
        /// <param name="requireAuth">if set to <c>true</c> [require auth].</param>
        /// <returns></returns>
        /// <exception cref="PocketException">No access token available. Use authentification first.</exception>
        protected T Request<T>(string method, Dictionary<string, string> parameters = null, bool requireAuth = true) where T : class, new()
        {
            if (requireAuth && AccessCode == null)
            {
                throw new PocketException("SDK error: No access token available. Use authentification first.");
            }

            // every single Pocket API endpoint requires HTTP POST data
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, method);
            HttpResponseMessage response = null;

            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            // add consumer key to each request
            parameters.Add("consumer_key", PlatformConsumerKey);

            // add access token (necessary for all requests except authentification)
            if (AccessCode != null)
            {
                parameters.Add("access_token", AccessCode);
            }

            // content of the request
            request.Content = new FormUrlEncodedContent(parameters);

            // make  request
            try
            {
                var tsk = _webClient.SendAsync(request);
                tsk.Wait();
                response = tsk.Result;
            }
            catch (HttpRequestException exc)
            {
                throw new PocketException(exc.Message, exc);
            }

            // validate HTTP response
            ValidateResponse(response);

            // cache headers
            _lastHeaders = response.Headers;

            // read response
            var responseString = response.Content.ReadAsStringAsync().Result;

            responseString = responseString.Replace("[]", "{}");

            // deserialize object
            var parsedResponse = JsonConvert.DeserializeObject<T>(responseString,
                                                                                      new JsonSerializerSettings
                                                                                      {
                                                                                          Error = (sender, args) => { throw new PocketException(String.Format("Parse error: {0}", args.ErrorContext.Error.Message)); },
                                                                                          Converters =
                                                                                                              {
                                                                                                                new BoolConverter(),
                                                                                                                new UnixDateTimeConverter(),
                                                                                                                new NullableIntConverter()
                                                                                                              }
                                                                                      }
                                                                                    );

            return parsedResponse;
        }



        /// <summary>
        /// Sends a list of actions
        /// </summary>
        /// <param name="actionParameters">The action parameters.</param>
        /// <returns></returns>
        internal bool Send(List<ActionParameter> actionParameters)
        {
            List<Dictionary<string, object>> actionParamList = new List<Dictionary<string, object>>();

            foreach (var action in actionParameters)
            {
                actionParamList.Add(action.Convert());
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>() { { "actions", JsonConvert.SerializeObject(actionParamList) } };

            Modify response = Request<Modify>("send", parameters);

            return response.Status;
        }


        /// <summary>
        /// Sends an action
        /// </summary>
        /// <param name="actionParameter">The action parameter.</param>
        /// <returns></returns>
        internal bool Send(ActionParameter actionParameter)
        {
            bool response = Send(new List<ActionParameter>() { actionParameter });
            return response;
        }


        /// <summary>
        /// Validates the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <exception cref="PocketException">
        /// Error retrieving response
        /// </exception>
        protected void ValidateResponse(HttpResponseMessage response)
        {
            // no error found
            if (response.StatusCode == HttpStatusCode.OK)
                return;

            string exceptionString;

            var containsError = response.Headers.Contains("X-Error");

            // get custom pocket headers
            var error = TryGetHeaderValue(response.Headers, "X-Error");
            int errorCode = Convert.ToInt32(TryGetHeaderValue(response.Headers, "X-Error-Code"));

            // create exception strings
            if (containsError)
            {
                exceptionString = String.Format("Pocket error: {0} ({1}) ", error, errorCode);
            }
            else
            {
                exceptionString = String.Format("Request error: {0} ({1})", response.ReasonPhrase, (int)response.StatusCode);
            }

            // create exception
            var exception = new PocketException(exceptionString);

            if (containsError)
            {
                // add custom pocket fields
                exception.PocketError = error;
                exception.PocketErrorCode = errorCode;

                // add to generic exception data
                exception.Data.Add("X-Error", error);
                exception.Data.Add("X-Error-Code", errorCode);
            }

            throw exception;
        }


        /// <summary>
        /// Tries to fetch a header value.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected string TryGetHeaderValue(HttpResponseHeaders headers, string key)
        {
            string result = null;

            if (headers == null || String.IsNullOrEmpty(key))
            {
                return null;
            }

            foreach (var header in headers)
            {
                if (header.Key == key)
                {
                    var headerEnumerator = header.Value.GetEnumerator();
                    headerEnumerator.MoveNext();

                    result = headerEnumerator.Current;
                    break;
                }
            }

            return result;
        }
    }
}
