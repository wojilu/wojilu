using wojilu.weibo.Common;

namespace wojilu.weibo.Core.Sina
{
    public class ObtainTokenPost : HttpPost
    {
        public ObtainTokenPost(string uri)
            : base(uri)
        { }

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
