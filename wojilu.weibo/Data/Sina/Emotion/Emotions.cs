using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Emotion
{
    /// <summary>
    ///   Represents a group of emotions.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class Emotions
    {
        private Collection<EmotionInfo> emotions;

        /// <summary>
        ///   Gets the emotions.
        /// </summary>
        [JsonProperty("emotions")]
        public Collection<EmotionInfo> Items
        {
            get
            {
                if (null == emotions)
                    emotions = new Collection<EmotionInfo>();
                return emotions;
            }
        }
    }
}