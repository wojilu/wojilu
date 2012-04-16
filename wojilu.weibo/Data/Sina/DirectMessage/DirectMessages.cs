using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.DirectMessage
{
    /// <summary>
    ///   Represents a set of direct message.
    /// </summary>
    [Serializable]
    [JsonObject("direct-messages")]
    public class DirectMessages
    {
        private Collection<DirectMessageInfo> items = new Collection<DirectMessageInfo>();

        /// <summary>
        ///   Gets the direct messages.
        /// </summary>
        [JsonProperty("direct_message")]
        public Collection<DirectMessageInfo> Items
        {
            get { return items; }
        }
    }
}