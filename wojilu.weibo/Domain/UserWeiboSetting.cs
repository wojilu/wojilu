using wojilu.Members.Users.Domain;
using wojilu.ORM;
using System;

namespace wojilu.weibo.Domain
{
    [Table("UsersWeiboSettings")]
    public class UserWeiboSetting : ObjectBase<UserWeiboSetting>
    {
        public int AppId { get; set; }

        private User _user;

        [NotSave]
        public User User
        {
            get
            {
                if (_user==null)
                {
                    _user = User.findById(UserId);
                }
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public int UserId { get; set; }

        /// <summary>
        /// 用户微博id
        /// </summary>
        public string WeiboUid { get; set; }

        public string WeiboName { get; set; }

        public string AccessToken { get; set; }

        /// <summary>
        /// 适用于oauth 1.0
        /// </summary>
        public string AccessSecrct { get; set; }

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
        /// 是否同步
        /// </summary>
        public int IsSync { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindTime { get; set; }


        /// <summary>
        /// 是否token过期,采用oauth 2.0认证方式需判断此参数，过期需要refresh token
        /// </summary>
        [NotSave]
        public bool IsExpire
        {
            get
            {
              return  (DateTime.Now - BindTime).TotalSeconds > ExpireIn;
            }
        }
    }
}