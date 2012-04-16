using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Relationship
{
    /// <summary>
    ///   Represents the relationship info of two users.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class RelationshipInfo
    {
        /// <remarks />
        [JsonProperty("source")]
        public RelationshipPart Source { get; set; }

        /// <remarks />
        [JsonProperty("target")]
        public RelationshipPart Target { get; set; }
    }
}