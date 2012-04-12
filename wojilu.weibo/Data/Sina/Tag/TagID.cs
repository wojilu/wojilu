using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Tag
{
    /// <summary>
    ///   Represents a tag id.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class TagID
    {
        /// <summary>
        ///   Gets the statuses.
        /// </summary>
        [JsonProperty("tagid")]
        public long ID { get; set; }
    }
}