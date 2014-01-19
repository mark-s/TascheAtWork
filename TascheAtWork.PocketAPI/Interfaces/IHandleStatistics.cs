using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IHandleStatistics
    {
        /// <summary>
        /// Statistics from the user account.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        PocketStatistics GetUserStatistics();

        /// <summary>
        /// Returns API usage statistics.
        /// If a request was made before, the data is returned synchronously from the cache.
        /// Note: This method only works for authenticated users with a given AccessCode.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        PocketLimits GetUsageLimits();
    }
}