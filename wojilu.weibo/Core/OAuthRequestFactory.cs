using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.weibo.Core
{
    public static class OAuthRequestFactory
    {
        public static IOAuthRequestStrategy GetStrategy(string name)
        {
            switch (name.ToLower())
            {
                case SupportWeiboType.Sina:
                    return new SinaOAuthRequestStrategy();
                case SupportWeiboType.QQWeibo:
                    return new QQOAuthRequestStagtegy();
                case SupportWeiboType.WangYi163:
                case SupportWeiboType.Douban:
                case SupportWeiboType.Renren:
                default:
                    return null;
            }
        }

        public static IOAuthRequestStrategy GetSinaStrategy()
        {
            return GetStrategy(SupportWeiboType.Sina);
        }

        public static IOAuthRequestStrategy GetQQWeiboStrategy()
        {
            return GetStrategy(SupportWeiboType.QQWeibo);
        }
    }
}
