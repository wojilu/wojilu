using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Url
{
    /// <summary>
    ///   Represents a short and long url mapping.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class UrlInfo
    {
        /// <summary>
        ///   Gets or sets the short url.
        /// </summary>
        [JsonProperty("url_short")]
        public string ShortUrl { get; set; }

        /// <summary>
        ///   Gets or sets the long url.
        /// </summary>
        [JsonProperty("url_long")]
        public string LongUrl { get; set; }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        ///   Gets or sets the share count of the short url.
        /// </summary>
        [JsonProperty("share_counts")]
        public int SharedCount { get; set; }

        /// <summary>
        ///   Gets or sets the comment count of the short url.
        /// </summary>
        [JsonProperty("comment_counts")]
        public int CommentCount { get; set; }
    }
}