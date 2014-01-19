namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IPocketAPISession
    {

        /// <summary>
        /// The authentification URL
        /// </summary>
        string AuthentificationUri { get; set; }

        /// <summary>
        /// callback URL for API calls
        /// </summary>
        string AuthenticationCallbackUri { get; set; }

        /// <summary>
        /// Accessor for the Pocket API key
        /// see: http://getpocket.com/developer
        /// </summary>
        string PlatformConsumerKey { get; set; }

        /// <summary>
        /// Code retrieved on authentification
        /// </summary>
        string RequestCode { get; set; }

        /// <summary>
        /// Code retrieved on authentification-success
        /// </summary>
        string AccessCode { get; set; }
    }
}