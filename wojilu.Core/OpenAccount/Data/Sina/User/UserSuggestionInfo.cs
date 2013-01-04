using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.User
{
    /// <summary>
    ///   Represents the user suggestion info.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class UserSuggestionInfo
    {
        /// <remarks />
        [JsonProperty("uid")]
        public long ID { get; set; }

        /// <remarks />
        [JsonProperty("nickname")]
        public string ScreenName { get; set; }

        /// <remarks />
        [JsonProperty("remark")]
        public string Remark { get; set; }
    }
}