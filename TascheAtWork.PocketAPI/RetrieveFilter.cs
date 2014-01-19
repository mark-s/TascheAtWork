namespace TascheAtWork.PocketAPI
{
    /// <summary>
    /// Filter for simple retrieve requests
    /// </summary>
    public enum RetrieveFilter
    {
        /// <summary>
        /// All types
        /// </summary>
        All,

        /// <summary>
        /// Only unread items
        /// </summary>
        Unread,

        /// <summary>
        /// Archived items
        /// </summary>
        Archive,

        /// <summary>
        /// Favorited items
        /// </summary>
        Favorite,

        /// <summary>
        /// Only articles
        /// </summary>
        Article,

        /// <summary>
        /// Only videos
        /// </summary>
        Video,

        /// <summary>
        /// Only images
        /// </summary>
        Image
    }
}