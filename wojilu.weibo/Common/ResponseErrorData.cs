namespace wojilu.weibo.Common
{
    /// <summary>
    ///   Represents the contextual data of a HTTP response error.
    /// </summary>
    public class SinaResponseErrorData
    {
        /// <summary>
        ///   Gets the request uri (absolute url, probably with query strings if it's not a POST request).
        /// </summary>
        public string RequestUri { get; internal set; }

        /// <summary>
        ///   Gets the error code returned by server.
        /// </summary>
        public int ErrorCode { get; internal set; }

        /// <summary>
        ///   Gets The exception of <see cref="AMicroblogException" /> type.
        /// </summary>
        public SinaWeiboException Exception { get; internal set; }

        /// <summary>
        ///   Gets the rquest method.
        /// </summary>
        public string HttpMethod { get; internal set; }

        /// <summary>
        ///   Gets the rquest content type.
        /// </summary>
        public string ContentType { get; internal set; }
    }
}