using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents the unread counters info.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class UnreadInfo
    {
        /// <remarks />
        [JsonProperty("status")]
        public int? NewStatuses { get; set; }

        /// <remarks />
        [JsonProperty("follower")]
        public int Followers { get; set; }

        /// <remarks />
        [JsonProperty("dm")]
        public int Messages { get; set; }

        /// <remarks />
        [JsonProperty("cmt")]
        public int Comments { get; set; }

        /// <remarks />
        [JsonProperty("mention_status")]
        public int MentionStatuses { get; set; }

        /// <remarks />
        [JsonProperty("mention_cmt")]
        public int MentionComments { get; set; }

        /// <remarks />
        [JsonProperty("group")]
        public int GroupMessages { get; set; }

        /// <remarks />
        [JsonProperty("private_group")]
        public int PrivateGroupMessages { get; set; }

        /// <remarks />
        [JsonProperty("notice")]
        public int Notices { get; set; }

        /// <remarks />
        [JsonProperty("invite")]
        public int Invites { get; set; }

        /// <remarks />
        [JsonProperty("badge")]
        public int Badges { get; set; }

        /// <remarks />
        [JsonProperty("photo")]
        public int PhotoMessages { get; set; }
    }
}