using System;
using System.Threading.Tasks;
using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.PocketAPI
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public partial class ClientCore
    {
        /// <summary>
        /// Statistics from the user account.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public PocketStatistics GetUserStatistics()
        {
            return Request<PocketStatistics>("stats");
        }


        /// <summary>
        /// Statistics from the user account.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        [Obsolete("Please use GetUserStatistics instead")]
        public PocketStatistics Statistics()
        {
            return GetUserStatistics();
        }


        /// <summary>
        /// Returns API usage statistics.
        /// If a request was made before, the data is returned synchronously from the cache.
        /// Note: This method only works for authenticated users with a given AccessCode.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public PocketLimits GetUsageLimits()
        {
            string rateLimitForConsumerKey = TryGetHeaderValue(_lastHeaders, "X-Limit-Key-Limit");

            if (rateLimitForConsumerKey == null)
            {
                // this is the fastest way to do a non-failing request to receive the correct headers
                Get(count: 1);
            }

            return new PocketLimits()
            {
                RateLimitForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-Key-Limit")),
                RemainingCallsForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-Key-Remaining")),
                SecondsUntilLimitResetsForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-Key-Reset")),
                RateLimitForUser = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-User-Limit")),
                RemainingCallsForUser = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-User-Remaining")),
                SecondsUntilLimitResetsForUser = Convert.ToInt32(TryGetHeaderValue(_lastHeaders, "X-Limit-User-Reset"))
            };
        }
    }
}