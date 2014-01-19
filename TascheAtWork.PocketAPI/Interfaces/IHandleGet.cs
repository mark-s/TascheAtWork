using System;
using System.Collections.Generic;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IHandleGet
    {

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
        /// <exception cref="PocketException"></exception>
        List<PocketItem> GetItems(
            State? state = null,
            bool? favorite = null,
            string tag = null,
            ContentType? contentType = null,
            Sort? sort = null,
            string search = null,
            string domain = null,
            DateTime? since = null,
            int? count = null,
            int? offset = null
            );

        /// <summary>
        /// Retrieves an item by a given ID
        /// Note: The Pocket API contains no method, which allows to retrieve a single item, so all items are retrieved and filtered locally by the ID.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        PocketItem GetItem(int itemID);

        /// <summary>
        /// Retrieves all items by a given filter
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        List<PocketItem> GetItems(RetrieveFilter filter);

        /// <summary>
        /// Retrieves all available tags.
        /// Note: The Pocket API contains no method, which allows to retrieve all tags, so all items are retrieved and the associated tags extracted.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        List<PocketTag> GetTags();

        /// <summary>
        /// Retrieves items by tag
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        List<PocketItem> SearchByTag(string tag);

        /// <summary>
        /// Retrieves items which match the specified search string in title and URI
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
        /// <exception cref="PocketException"></exception>
        List<PocketItem> Search(string searchString, bool searchInUri = true);

        /// <summary>
        /// Finds the specified search string in title and URI for an available list of items
        /// </summary>
        /// <param name="availableItems">The available items.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Search string length has to be a minimum of 2 chars</exception>
        /// <exception cref="PocketException"></exception>
        List<PocketItem> Search(List<PocketItem> availableItems, string searchString);
    }
}