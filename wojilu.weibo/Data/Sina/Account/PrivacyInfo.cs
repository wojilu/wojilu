using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Account
{
    /// <summary>
    ///   Represents the user privacy info.
    /// </summary>
    /// <remarks>
    ///   comment ： 谁可以评论此账号的微薄。0：所有人，1：我关注的人。 dm ： 谁可以给此账号发私信。0：所有人，1：我关注的人。 real_name： 是否允许别人通过真实姓名搜索到我， 0允许，1不允许。 geo ： 发布微博，是否允许微博保存并显示所处的地理位置信息。 0允许，1不允许。 badge ： 勋章展现状态，值—1私密状态，0公开状态。
    /// </remarks>
    [Serializable]
    [JsonObject]
    public class PrivacyInfo
    {
        /// <remarks />
        [JsonProperty("badge")]
        public int BadgePrivacy { get; set; }

        /// <remarks />
        [JsonProperty("comment")]
        public int CommentPrivacy { get; set; }

        /// <remarks />
        [JsonProperty("geo")]
        public int GeoPrivacy { get; set; }

        /// <remarks />
        [JsonProperty("message")]
        public int MessagePrivacy { get; set; }

        /// <remarks />
        [JsonProperty("mobile")]
        public int MobilePrivacy { get; set; }

        /// <remarks />
        [JsonProperty("realname")]
        public int RealNamePrivacy { get; set; }

        /// <remarks />
        [JsonProperty("webim")]
        public int WebIMPrivacy { get; set; }
    }
}