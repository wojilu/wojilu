/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using System.Collections.Generic;
using wojilu.Drawing;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Comments;

namespace wojilu.Common.Microblogs.Domain {

    [Serializable]
    public class Microblog : ObjectBase<Microblog>, IAppData, ILike, ICommentTarget {


        /// <summary>
        /// 微博作者
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 转发微博
        /// </summary>
        public long ParentId { get; set; } // 转发微博

        [LongText]
        public String Content { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public int Replies { get; set; }
        public int Reposts { get; set; } // 转发数量
        public int Likes { get; set; }

        public DateTime Created { get; set; }

        public int SaveStatus { get; set; }

        //-----------------------------------------------------------

        /// <summary>
        /// 根据 DataType 和 DataId 加载 OpenComment；
        /// 以及根据它和 target 同步 likes 等信息
        /// </summary>
        public String DataType { get; set; }

        /// <summary>
        /// 根据 DataType 和 DataId 加载 OpenComment；
        /// 以及根据它和 target 同步 likes 等信息
        /// </summary>
        public long DataId { get; set; }

        //-------------------------------------------------------------------

        /// <summary>
        /// 视频：播放页面
        /// </summary>
        public String PageUrl { get; set; }

        /// <summary>
        /// 视频：flash网址
        /// </summary>
        public String FlashUrl { get; set; }

        /// <summary>
        /// 视频：缩略图
        /// </summary>
        public String PicUrl { get; set; }


        //-------------------------------------------------------------------

        /// <summary>
        /// 上传的图片
        /// </summary>
        public String Pic { get; set; }


        [NotSave]
        public String PicMedium {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarOriginal( this.Pic );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, "m" );
                }
            }
        }

        [NotSave]
        public String PicBig {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarThumb( this.Pic, ThumbnailType.Big );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, "b" );
                }
            }
        }

        [NotSave]
        public String PicOriginal {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarOriginal( this.Pic );
                }
                else {
                    return sys.Path.GetPhotoOriginal( this.Pic );
                }
            }
        }

        [NotSave]
        public String PicSmall {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarThumb( this.Pic, ThumbnailType.Medium );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, "s" );
                }
            }
        }

        [NotSave]
        public String PicSx {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarThumb( this.Pic, ThumbnailType.Medium );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, "sx" );
                }
            }
        }

        private Boolean isUserAvatar() {
            if (this.Pic == null) return false;
            return this.Pic.IndexOf( "face/" ) > 0;
        }

        [NotSave]
        public Boolean IsPic {
            get { return strUtil.HasText( this.Pic ); }
        }


        //-------------------------------------------------------------------------

        #region IAppData 成员

        [NotSave]
        public long AppId { get { return 0; } set { } }

        [NotSave]
        public User Creator { get { return this.User; } set { this.User = value; } }

        [NotSave]
        public string CreatorUrl { get { return this.User.Url; } set { } }

        [NotSave]
        public long OwnerId { get { return this.User.Id; } set { } }

        [NotSave]
        public string OwnerType { get { return typeof( User ).FullName; } set { } }

        [NotSave]
        public string OwnerUrl { get { return this.User.Url; } set { } }

        [NotSave]
        public string Title { get { return "微博: " + strUtil.ParseHtml( this.Content, 50 ); } set { } }

        [NotSave]
        public int AccessStatus { get { return 0; } set { } }

        #endregion


        public Type GetAppType() {
            return null;
        }
    }

}
