using System;

namespace wojilu.weibo.Data.Sina.Emotion
{
    /// <summary>
    ///   Represents the type of emotion.
    /// </summary>
    [Serializable]
    public enum EmotionType
    {
        /// <summary>
        ///   Represents the image type of emotion
        /// </summary>
        Image,

        /// <summary>
        ///   Represents the magic type of emotion
        /// </summary>
        Magic,

        /// <summary>
        ///   Represents the cartoon type of emotion
        /// </summary>
        Cartoon
    }
}