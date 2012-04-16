using System;
using System.Xml.Serialization;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents a hyperlink.
    /// </summary>
    [Serializable]
    [XmlRoot("a")]
    public class Hyperlink
    {
        /// <remarks />
        [XmlAttribute("href")]
        public string Uri { get; set; }

        /// <remarks />
        [XmlText]
        public string Text { get; set; }
    }
}