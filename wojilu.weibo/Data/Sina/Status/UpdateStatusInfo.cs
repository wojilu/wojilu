using System;
using System.Xml.Serialization;

namespace wojilu.weibo.Data.Sina.Status
{
    /// <summary>
    ///   Represents the update status info.
    /// </summary>
    [Serializable]
    public class UpdateStatusInfo
    {
        /// <summary>
        ///   Gets or sets the status text.
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        ///   Gets or sets the id of a replying status.
        /// </summary>
        [XmlElement("in_reply_to_status_id")]
        public long? ReplyTo { get; set; }

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

        /// <summary>
        ///   Gets or sets a json format string for annotation.
        /// </summary>
        [XmlElement("annotations")]
        public string Annotations { get; set; }
    }
}