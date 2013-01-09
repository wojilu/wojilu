using System;
using System.Xml.Serialization;

namespace wojilu.weibo.Data.Sina.Account
{
    /// <summary>
    ///   Represents the update profile info.
    /// </summary>
    [Serializable]
    public class UpdateProfileInfo
    {
        /// <summary>
        ///   Gets or sets the screen name.
        /// </summary>
        [XmlElement("name")]
        public string ScreenName { get; set; }

        /// <summary>
        ///   Gets or sets the gender. f: female, m: male
        /// </summary>
        [XmlElement("gender")]
        public string Gender { get; set; }

        /// <summary>
        ///   Gets or sets the province.
        /// </summary>
        /// <remarks>
        ///   See http://open.weibo.com/wiki/%E7%9C%81%E4%BB%BD%E5%9F%8E%E5%B8%82%E7%BC%96%E7%A0%81%E8%A1%A8
        /// </remarks>
        [XmlElement("province")]
        public int? Province { get; set; }

        /// <summary>
        ///   Gets or sets the city. 1000 means unspecified
        /// </summary>
        /// <remarks>
        ///   See http://open.weibo.com/wiki/%E7%9C%81%E4%BB%BD%E5%9F%8E%E5%B8%82%E7%BC%96%E7%A0%81%E8%A1%A8
        /// </remarks>
        [XmlElement("city")]
        public int? City { get; set; }

        /// <summary>
        ///   Gets or sets the personal description. Less than 70 chinese chars.
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }
    }
}