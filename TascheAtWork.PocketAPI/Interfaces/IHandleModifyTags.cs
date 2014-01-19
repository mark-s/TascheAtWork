using TascheAtWork.PocketAPI.Models;

namespace TascheAtWork.PocketAPI.Interfaces
{
    public interface IHandleModifyTags
    {
        /// <summary>
        /// Adds the specified tags to an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool AddTags(int itemID, string[] tags);

        /// <summary>
        /// Adds the specified tags to an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool AddTags(PocketItem item, string[] tags);

        /// <summary>
        /// Removes the specified tags from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTags(int itemID, string[] tags);

        /// <summary>
        /// Removes the specified tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTags(PocketItem item, string[] tags);

        /// <summary>
        /// Removes a tag from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTag(int itemID, string tag);

        /// <summary>
        /// Removes a tag from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTag(PocketItem item, string tag);

        /// <summary>
        /// Clears all tags from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTags(int itemID);

        /// <summary>
        /// Clears all tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RemoveTags(PocketItem item);

        /// <summary>
        /// Replaces all existing tags with the given tags in an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool ReplaceTags(int itemID, string[] tags);

        /// <summary>
        /// Replaces all existing tags with the given new ones in an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool ReplaceTags(PocketItem item, string[] tags);

        /// <summary>
        /// Renames a tag in an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag name.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RenameTag(int itemID, string oldTag, string newTag);

        /// <summary>
        /// Renames a tag in an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag name.</param>
        /// <returns></returns>
        /// <exception cref="PocketAPIException"></exception>
        bool RenameTag(PocketItem item, string oldTag, string newTag);
    }
}