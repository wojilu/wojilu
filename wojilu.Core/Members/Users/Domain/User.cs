/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Web;
using System.Collections;

using wojilu.ORM;
using wojilu.Drawing;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

using wojilu.Common;
using wojilu.Common.Resource;
using wojilu.Common.Security;
using wojilu.Web;

namespace wojilu.Members.Users.Domain {

    [Table("Users")]
    [Serializable]
    public class User : ObjectBase<User>, IUser, IShareData, IHits {

        public String GetUrl() {
            return "space";
        }

        public User() { }
        public User( int id ) { Id = id; }

        [Column( Length = 20 )]
        public String Name { get; set; }
        [Column( Length = 100 )]
        public String Pwd { get; set; }
        public String PwdSalt { get; set; }
        [Column( Length = 20 )]
        public String RealName { get; set; }
        [Column( Length = 20 )]
        public String Title { get; set; }
        public String Url { get; set; }

        public int ProfileId { get; set; }
        public int GroupId { get; set; }

        // 网站对用户的分类，待扩展功能
        public int CategoryId { get; set; }
        public int TemplateId { get; set; }

        public String Email { get; set; }
        public String QQ { get; set; }
        public String MSN { get; set; }

        public int ProvinceId1 { get; set; }
        public int ProvinceId2 { get; set; }
        [Column( Length = 10 )]
        public String City1 { get; set; }
        [Column( Length = 10 )]
        public String City2 { get; set; }

        public int Gender { get; set; }
        public int Relationship { get; set; }
        public int Blood { get; set; }
        public int Degree { get; set; }
        public int Zodiac { get; set; }

        public int BirthYear { get; set; }
        public int BirthMonth { get; set; }
        public int BirthDay { get; set; }

        public int getAge () {
            return DateTime.Now.Year - BirthYear;
        }

        public int FriendCount { get; set; } // 好友数
        public int MemberCount { get; set; }
        public int FollowingCount { get; set; } // 关注的人数
        public int FollowersCount { get; set; } // 粉丝数

        public int Credit { get; set; } // cache from [UserIncome] KeyCurrency

        public int LoginCount { get; set; }
        public int LoginDay { get; set; }
        public int MsgCount { get; set; } // 短消息数量

        public int MsgNewCount { get; set; } // 未读短信数
        public int NewNotificationCount { get; set; } // 未读通知数
        public int PostCount { get; set; } // 论坛帖子数

        public int MicroblogAt { get; set; } // 微博at提到的数量
        public int MicroblogAtUnread { get; set; } // (未读)微博at提到的数量

        public int Pins { get; set; }
        public int Likes { get; set; }

        public int Hits { get; set; }

        [Column( Length = 30 )]
        public String LastLoginIp { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public int Status { get; set; }
        [LongText]
        public String Signature { get; set; } // 论坛签名
        public String Security { get; set; }
        public DateTime Created { get; set; }

        public int RankId { get; set; }

        public int IsEmailConfirmed { get; set; } // 是否邮件激活了

        public int IsBind { get; set; } // 是否绑定过第三方帐号
        public int LoginType { get; set; } // 登录类型(本地登录为空，第三方登录的值是 UserConnect.Id)

        private int _roleId;
        private String _pic;

        [Column( Length = 150 )]
        public String Pic {
            get {
                if (strUtil.IsNullOrEmpty( _pic )) return UserFactory.Guest.Pic;
                return _pic;
            }
            set { _pic = value; }
        }

        public int RoleId {
            get {
                if (this.Id > 0 && _roleId == 0) {
                    _roleId = SiteRole.NormalMember.Id;
                }
                return _roleId;
            }
            set { _roleId = value; }
        }

        //----------------------------------------------------------------

        [NotSave]
        public Boolean IsRegisterUser {
            get { return this.Id > 0; }
        }

        //----------------------------------------------------------------


        [NotSave]
        public String PicM {
            get { return sys.Path.GetAvatarThumb( this.Pic, "m" ); }
        }

        [NotSave]
        public String PicSX {
            get { return sys.Path.GetAvatarThumb( this.Pic, "sx" ); }
        }

        [NotSave]
        public String PicO {
            get { return sys.Path.GetAvatarOriginal( this.Pic ); }
        }

        [NotSave]
        public String PicSmall {
            get { return sys.Path.GetAvatarThumb( this.Pic, "s" ); }
        }

        public Boolean HasUploadPic() {
            if (strUtil.IsNullOrEmpty( this.Pic )) return false;
            if (this.Pic == UserFactory.Guest.Pic) return false;
            return true;
        }

        //----------------------------------------------------------------

        [NotSave]
        public String GenderString {
            get { return AppResource.Gender.GetName( this.Gender ); }
        }

        [NotSave]
        public String IncomeInfo { get; set; }

        private MemberProfile _profile;
        [NotSave]
        public MemberProfile Profile {
            get {
                if (_profile == null) {
                    _profile = MemberProfile.findById( ProfileId );
                }
                return _profile;
            }
        }

        private SiteRole _role;

        [NotSave]
        public SiteRole Role {
            get {
                if (_role == null) {
                    _role = SiteRole.GetById( this.Id, RoleId );
                }
                return _role;
            }
        }

        private SiteRank _siteRank;

        [NotSave]
        public SiteRank Rank {
            get {
                if (_siteRank == null) {
                    _siteRank = SiteRank.GetById( this.RankId );
                }
                return _siteRank;
            }
        }

        public IShareInfo GetShareInfo() {
            return new MemberFeed( this );
            //return null;
        }

        // TODO 个人空间所具有的角色
        public IList GetRoles() {
            return new ArrayList();
        }

        public IRole GetUserRole( IMember user ) {
            return null;
        }

        public IRole GetAdminRole() {
            return null;
        }

        // 在 null object 模式下保存真实的ID
        private int _realId;
        [NotSave]
        public int RealId {
            get {
                if (_realId <= 0) return this.Id;
                return _realId;
            }
            set { _realId = value; }
        }

        /// <summary>
        /// 显示的名称：UserName用户名，RealName真实姓名
        /// </summary>
        [NotSave]
        public string DisplayName
        {
            get
            {
                if (config.Instance.Site.UserDisplayName == Config.UserDisplayNameType.RealName && String.IsNullOrEmpty(this.RealName) != true)
                    return this.RealName;
                else if (config.Instance.Site.UserDisplayName == Config.UserDisplayNameType.Name)
                    return this.Name;
                else if (String.IsNullOrEmpty(this.RealName) != true)
                    return this.RealName;
                else
                    return this.Name;
            }
        }
    }
}

