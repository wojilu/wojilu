using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace wojilu.weibo.Data
{
    /// <summary>
    ///   Represents an error response from remote server.
    ///   <remarks>
    ///     {"error":"User does not exists!","error_code":20003,"request":"/2/users/show.json"}
    ///   </remarks>
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Error:{ErrorCode}:{ErrorMessage}")]
    public class ErrorResponse
    {
        /// <remarks />
        [JsonProperty("request")]
        public string Uri { get; set; }

        /// <remarks />
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        /// <remarks />
        [JsonProperty("error")]
        public string ErrorMessage { get; set; }
    }
}