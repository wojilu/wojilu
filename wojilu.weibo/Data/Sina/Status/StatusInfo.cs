using System;
using Newtonsoft.Json;
using wojilu.weibo.Data.Sina.Common;
using wojilu.weibo.Data.Sina.User;

namespace wojilu.weibo.Data.Sina.Status
{
    /// <summary>
    ///   Represents the status info.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class StatusInfo
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
        ///   Gets or sets a boolead indicates whether the status is favarited.
        /// </summary>
        [JsonProperty("favorited")]
        public bool Favorited { get; set; }

        /// <summary>
        ///   Gets or sets a boolead indicates whether the status is truncated.
        /// </summary>
        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        /// <summary>
        ///   Gets or sets the mid.
        /// </summary>
        [JsonProperty("in_reply_to_status_id")]
        public string ReplyTo { get; set; }

        /// <summary>
        ///   Gets or sets the mid.
        /// </summary>
        [JsonProperty("in_reply_to_user_id")]
        public string ReplyToUserId { get; set; }

        /// <summary>
        ///   Gets or sets the mid.
        /// </summary>
        [JsonProperty("in_reply_to_screen_name")]
        public string ReplyToUserScreenName { get; set; }

        /// <summary>
        ///   Gets or sets the thumbnail_pic.
        /// </summary>
        [JsonProperty("thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        /// <summary>
        ///   Gets or sets the bmiddle_pic.
        /// </summary>
        [JsonProperty("bmiddle_pic")]
        public string MiddlePic { get; set; }

        /// <summary>
        ///   Gets or sets the original_pic.
        /// </summary>
        [JsonProperty("original_pic")]
        public string OriginalPic { get; set; }

        /// <summary>
        ///   Gets or sets the mid.
        /// </summary>
        [JsonProperty("mid")]
        public string Mid { get; set; }

        /// <summary>
        ///   Gets or sets the user who posts this status.
        /// </summary>
        [JsonProperty("user")]
        public UserInfo User { get; set; }

        /// <summary>
        ///   Gets or sets the user who posts this status.
        /// </summary>
        [JsonProperty("geo")]
        public Geo Geo { get; set; }

        /// <summary>
        ///   Gets or sets the status that current status is reposted with.
        /// </summary>
        [JsonProperty("retweeted_status")]
        public StatusInfo RetweetedStatus { get; set; }
    }
}