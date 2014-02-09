using System;
using TascheAtWork.PocketAPI.Interfaces;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;
using TascheAtWork.PocketAPI.Models.Response;

namespace TascheAtWork.PocketAPI.Methods
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public class AddMethods : IHandleAdd
    {
        private readonly IInternalAPI _client;

        public AddMethods(IInternalAPI client)
        {
            _client = client;
        }

        /// <summary>
        /// Adds a new item to pocket
        /// </summary>
        /// <param name="uri">The URL of the item you want to save</param>
        /// <param name="tags">A comma-separated list of tags to apply to the item</param>
        /// <param name="title">This can be included for cases where an item does not have a title, which is typical for image or PDF URLs. If Pocket detects a title from the content of the page, this parameter will be ignored.</param>
        /// <param name="tweetID">If you are adding Pocket support to a Twitter client, please send along a reference to the tweet status id. This allows Pocket to show the original tweet alongside the article.</param>
        /// <returns>A simple representation of the saved item which doesn't contain all data (is only returned by calling the Retrieve method)</returns>
        /// <exception cref="PocketAPIException"></exception>
        public PocketItem AddItem(Uri uri, string[] tags = null, string title = null, string tweetID = null)
        {
            var parameters = new AddParameters
                                            {
                                                Uri = uri,
                                                Tags = tags,
                                                Title = title,
                                                TweetID = tweetID
                                            };

            var response = _client.Request<AddResponse>("add", parameters.ConvertToHTTPPostParameters());

            return response.Item;
        }
    }
}