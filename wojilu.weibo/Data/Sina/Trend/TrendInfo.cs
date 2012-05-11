using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Trend
{
    /// <summary>
    ///   Represents a trend(topic).
    /// </summary>
    [Serializable]
    [JsonObject]
    public class TrendInfo
    {
        /// <summary>
        ///   Gets or sets the trend id.
        /// </summary>
        [JsonProperty("trend_id")]
        public long ID { get; set; }

        /// <summary>
        ///   Gets or sets the hotword of the trend.
        /// </summary>
        [JsonProperty("hotword")]
        public string HotWord { get; set; }

        /// <summary>
        ///   Gets or sets the number of hits the trend has.
        /// </summary>
        [JsonProperty("num")]
        public long Hits { get; set; }
    }

    /// <summary>
    ///   Represents a trend(topic).
    /// </summary>
    [Serializable]
    [JsonObject("trend")]
    public class PeriodTrendInfo
    {
        /// <summary>
        ///   Gets or sets the name of the trend.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the query of the trend.
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        ///   Gets or sets the amount of the trend.
        /// </summary>
        [JsonProperty("amount")]
        public int Mmount { get; set; }

        /// <summary>
        ///   Gets or sets the delta of the trend.
        /// </summary>
        [JsonProperty("delta")]
        public int Delta { get; set; }
    }
}