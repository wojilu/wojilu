using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.User
{
    /// <summary>
    ///   Represents the user ids.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class UserIDs
    {
        private Collection<long> ids;

        /// <summary>
        ///   Gets the statuses.
        /// </summary>
        [JsonProperty("ids")]
        public Collection<long> IDs
        {
            get
            {
                if (null == ids)
                    ids = new Collection<long>();
                return ids;
            }
        }

        /// <remarks />
        [JsonProperty("next_cursor")]
        public int NextCursor { get; set; }

        /// <remarks />
        [JsonProperty("previous_cursor")]
        public int PreviousCursor { get; set; }

        /// <summary>
        ///   Gets or sets the total_number.
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }
    }
}