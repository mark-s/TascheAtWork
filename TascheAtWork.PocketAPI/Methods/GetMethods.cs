using System;
using System.Collections.Generic;
using System.Linq;
using TascheAtWork.PocketAPI.Interfaces;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;
using TascheAtWork.PocketAPI.Models.Response;

namespace TascheAtWork.PocketAPI.Methods
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public class GetMethods : IHandleGet
    {
        private readonly IInternalAPI _client;


        public GetMethods(IInternalAPI client)
        {
            _client = client;
        }


        /// <summary>
        /// Retrieves items from pocket
        /// with the given filters
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="favorite">The favorite.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="search">The search.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="since">The since.</param>
        /// <param name="count">The count.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketItem> GetItems(State? state = null,
                                                        bool? favorite = null,
                                                        string tag = null,
                                                        ContentType? contentType = null,
                                                        Sort? sort = null,
                                                        string search = null,
                                                        string domain = null,
                                                        DateTime? since = null,
                                                        int? count = null,
                                                        int? offset = null)
        {
            var parameters = new RetrieveParameters
                                                {
                                                    State = state,
                                                    Favorite = favorite,
                                                    Tag = tag,
                                                    ContentType = contentType,
                                                    Sort = sort,
                                                    DetailType = DetailType.complete,
                                                    Search = search,
                                                    Domain = domain,
                                                    Since = since,
                                                    Count = count,
                                                    Offset = offset
                                                };

            var response = _client.Request<RetrieveResponse>("get", parameters.ConvertToHTTPPostParameters());

            return response.Items;
        }


        /// <summary>
        /// Retrieves an item by a given ID
        /// Note: The Pocket API contains no method, which allows to retrieve a single item, so all items are retrieved and filtered locally by the ID.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public PocketItem GetItem(int itemID)
        {
            var items = GetItems(state: State.all);
            return items.SingleOrDefault(item => item.ID == itemID);
        }


        /// <summary>
        /// Retrieves all items by a given filter
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketItem> GetItems(RetrieveFilter filter)
        {
            var parameters = new RetrieveParameters();

            switch (filter)
            {
                case RetrieveFilter.Article:
                    parameters.ContentType = ContentType.article;
                    break;
                case RetrieveFilter.Image:
                    parameters.ContentType = ContentType.image;
                    break;
                case RetrieveFilter.Video:
                    parameters.ContentType = ContentType.video;
                    break;
                case RetrieveFilter.Favorite:
                    parameters.Favorite = true;
                    break;
                case RetrieveFilter.Unread:
                    parameters.State = State.unread;
                    break;
                case RetrieveFilter.Archive:
                    parameters.State = State.archive;
                    break;
                case RetrieveFilter.All:
                    parameters.State = State.all;
                    break;
            }

            parameters.DetailType = DetailType.complete;

            RetrieveResponse response = _client.Request<RetrieveResponse>("get", parameters.ConvertToHTTPPostParameters());

            return response.Items;
        }


        /// <summary>
        /// Retrieves all available tags.
        /// Note: The Pocket API contains no method, which allows to retrieve all tags, so all items are retrieved and the associated tags extracted.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketTag> GetTags()
        {
            var items = GetItems(state: State.all);

            return items.Where(item => item.Tags != null)
                            .SelectMany(item => item.Tags)
                            .GroupBy(item => item.Name)
                            .Select(item => item.First())
                            .ToList();
        }


        /// <summary>
        /// Retrieves items by tag
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketItem> SearchByTag(string tag)
        {
            return GetItems(tag: tag);
        }


        /// <summary>
        /// Retrieves items which match the specified search string in title and URI
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="searchInUri"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketItem> Search(string searchString, bool searchInUri = true)
        {
            var items = GetItems(RetrieveFilter.All);
            return Search(items, searchString);
        }


        /// <summary>
        /// Finds the specified search string in title and URI for an available list of items
        /// </summary>
        /// <param name="availableItems">The available items.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
        /// <exception cref="PocketAPIException"></exception>
        public List<PocketItem> Search(List<PocketItem> availableItems, string searchString)
        {
            if (searchString.Length < 2)
                throw new ArgumentOutOfRangeException("Search string length has to be a minimum of 2 chars");

            var strSearch = searchString.ToUpperInvariant();

            return availableItems.Where(item =>
                                                    (
                                                    (!String.IsNullOrEmpty(item.FullTitle) && item.FullTitle.ToUpperInvariant().Contains(strSearch))
                                                    || item.Uri.ToString().ToUpperInvariant().Contains(strSearch))
                                                  ).ToList(); 
        }
    }
}