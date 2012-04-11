using System;
using System.Xml.Serialization;

namespace wojilu.weibo.Data.Sina.Status
{
    /// <summary>
    ///   Represents the update status with pic.
    /// </summary>
    [Serializable]
    public class UpdateStatusWithPicInfo
    {
        /// <summary>
        ///   The status text.
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        ///   The location of pic file.
        /// </summary>
        [XmlElement("pic")]
        public string Pic { get; set; }

        /// <summary>
        ///   纬度, 有效范围：-90.0到+90.0，+表示北纬。
        /// </summary>
        [XmlElement("lat")]
        public float? Latitude { get; set; }

        /// <summary>
        ///   经度, 有效范围：-180.0到+180.0，+表示东经。
        /// </summary>
        [XmlElement("long")]
        public float? Longitude { get; set; }
    }
}