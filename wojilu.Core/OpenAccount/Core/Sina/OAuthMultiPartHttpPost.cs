using System.Net;
using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    /// <summary>
    /// Posts the multi-part fields in the post body with the OAuth authorization header in the request.
    /// </summary>
    public class OAuthMultiPartHttpPost : MultiPartHttpPost
    {
        protected string Token;
        /// <summary>
        /// Initializes a new instance of <see cref="OAuthMultiPartHttpPost"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The uri to identify a resource in the remote server.</param>
        public OAuthMultiPartHttpPost(string uri,string token)
            : base(uri)
        {
            this.Token = token;
        }

        /// <summary>
        /// See <see cref="HttpRequest.AppendHeaders"/>.
        /// </summary>        
        protected override void AppendHeaders(WebHeaderCollection headers)
        {
            var oAuthHeader = OAuthHelper.ConstructOAuthHeader(SinaConstants.OAuthHeaderPrefix,Token);
            
            headers.Add(HttpRequestHeader.Authorization, oAuthHeader);

            base.AppendHeaders(headers);
        }
    }
}
