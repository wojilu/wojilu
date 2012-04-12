using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Relationship
{
    /// <summary>
    ///   Represents a relationship part.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class RelationshipPart
    {
        /// <remarks />
        [JsonProperty("id")]
        public long UserID { get; set; }

        /// <remarks />
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        /// <remarks />
        [JsonProperty("following")]
        public bool Following { get; set; }

        /// <remarks />
        [JsonProperty("followed_by")]
        public bool FollowedBy { get; set; }

        /// <remarks />
        [JsonProperty("notifications_enabled")]
        public bool NotificationsEnabled { get; set; }
    }
}