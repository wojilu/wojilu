using System;
using System.Diagnostics;
using Newtonsoft.Json;
using wojilu.weibo.Data.Sina.Status;

namespace wojilu.weibo.Data.Sina.User
{
    /// <summary>
    ///   Represents a user.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("User:{ScreenName}")]
    [JsonObject]
    public class UserInfo
    {
        /// <remarks />
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <remarks />
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        /// <remarks />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the markup(note) you set for this user.
        /// </summary>
        [JsonProperty("define_as")]
        public string DefineAs { get; set; }

        /// <remarks />
        [JsonProperty("province")]
        public string Province { get; set; }

        /// <remarks />
        [JsonProperty("city")]
        public string City { get; set; }

        /// <remarks />
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <remarks />
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <remarks />
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <remarks />
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        /// <remarks />
        [JsonProperty("domain")]
        public string Domain { get; set; }

        /// <remarks />
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <remarks />
        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        /// <remarks />
        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }

        /// <remarks />
        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }

        /// <remarks />
        [JsonProperty("favourites_count")]
        public int FavouritesCount { get; set; }

        /// <remarks />
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <remarks />
        [JsonProperty("geo_enabled")]
        public bool GeoEnabled { get; set; }

        /// <remarks />
        [JsonProperty("allow_all_act_msg")]
        public bool AllowAllActMsg { get; set; }

        /// <remarks />
        [JsonProperty("following")]
        public bool Following { get; set; }

        /// <remarks />
        [JsonProperty("verified")]
        public bool Verified { get; set; }

        /// <remarks />
        [JsonProperty("status")]
        public StatusInfo LatestStatus { get; set; }
    }
}