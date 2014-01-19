using System.Threading.Tasks;
using TascheAtWork.PocketAPI.Models;
using TascheAtWork.PocketAPI.Models.Parameters;

namespace TascheAtWork.PocketAPI
{
    /// <summary>
    /// PocketClient
    /// </summary>
    public partial class ClientCore
    {
        /// <summary>
        /// Adds the specified tags to an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool AddTags(int itemID, string[] tags)
        {
            return SendTags(itemID, "tags_add", tags);
        }


        /// <summary>
        /// Adds the specified tags to an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool AddTags(PocketItem item, string[] tags)
        {
            return AddTags(item.ID, tags);
        }


        /// <summary>
        /// Removes the specified tags from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTags(int itemID, string[] tags)
        {
            return SendTags(itemID, "tags_remove", tags);
        }


        /// <summary>
        /// Removes the specified tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTags(PocketItem item, string[] tags)
        {
            return RemoveTags(item.ID, tags);
        }


        /// <summary>
        /// Removes a tag from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tag.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTag(int itemID, string tag)
        {
            return SendTags(itemID, "tags_remove", new string[] {tag});
        }


        /// <summary>
        /// Removes a tag from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTag(PocketItem item, string tag)
        {
            return RemoveTag(item.ID, tag);
        }


        /// <summary>
        /// Clears all tags from an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTags(int itemID)
        {
            return SendDefault(itemID, "tags_clear");
        }


        /// <summary>
        /// Clears all tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RemoveTags(PocketItem item)
        {
            return RemoveTags(item.ID);
        }


        /// <summary>
        /// Replaces all existing tags with the given tags in an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool ReplaceTags(int itemID, string[] tags)
        {
            return SendTags(itemID, "tags_replace", tags);
        }


        /// <summary>
        /// Replaces all existing tags with the given new ones in an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool ReplaceTags(PocketItem item, string[] tags)
        {
            return ReplaceTags(item.ID, tags);
        }


        /// <summary>
        /// Renames a tag in an item.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag name.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RenameTag(int itemID, string oldTag, string newTag)
        {
            return Send(new ActionParameter()
            {
                Action = "tag_rename",
                ID = itemID,
                OldTag = oldTag,
                NewTag = newTag
            });
        }


        /// <summary>
        /// Renames a tag in an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="oldTag">The old tag.</param>
        /// <param name="newTag">The new tag name.</param>
        /// <returns></returns>
        /// <exception cref="PocketException"></exception>
        public bool RenameTag(PocketItem item, string oldTag, string newTag)
        {
            return RenameTag(item.ID, oldTag, newTag);
        }


        /// <summary>
        /// Puts the send action for tags.
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <param name="action">The action.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        protected bool SendTags(int itemID, string action, string[] tags)
        {
            return Send(new ActionParameter()
            {
                Action = action,
                ID = itemID,
                Tags = tags
            });
        }
    }
}