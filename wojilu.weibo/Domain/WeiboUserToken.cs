using wojilu.Members.Users.Domain;

namespace wojilu.weibo.Domain
{
    public class WeiboUserToken : ObjectBase<WeiboUserToken>
    {
        public int AppId { get; set; }

        public User User { get; set; }

        public long WeiboUid { get; set; }

        public string WeiboName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpireTime { get; set; }

        ///<summary>
        ///  目前用户绑定的微博类型,QQ=1,sina=0 目前仅打算支持这两种
        ///</summary>
        public int WeiboType { get; set; }
    }
}