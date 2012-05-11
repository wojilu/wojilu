using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace wojilu.weibo.Data.Sina.Common
{
    /// <summary>
    ///   Represents a geo point.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class GeoPoint
    {
        /// <summary>
        ///   Gets or sets the geo content (in string form)
        /// </summary>
        /// <example>
        ///   125.12 253.62
        /// </example>
        [XmlText]
        [JsonProperty]
        public string Content { get; set; }

        /// <summary>
        ///   Gets the latitude.
        /// </summary>
        [JsonIgnore]
        public string Lat
        {
            get
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    string[] groups = Content.Split(' ');
                    if (1 < groups.Length)
                        return groups[0];
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///   Gets the longitude.
        /// </summary>
        [JsonIgnore]
        public string Long
        {
            get
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    string[] groups = Content.Split(' ');
                    if (1 < groups.Length)
                        return groups[1];
                }

                return string.Empty;
            }
        }
    }
}