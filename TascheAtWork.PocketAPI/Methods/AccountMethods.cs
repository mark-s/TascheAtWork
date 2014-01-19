using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TascheAtWork.PocketAPI.Interfaces;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;
using TascheAtWork.PocketAPI.Models.Response;

namespace TascheAtWork.PocketAPI.Methods
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public class AccountMethods : IHandleAccounts
    {
        private readonly IInternalAPI _client;
        private readonly IPocketAPISession _sessionData;

        public AccountMethods(IInternalAPI client, IPocketAPISession sessionData)
        {
            _client = client;
            _sessionData = sessionData;
        }

        /// <summary>
        /// Retrieves the requestCode from Pocket, which is used to generate the Authentication URI to authenticate the user
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Authentication methods need a callbackUri on initialization of the PocketClient class</exception>
        /// <exception cref="PocketAPIException"></exception>
        public string GetRequestCode()
        {
            // check if request code is available
            if (_sessionData.AuthenticationCallbackUri == null)
            {
                throw new NullReferenceException("Authentication methods need a callbackUri on initialization of the PocketClient class");
            }

            // do request
            RequestCodeResponse response = _client.Request<RequestCodeResponse>("oauth/request", new Dictionary<string, string>()
            {
                {"redirect_uri", _sessionData.AuthenticationCallbackUri}
            }, false);

            // save code to client
            _sessionData.RequestCode = response.Code;

            // generate redirection URI and return
            return _sessionData.RequestCode;
        }


        /// <summary>
        /// Generate Authentication URI from requestCode
        /// </summary>
        /// <param name="requestCode">The requestCode. If no requestCode is supplied, the property from the PocketClient intialization is used.</param>
        /// <returns>A valid URI to redirect the user to.</returns>
        /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
        public Uri GenerateAuthenticationUri(string requestCode = null)
        {
            // check if request code is available
            if (_sessionData.RequestCode == null && requestCode == null)
            {
                throw new NullReferenceException("Call GetRequestCode() first to receive a request_code");
            }

            // override property with given param if available
            if (requestCode != null)
                _sessionData.RequestCode = requestCode;

            return new Uri(string.Format(_sessionData.AuthentificationUri, _sessionData.RequestCode, _sessionData.AuthenticationCallbackUri));
        }


        /// <summary>
        /// Requests the access code and username after authentication
        /// The access code has to permanently be stored within the users session, and should be passed in the constructor for all future PocketClient initializations. 
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <returns>The authenticated user</returns>
        /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
        public PocketUser GetUser(string requestCode = null)
        {
            // check if request code is available
            if (_sessionData.RequestCode == null && requestCode == null)
            {
                throw new NullReferenceException("Call GetRequestCode() first to receive a request_code");
            }

            // override property with given param if available
            if (requestCode != null)
            {
                _sessionData.RequestCode = requestCode;
            }

            // do request
            PocketUser response = _client.Request<PocketUser>("oauth/authorize", new Dictionary<string, string>()
            {
                {"code", _sessionData.RequestCode}
            }, false);

            // save code to client
            _sessionData.AccessCode = response.Code;

            return response;
        }


        /// <summary>
        /// Registers a new account.
        /// Account has to be activated via a activation email sent by Pocket.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">All parameters are required</exception>
        /// <exception cref="System.FormatException">
        /// Invalid email address.
        /// or
        /// Invalid username. Please only use letters, numbers, and/or dashes and between 1-20 characters.
        /// or
        /// Invalid password.
        /// </exception>
        /// <exception cref="PocketAPIException"></exception>
        public bool RegisterAccount(string username, string email, string password)
        {
            if (username == null || email == null || password == null)
            {
                throw new ArgumentNullException("All parameters are required");
            }

            Match matchEmail = Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,10}))$");
            Match matchUsername = Regex.Match(username, @"^([\w\-_]{1,20})$");

            if (!matchEmail.Success)
            {
                throw new FormatException("(1) Invalid email address.");
            }

            if (!matchUsername.Success)
            {
                throw new FormatException("(2) Invalid username. Please only use letters, numbers, and/or dashes and between 1-20 characters.");
            }

            if (password.Length < 3)
            {
                throw new FormatException("(3) Invalid password.");
            }

            RegisterParameters parameters = new RegisterParameters()
            {
                Username = username,
                Email = email,
                Password = password
            };

            ResponseBase response = _client.Request<ResponseBase>("signup", parameters.ConvertToHTTPPostParameters(), false);

            return response.Status;
        }
    }
}