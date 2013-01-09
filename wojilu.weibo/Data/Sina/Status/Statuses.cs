using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Status
{
    /// <summary>
    ///   Represents a set of status.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class Statuses
    {
        private Collection<StatusInfo> items;

        /// <summary>
        ///   Gets the statuses.
        /// </summary>
        [JsonProperty("statuses")]
        public Collection<StatusInfo> Items
        {
            get
            {
                if (null == items)
                    items = new Collection<StatusInfo>();
                return items;
            }
        }

        /// <summary>
        ///   Gets or sets the total count.
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalCount { get; set; }

        /// <summary>
        ///   Gets or sets the previous cursor.
        /// </summary>
        [JsonProperty("previous_cursor")]
        public long PreviousCursor { get; set; }

        /// <summary>
        ///   Gets or sets the next cursor.
        /// </summary>
        [JsonProperty("next_cursor")]
        public long NextCursor { get; set; }

        /// <summary>
        ///   Unknown.
        /// </summary>
        [JsonProperty("hasvisible")]
        public bool HasVisible { get; set; }
    }
}