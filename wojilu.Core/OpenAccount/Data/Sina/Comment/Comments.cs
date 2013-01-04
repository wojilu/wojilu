using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using wojilu.weibo.Data.Sina.Comment;

namespace wojilu.weibo.Data.Comment
{
    /// <summary>
    ///   Represents a set of comment.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class Comments
    {
        private Collection<CommentInfo> items = new Collection<CommentInfo>();

        /// <summary>
        ///   Gets the comments.
        /// </summary>
        [JsonProperty("comments")]
        public Collection<CommentInfo> Items
        {
            get
            {
                if (null == items)
                    items = new Collection<CommentInfo>();
                return items;
            }
        }

        /// <summary>
        ///   Gets or sets the previous cursor.
        /// </summary>
        [JsonProperty("previous_cursor")]
        public long PreviousCursor { get; set; }

        /// <summary>
        ///   Gets or sets the next cursor.
        /// </summary>
        [JsonProperty("next_cursor")]
        public long NextCursor { get; set; }

        /// <summary>
        ///   Unknown.
        /// </summary>
        [JsonProperty("hasvisible")]
        public bool HasVisible { get; set; }
    }
}