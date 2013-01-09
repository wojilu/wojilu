using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    /// <summary>
    /// Performs a simple/clean (non-token-attached) HTTP-Get request to the resource identified by the uri.
    /// <remarks>
    /// If there are qurey parameters, please add to <see cref="Params"/>. 
    /// They will be appended to the uri before making the request.
    /// </remarks>
    /// </summary>
    public class SinaHttpGet : SinaHttpRequest
    {
        private ParamCollection queryParams = new ParamCollection();

        /// <summary>
        /// Initializes a new instance of <see cref="SinaHttpGet"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        public SinaHttpGet(string uri)
            : base(uri)
        {
            base.Method = HttpMethod.Get;
        }

        /// <summary>
        /// Gets or sets parameters to be sent in the request query string.
        /// </summary>
        public virtual ParamCollection Params
        {
            get
            {
                return queryParams;
            }
        }

        /// <summary>
        /// Constructs the uri by appending the query parameters in <see cref="Params"/>.
        /// </summary>
        /// <example>
        /// For example: 
        /// Before: http://api.t.sina.com.cn/statuses/public_timeline.xml 
        /// After: http://api.t.sina.com.cn/statuses/public_timeline.xml?source=123123. 
        /// </example>
        /// <returns>The uri with query string appended.</returns>
        protected override string ConstructUri()
        {
            var uri = Uri;
            if (null != Params)
            {
                uri += "?";
                foreach (var item in Params)
                {
                    uri += string.Format("{0}={1}&", RFC3986Encoder.UrlEncode(item.Name), RFC3986Encoder.UrlEncode(item.Value));
                }
                uri = uri.TrimEnd('&');
            }

            return uri;
        }
    }
}
