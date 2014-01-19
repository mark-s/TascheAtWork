using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using TascheAtWork.PocketAPI.Helpers;
using TascheAtWork.PocketAPI.Interfaces;
using TascheAtWork.PocketAPI.Methods;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;
using TascheAtWork.PocketAPI.Models.Response;

namespace TascheAtWork.PocketAPI
{
    public class PocketAPIClient : IPocketClient, IInternalAPI
    {
        private readonly IPocketAPISession _pocketSession = new PocketSessionData();
        private readonly HttpClient _webClient;

        private readonly IHandleAdd _addMethods;
        private readonly IHandleAccounts _accountMethods;
        private readonly IHandleGet _getMethods;
        private readonly IHandleModify _modifyMethods;
        private readonly IHandleModifyTags _modifyTagMethods;

        /// <summary>
        /// Caches HTTP headers from last response
        /// </summary>
        private HttpResponseHeaders _cachedHeaders;

        /// <summary>
        /// The base URL for the Pocket API
        /// </summary>
        private readonly Uri _pocketAPIBaseUri = new Uri("https://getpocket.com/v3/");


        /// <param name="platformConsumerKey">The API key</param>
        /// <param name="accessCode">Provide an access code if the user is already authenticated</param>
        /// <param name="callbackUri">The callback URL is called by Pocket after authentication</param>
        public PocketAPIClient(string platformConsumerKey, string accessCode = null, string callbackUri = null)
        {

            _addMethods = new AddMethods(this);
            _accountMethods = new AccountMethods(this, _pocketSession);
            _getMethods = new GetMethods(this);
            _modifyMethods = new ModifyMethods(this);
            _modifyTagMethods = new ModifyTagMethods(this);

            // assign public properties
            _pocketSession.PlatformConsumerKey = platformConsumerKey;

            // assign access code if submitted
            if (accessCode != null)
                _pocketSession.AccessCode = accessCode;

            // assign callback uri if submitted
            if (callbackUri != null)
                _pocketSession.AuthenticationCallbackUri = Uri.EscapeUriString(callbackUri);

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
        /// <exception cref="PocketAPIException">No access token available. Use authentification first.</exception>
        public T Request<T>(string method, Dictionary<string, string> parameters = null, bool requireAuth = true) where T : class, new()
        {
            if (requireAuth && _pocketSession.AccessCode == null)
            {
                throw new PocketAPIException("SDK error: No access token available. Use authentification first.");
            }

            // every single Pocket API endpoint requires HTTP POST data
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, method);
            HttpResponseMessage response = null;

            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            // add consumer key to each request
            parameters.Add("consumer_key", _pocketSession.PlatformConsumerKey);

            // add access token (necessary for all requests except authentification)
            if (_pocketSession.AccessCode != null)
            {
                parameters.Add("access_token", _pocketSession.AccessCode);
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
                throw new PocketAPIException(exc.Message, exc);
            }

            // validate HTTP response
            ValidateResponse(response);

            // cache headers
            _cachedHeaders = response.Headers;

            // read response
            var responseString = response.Content.ReadAsStringAsync().Result;

            responseString = responseString.Replace("[]", "{}");

            // deserialize object
            var parsedResponse = JsonConvert.DeserializeObject<T>(responseString,
                                                                                      new JsonSerializerSettings
                                                                                      {
                                                                                          Error = (sender, args) => { throw new PocketAPIException(String.Format("Parse error: {0}", args.ErrorContext.Error.Message)); },
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
        public bool Send(List<ActionParameter> actionParameters)
        {
            List<Dictionary<string, object>> actionParamList = new List<Dictionary<string, object>>();

            foreach (var action in actionParameters)
            {
                actionParamList.Add(action.Convert());
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>() { { "actions", JsonConvert.SerializeObject(actionParamList) } };

            ModifyResponse response = Request<ModifyResponse>("send", parameters);

            return response.Status;
        }

        /// <summary>
        /// Sends an action
        /// </summary>
        /// <param name="actionParameter">The action parameter.</param>
        /// <returns></returns>
        public bool Send(ActionParameter actionParameter)
        {
            bool response = Send(new List<ActionParameter>() { actionParameter });
            return response;
        }

        /// <summary>
        /// Validates the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException">
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
            var exception = new PocketAPIException(exceptionString);

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

        public string CallbackUri { get { return _pocketSession.AuthenticationCallbackUri; } set { _pocketSession.AuthenticationCallbackUri = value; } }
        public string PlatformConsumerKey { get { return _pocketSession.PlatformConsumerKey; } set { _pocketSession.PlatformConsumerKey = value; } }
        public string RequestCode { get { return _pocketSession.RequestCode; } set { _pocketSession.RequestCode = value; } }
        public string AccessCode { get { return _pocketSession.AccessCode; } set { _pocketSession.AccessCode = value; } }
        public string GetRequestCode()
        {
            return _accountMethods.GetRequestCode();
        }

        public Uri GenerateAuthenticationUri(string requestCode = null)
        {
            return _accountMethods.GenerateAuthenticationUri(requestCode);
        }

        public PocketUser GetUser(string requestCode = null)
        {
            return _accountMethods.GetUser(requestCode);
        }

        public bool RegisterAccount(string username, string email, string password)
        {
            return _accountMethods.RegisterAccount(username, email, password);
        }

        public PocketItem AddItem(Uri uri, string[] tags = null, string title = null, string tweetID = null)
        {
            return _addMethods.AddItem(uri, tags, title, tweetID);
        }

        public List<PocketItem> GetItems(State? state = null, bool? favorite = null,
                                                        string tag = null, ContentType? contentType = null, Sort? sort = null,
                                                        string search = null, string domain = null, DateTime? since = null,
                                                        int? count = null, int? offset = null)
        {
            return _getMethods.GetItems(state, favorite, tag, contentType, sort, search, domain, since, count, offset);
        }

        public PocketItem GetItem(int itemID)
        {
            return _getMethods.GetItem(itemID);
        }

        public List<PocketItem> GetItems(RetrieveFilter filter)
        {
            return _getMethods.GetItems(filter);
        }

        public List<PocketTag> GetTags()
        {
            return _getMethods.GetTags();
        }

        public List<PocketItem> SearchByTag(string tag)
        {
          return  _getMethods.SearchByTag(tag);
        }

        public List<PocketItem> Search(string searchString, bool searchInUri = true)
        {
            return _getMethods.Search(searchString, searchInUri);
        }

        public List<PocketItem> Search(List<PocketItem> availableItems, string searchString)
        {
            return _getMethods.Search(availableItems, searchString);
        }

        public bool Archive(int itemID)
        {
            return _modifyMethods.Archive(itemID);
        }

        public bool Archive(PocketItem item)
        {
            return _modifyMethods.Archive(item);
        }

        public bool Unarchive(int itemID)
        {
            return _modifyMethods.Unarchive(itemID);
        }

        public bool Unarchive(PocketItem item)
        {
            return _modifyMethods.Unarchive(item);
        }

        public bool Favorite(int itemID)
        {
            return _modifyMethods.Favorite(itemID);
        }

        public bool Favorite(PocketItem item)
        {
            return _modifyMethods.Favorite(item);
        }

        public bool Unfavorite(int itemID)
        {
            return _modifyMethods.Unfavorite(itemID);
        }

        public bool Unfavorite(PocketItem item)
        {
            return _modifyMethods.Unfavorite(item);
        }

        public bool Delete(int itemID)
        {
            return _modifyMethods.Delete(itemID);
        }

        public bool Delete(PocketItem item)
        {
            return _modifyMethods.Delete(item);
        }

        public bool AddTags(int itemID, string[] tags)
        {
            return _modifyTagMethods.AddTags(itemID, tags);
        }

        public bool AddTags(PocketItem item, string[] tags)
        {
            return _modifyTagMethods.AddTags(item, tags);
        }

        public bool RemoveTags(int itemID, string[] tags)
        {
            return _modifyTagMethods.RemoveTags(itemID, tags);
        }

        public bool RemoveTags(PocketItem item, string[] tags)
        {
            return _modifyTagMethods.RemoveTags(item, tags);
        }

        public bool RemoveTag(int itemID, string tag)
        {
            return _modifyTagMethods.RemoveTag(itemID, tag);
        }

        public bool RemoveTag(PocketItem item, string tag)
        {
            return _modifyTagMethods.RemoveTag(item, tag);
        }

        public bool RemoveTags(int itemID)
        {
            return _modifyTagMethods.RemoveTags(itemID);
        }

        public bool RemoveTags(PocketItem item)
        {
            return _modifyTagMethods.RemoveTags(item);
        }

        public bool ReplaceTags(int itemID, string[] tags)
        {
            return _modifyTagMethods.ReplaceTags(itemID, tags);
        }

        public bool ReplaceTags(PocketItem item, string[] tags)
        {
            return _modifyTagMethods.ReplaceTags(item, tags);
        }

        public bool RenameTag(int itemID, string oldTag, string newTag)
        {
            return _modifyTagMethods.RenameTag(itemID, oldTag, newTag);
        }

        public bool RenameTag(PocketItem item, string oldTag, string newTag)
        {
            return _modifyTagMethods.RenameTag(item, oldTag, newTag);
        }

        public PocketStatistics GetUserStatistics()
        {
            throw new NotImplementedException();
        }

        public PocketLimits GetUsageLimits()
        {
            throw new NotImplementedException();
        }
    }
}
