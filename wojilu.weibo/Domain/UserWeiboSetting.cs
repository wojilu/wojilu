using wojilu.Members.Users.Domain;

namespace wojilu.weibo.Domain
{
    public class UserWeiboSetting : ObjectBase<UserWeiboSetting>
    {
        public int AppId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 用户微博id
        /// </summary>
        public long WeiboUid { get; set; }

        public string WeiboName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int ExpireIn { get; set; }

        ///<summary>
        ///  目前用户绑定的微博类型,QQ,sina 目前仅打算支持这两种
        ///</summary>
        public int WeiboType { get; set; }

        /// <summary>
        /// 是否绑定
        /// </summary>
        public bool IsBind { get; set; }

        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSync { get; set; }
    }
}