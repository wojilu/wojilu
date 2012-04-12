using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    /// <summary>
    /// Performs a OAuth token-attached HTTP-Delete request to the resource identified by the <c>uri</c>.
    /// </summary>
    public class OAuthHttpDelete : OAuthHttpGet
    {
        /// <summary>
        /// Initializes a new instance of <see cref="OAuthHttpDelete"/> with the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri"></param>
        public OAuthHttpDelete(string uri,string token)
            : base(uri,token)
        {
            base.Method = HttpMethod.Delete;
        }
    }
}
