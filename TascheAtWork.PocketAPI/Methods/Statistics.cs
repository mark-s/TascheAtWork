//using System;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using TascheAtWork.PocketAPI.Models;

//namespace TascheAtWork.PocketAPI
//{
//    /// <summary>
//    /// PocketClient
//    /// </summary>
//    public class Statistics
//    {
//        private readonly PocketSessionData _sessionData;

//        public Statistics(PocketSessionData sessionData)
//        {
//            _sessionData = sessionData;
//        }

//        /// <summary>
//        /// Statistics from the user account.
//        /// </summary>
//        /// <returns></returns>
//        /// <exception cref="PocketException"></exception>
//        public PocketStatistics GetUserStatistics()
//        {
//            return Request<PocketStatistics>("stats");
//        }


//        /// <summary>
//        /// Statistics from the user account.
//        /// </summary>
//        /// <returns></returns>
//        /// <exception cref="PocketException"></exception>
//        [Obsolete("Please use GetUserStatistics instead")]
//        public PocketStatistics Statistics()
//        {
//            return GetUserStatistics();
//        }


//        /// <summary>
//        /// Returns API usage statistics.
//        /// If a request was made before, the data is returned synchronously from the cache.
//        /// Note: This method only works for authenticated users with a given AccessCode.
//        /// </summary>
//        /// <returns></returns>
//        /// <exception cref="PocketException"></exception>
//        public PocketLimits GetUsageLimits()
//        {
//            string rateLimitForConsumerKey = TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-Key-Limit");

//            if (rateLimitForConsumerKey == null)
//            {
//                // this is the fastest way to do a non-failing request to receive the correct headers
//                Get(count: 1);
//            }

//            return new PocketLimits()
//            {
//                RateLimitForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-Key-Limit")),
//                RemainingCallsForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-Key-Remaining")),
//                SecondsUntilLimitResetsForConsumerKey = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-Key-Reset")),
//                RateLimitForUser = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-User-Limit")),
//                RemainingCallsForUser = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-User-Remaining")),
//                SecondsUntilLimitResetsForUser = Convert.ToInt32(TryGetHeaderValue(_sessionData.CachedHeaders, "X-Limit-User-Reset"))
//            };
//        }


//        /// <summary>
//        /// Tries to fetch a header value.
//        /// </summary>
//        /// <param name="headers">The headers.</param>
//        /// <param name="key">The key.</param>
//        /// <returns></returns>
//        protected string TryGetHeaderValue(HttpResponseHeaders headers, string key)
//        {
//            string result = null;

//            if (headers == null || String.IsNullOrEmpty(key))
//            {
//                return null;
//            }

//            foreach (var header in headers)
//            {
//                if (header.Key == key)
//                {
//                    var headerEnumerator = header.Value.GetEnumerator();
//                    headerEnumerator.MoveNext();

//                    result = headerEnumerator.Current;
//                    break;
//                }
//            }

//            return result;
//        }

//    }
//}