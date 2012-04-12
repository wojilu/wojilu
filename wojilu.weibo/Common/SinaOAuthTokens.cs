using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the OAuth access token.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("AccessToken:{Token}")]
    [JsonObject]
    public class SinaOAuthAccessToken
    {
        /// <summary>
        ///   Gets or sets the token field.
        /// </summary>
        [JsonProperty("access_token")]
        public string Token { get; set; }

        /// <summary>
        ///   Gets or sets the refresh token field.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        ///   Gets or sets the user id field.
        /// </summary>
        [JsonProperty("uid")]
        public string UserID { get; set; }

        /// <summary>
        ///   Gets or sets the expire in field.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }


    }
}