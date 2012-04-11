using System;
using System.Xml.Serialization;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents a boolean result.
    /// </summary>
    [Serializable]
    public class BooleanResultInfo
    {
        /// <remarks />
        [XmlText]
        public virtual bool Value { get; set; }
    }

    /// <summary>
    ///   Represents a friendship existence result.
    /// </summary>
    [Serializable]
    [XmlRoot("friends")]
    public class ExistsFriendshipResultInfo : BooleanResultInfo
    {
    }
}