using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace wojilu.weibo.common
{
    /// <summary>
    ///   Provides utility methods for xml serialization.
    /// </summary>
    public static class XmlSerializationHelper
    {
        /// <summary>
        ///   Deserializes an object (type indicated by <typeparamref name="T" /> ) loaded from the specified <paramref
        ///    name="location" /> .
        /// </summary>
        /// <typeparam name="T"> The type of the object. </typeparam>
        /// <param name="location"> The file location. </param>
        /// <returns> The object deserialized. </returns>
        public static T LoadFromFile<T>(string location) where T : class
        {
            T result = null;
            using (var reader = new XmlTextReader(location))
            {
                var s = new XmlSerializer(typeof (T));

                result = s.Deserialize(reader) as T;
            }

            return result;
        }

        /// <summary>
        ///   Serializes the <paramref name="target" /> object into xml and then saves it into the file specified by <paramref
        ///    name="location" /> .
        /// </summary>
        /// <param name="target"> The object to serialize. </param>
        /// <param name="location"> The file location. </param>
        public static void SaveToFile(object target, string location)
        {
            using (var writer = new StreamWriter(location))
            {
                var s = new XmlSerializer(target.GetType());

                s.Serialize(writer, target);
            }
        }

        /// <summary>
        ///   Deserializes an object (type indicated by <typeparamref name="T" /> ) from the specified <paramref name="xml" /> .
        /// </summary>
        /// <typeparam name="T"> The type of the object. </typeparam>
        /// <param name="xml"> The xml string. </param>
        /// <returns> The object deserialized. </returns>
        public static T XmlToObject<T>(string xml) where T : class
        {
            return XmlToObject(typeof (T), xml) as T;
        }

        /// <summary>
        ///   Deserializes an object (type indicated by <paramref name="type" /> ) from the specified <paramref name="xml" /> .
        /// </summary>
        /// <param name="type"> The type of the object. </param>
        /// <param name="xml"> The xml string. </param>
        /// <returns> The object deserialized. </returns>
        public static object XmlToObject(Type type, string xml)
        {
            object result = null;
            using (TextReader reader = new StringReader(xml))
            {
                var s = new XmlSerializer(type);

                result = s.Deserialize(reader);
            }

            return result;
        }

        /// <summary>
        ///   Serializes the <paramref name="target" /> object into xml.
        /// </summary>
        /// <param name="target"> The object to serialize. </param>
        /// <returns> The xml string. </returns>
        public static string ObjectToXml(object target)
        {
            string result = string.Empty;
            using (Stream stream = new MemoryStream())
            {
                var s = new XmlSerializer(target.GetType());
                s.Serialize(stream, target);

                stream.Position = 0;
                var sReader = new StreamReader(stream);
                result = sReader.ReadToEnd();
            }

            return result;
        }
    }
}