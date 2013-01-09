using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Tag
{
    /// <summary>
    ///   Represents a group of tag.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class TagInfo
    {
        /// <summary>
        ///   Gets or sets the tag id.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        ///   Gets or sets the tag id.
        /// </summary>
        [JsonProperty("value")]
        public string Tag { get; set; }

        /// <summary>
        ///   Gets or sets the tag value.
        /// </summary>
        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}