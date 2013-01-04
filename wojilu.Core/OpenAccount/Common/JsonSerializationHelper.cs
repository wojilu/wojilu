using Newtonsoft.Json;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Provides methods for JSON serialization.
    /// </summary>
    public static class JsonSerializationHelper
    {
        /// <summary>
        ///   Deserializers a JSON string into an strong-typed object.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="jsonStr"> </param>
        /// <returns> </returns>
        public static T JsonToObject<T>(string jsonStr) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }


        public static string ObjectToJson(object target)
        {
            return JsonConvert.SerializeObject(target);
        }
    }
}