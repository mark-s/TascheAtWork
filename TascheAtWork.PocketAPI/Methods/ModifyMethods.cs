using TascheAtWork.PocketAPI.Interfaces;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;

namespace TascheAtWork.PocketAPI.Methods
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public class ModifyMethods : IHandleModify
    {
        private readonly IInternalAPI _client;


        public ModifyMethods(IInternalAPI client)
        {
            _client = client;
        }

        /// <summary>
        /// Archives the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Archive(int itemID)
        {
            return SendDefault(itemID, "archive");
        }


        /// <summary>
        /// Archives the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Archive(PocketItem item)
        {
            return Archive(item.ID);
        }


        /// <summary>
        /// Un-archives the specified item (alias for Readd).
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Unarchive(int itemID)
        {
            return SendDefault(itemID, "readd");
        }


        /// <summary>
        /// Unarchives the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Unarchive(PocketItem item)
        {
            return Unarchive(item.ID);
        }


        /// <summary>
        /// Favorites the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Favorite(int itemID)
        {
            return SendDefault(itemID, "favorite");
        }


        /// <summary>
        /// Favorites the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Favorite(PocketItem item)
        {
            return Favorite(item.ID);
        }


        /// <summary>
        /// Un-favorites the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Unfavorite(int itemID)
        {
            return SendDefault(itemID, "unfavorite");
        }


        /// <summary>
        /// Un-favorites the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Unfavorite(PocketItem item)
        {
            return Unfavorite(item.ID);
        }


        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        public bool Delete(int itemID)
        {
            return SendDefault(itemID, "delete");
        }


        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Delete(PocketItem item)
        {
            return Delete(item.ID);
        }


        /// <summary>
        /// Puts an action
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private bool SendDefault(int itemID, string action)
        {
            return _client.Send(new ActionParameter()
            {
                Action = action,
                ID = itemID
            });
        }
    }
}