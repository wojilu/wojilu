using System;

namespace wojilu.weibo.Domain
{
    [Serializable]
    public class WeiboSetting
    {
        public bool Enable { get; set; }

        public bool UseSina { get; set; }

        public bool UseQQ { get; set; }

        public string SinaApiKey { get; set; }

        public string SinaApiSerect { get; set; }

        public string QQApiKey { get; set; }

        public string QQApiSerect { get; set; }

        public string SinaCallbackUrl { get; set; }

        public string QQCallbackUrl { get; set; }
    }
}