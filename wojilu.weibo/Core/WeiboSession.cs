using System;
using System.Collections.Generic;
using System.Text;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Core
{
    [Serializable]
    public class WeiboSession
    {
        public static readonly string SessionName = typeof(WeiboSession).ToString();

        public WeiboSession(UserWeiboSetting setting):this(setting,string.Empty)
        {

        }

        public WeiboSession(UserWeiboSetting setting,string weiboFriendName)
            : this(setting, string.Empty,weiboFriendName, string.Empty)
        {

        }

        public WeiboSession(UserWeiboSetting setting,string screenName,string friendName,string profileImg)
        {
            this.Setting = setting;
            this.ScreenName = screenName;
            this.WeiboFriendName = friendName;
            this.ProfileImg = profileImg;
        }

        /// <summary>
        /// 用户微博名
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// 微博友好名称 新浪微博
        /// </summary>
        public string WeiboFriendName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string ProfileImg { get; set; }

        public UserWeiboSetting Setting { get; set; }


    }
}
