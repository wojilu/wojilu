using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core
{
    public class SupportWeiboType
    {
        public const string Sina="sina";
        public const string QQWeibo = "qqweibo";
        public const string WangYi163 ="wangyi";
        public const string Douban = "douban";
        public const string Renren= "renren";

        public static List<string> GetSupports()
        {
            return new List<string>() { Sina, QQWeibo, WangYi163, Douban, Renren };
        }
    }   
}
