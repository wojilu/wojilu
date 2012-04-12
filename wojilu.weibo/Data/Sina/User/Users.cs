using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.User
{
    /// <summary>
    ///   Represents a group of users.
    /// </summary>
    [Serializable]
    [JsonObject("users")]
    public class Users
    {
        private Collection<UserInfo> items;

        /// <summary>
        ///   Gets the users.
        /// </summary>
        [JsonProperty("users")]
        public Collection<UserInfo> Items
        {
            get
            {
                // This is required, otherwise Json Deserialization fails.
                if (null == items)
                    items = new Collection<UserInfo>();

                return items;
            }
        }

        /// <summary>
        ///   Gets or sets the next page cursor.
        /// </summary>
        [JsonProperty("next_cursor")]
        public int NextCursor { get; set; }

        /// <summary>
        ///   Gets or sets the previous page cursor.
        /// </summary>
        [JsonProperty("previous_cursor")]
        public int PreviousCursor { get; set; }

        /// <summary>
        ///   Gets or sets the total_number.
        /// </summary>
        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }
    }
}