using System;
using System.Threading.Tasks;
using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IHandleAccounts
    {
        /// <summary>
        /// Retrieves the requestCode from Pocket, which is used to generate the Authentication URI to authenticate the user
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Authentication methods need a callbackUri on initialization of the PocketClient class</exception>
        /// <exception cref="PocketAPIException"></exception>
        string GetRequestCode();

        /// <summary>
        /// Generate Authentication URI from requestCode
        /// </summary>
        /// <param name="requestCode">The requestCode. If no requestCode is supplied, the property from the PocketClient intialization is used.</param>
        /// <returns>A valid URI to redirect the user to.</returns>
        /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
        Uri GenerateAuthenticationUri(string requestCode = null);

        /// <summary>
        /// Requests the access code and username after authentication
        /// The access code has to permanently be stored within the users session, and should be passed in the constructor for all future PocketClient initializations. 
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <returns>The authenticated user</returns>
        /// <exception cref="System.NullReferenceException">Call GetRequestCode() first to receive a request_code</exception>
        PocketUser GetUser(string requestCode = null);

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
        bool RegisterAccount(string username, string email, string password);
    }
}