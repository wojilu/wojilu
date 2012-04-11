using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents the source that a status is from.
    /// </summary>
    [Serializable]
    [JsonObject("source")]
    public class Source
    {
        /// <summary>
        ///   Gets or sets the content of the source object.
        /// </summary>
        [JsonProperty("a")]
        public Hyperlink Content { get; set; }
    }
}