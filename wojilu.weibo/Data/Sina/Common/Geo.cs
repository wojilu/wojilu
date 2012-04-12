using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents a geo.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class Geo
    {
        /// <summary>
        ///   Gets or sets the geo point.
        /// </summary>
        [JsonProperty("point")]
        public GeoPoint Point { get; set; }
    }
}