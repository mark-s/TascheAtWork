using System;
using System.Net.Http.Headers;
using TascheAtWork.PocketAPI.Interfaces;

namespace TascheAtWork.PocketAPI
{
    public class PocketSessionData : IPocketAPISession
    {

        public PocketSessionData()
        {
            AuthentificationUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}";
        }

        /// <summary>
        /// The authentification URL
        /// </summary>
        public string AuthentificationUri { get; set; }

        /// <summary>
        /// callback URL for API calls
        /// </summary>
        public string AuthenticationCallbackUri { get; set; }

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
    }
}