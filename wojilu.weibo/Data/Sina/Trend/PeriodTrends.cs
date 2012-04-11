using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Trend
{
    /// <summary>
    ///   Represents a group of trends(topic).
    /// </summary>
    [Serializable]
    [JsonObject]
    public class PeriodTrends
    {
        /// <summary>
        ///   Gets or sets the time of the trend.
        /// </summary>
        [JsonProperty("trends")]
        public PeriodTrendsItems Trends { get; set; }

        /// <summary>
        ///   Gets or sets the as-of of the trend.
        /// </summary>
        [JsonProperty("as_of")]
        public long? AsOf { get; set; }
    }

    /// <summary>
    ///   Represents the items of period trends.
    /// </summary>
    [JsonObject]
    public class PeriodTrendsItems
    {
        private Collection<PeriodTrendInfo> items;

        /// <summary>
        ///   Gets the trends.
        /// </summary>
        [JsonProperty("trend")]
        public Collection<PeriodTrendInfo> Items
        {
            get
            {
                if (null == items)
                    items = new Collection<PeriodTrendInfo>();

                return items;
            }
        }

        /// <summary>
        ///   Gets or sets the time of the trend.
        /// </summary>
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}