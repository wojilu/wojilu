using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Counter
{
    /// <summary>
    ///   Represents a counter info of a status.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class CounterInfo
    {
        /// <remarks />
        [JsonProperty("id")]
        public long StatusID { get; set; }

        /// <remarks />
        [JsonProperty("comments")]
        public int Comments { get; set; }

        /// <remarks />
        [JsonProperty("rt")]
        public int Forwards { get; set; }
    }
}