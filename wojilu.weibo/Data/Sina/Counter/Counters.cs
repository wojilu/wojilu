using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Counter
{
    /// <summary>
    ///   Represents a group of counters.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class Counters
    {
        private readonly Collection<CounterInfo> items = new Collection<CounterInfo>();

        /// <summary>
        ///   Gets the counter.
        /// </summary>
        [JsonProperty("count")]
        public Collection<CounterInfo> Items
        {
            get { return items; }
        }
    }
}