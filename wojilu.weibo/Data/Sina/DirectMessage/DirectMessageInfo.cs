using System;
using Newtonsoft.Json;
using wojilu.weibo.Data.Sina.User;

namespace wojilu.weibo.Data.Sina.DirectMessage
{
    /// <summary>
    ///   Represents the direct message.
    /// </summary>
    [Serializable]
    public class DirectMessageInfo
    {
        /// <summary>
        ///   Gets or sets the creation time of the direct message.
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        ///   Gets or sets the direct message id.
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        ///   Gets or sets the direct message's text.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///   Gets or sets the sender ID of the direct message.
        /// </summary>
        [JsonProperty("sender_id")]
        public long SenderID { get; set; }

        /// <summary>
        ///   Gets or sets the receiver ID of the direct message.
        /// </summary>
        [JsonProperty("recipient_id")]
        public long RecipientID { get; set; }

        /// <summary>
        ///   Gets or sets the sender's screen name.
        /// </summary>
        [JsonProperty("sender_screen_name")]
        public string SenderScreenName { get; set; }

        /// <summary>
        ///   Gets or sets the receiver's screen name.
        /// </summary>
        [JsonProperty("recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        /// <summary>
        ///   Gets or sets the sender of the direct message.
        /// </summary>
        [JsonProperty("sender")]
        public UserInfo Sender { get; set; }

        /// <summary>
        ///   Gets or sets the recipient of the direct message.
        /// </summary>
        [JsonProperty("recipient")]
        public UserInfo Recipient { get; set; }
    }
}