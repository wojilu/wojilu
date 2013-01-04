using System;
using Newtonsoft.Json;
using wojilu.weibo.Data.Sina.Status;
using wojilu.weibo.Data.Sina.User;

namespace wojilu.weibo.Data.Sina.Comment
{
    /// <summary>
    ///   Represents a comment.
    /// </summary>
    [Serializable]
    public class CommentInfo
    {
        /// <summary>
        ///   Gets or sets the creation time of the status.
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        ///   Gets or sets the stutus id.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        ///   Gets or sets the status text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the source of the status.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        ///   Gets or sets the user who posts this comment.
        /// </summary>
        [JsonProperty("user")]
        public UserInfo User { get; set; }

        /// <summary>
        ///   Gets or sets the status which this comment comments to.
        /// </summary>
        [JsonProperty("status")]
        public StatusInfo Status { get; set; }
    }
}