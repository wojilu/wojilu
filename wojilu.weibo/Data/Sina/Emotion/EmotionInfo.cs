using System;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Emotion
{
    /// <summary>
    ///   Represents the emotion info.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class EmotionInfo
    {
        /*
            <phrase>[嘻嘻]</phrase>
            <type>image</type>
            <url>http://timg.sjs.sinajs.cn/miniblog2style/images/common/face/ext/normal/c2/tooth.gif</url>
            <is_hot>false</is_hot>
            <is_common>true</is_common>
            <order_number>96</order_number>\
            <category>表情</category>
         */

        /// <summary>
        ///   Gets or sets the phrase represents the emotion.
        /// </summary>
        [JsonProperty("phrase")]
        public string Phrase { get; set; }

        /// <summary>
        ///   Gets or sets the type of the emotion.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        ///   Gets or sets the url of the emotion.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        ///   Gets or sets a boolean value indicating whether the emotion is popular.
        /// </summary>
        [JsonProperty("is_hot")]
        public bool IsHot { get; set; }

        /// <summary>
        ///   Gets or sets a boolean value indicating whether the emotion is common.
        /// </summary>
        [JsonProperty("is_common")]
        public bool IsCommon { get; set; }

        /// <summary>
        ///   Gets or sets order numer of the emotion.
        /// </summary>
        [JsonProperty("order_number")]
        public int OrderNumber { get; set; }

        /// <summary>
        ///   Gets or sets the category of the emotion.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}