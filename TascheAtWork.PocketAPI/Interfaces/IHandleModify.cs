using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IHandleModify
    {
        /// <summary>
        /// Archives the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Archive(int itemID);

        /// <summary>
        /// Archives the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Archive(PocketItem item);

        /// <summary>
        /// Un-archives the specified item (alias for Readd).
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Unarchive(int itemID);

        /// <summary>
        /// Unarchives the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Unarchive(PocketItem item);

        /// <summary>
        /// Favorites the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Favorite(int itemID);

        /// <summary>
        /// Favorites the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Favorite(PocketItem item);

        /// <summary>
        /// Un-favorites the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Unfavorite(int itemID);

        /// <summary>
        /// Un-favorites the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Unfavorite(PocketItem item);

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool Delete(int itemID);

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool Delete(PocketItem item);

    }
}