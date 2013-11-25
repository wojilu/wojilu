/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Serialization;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;
using wojilu.Common.Comments;
using wojilu.Apps.Photo.Helper;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoPost : ObjectBase<PhotoPost>, IAppData, IShareData, IHits, ICommentTarget, ILike {

        public long AppId { get; set; }
        public long SysCategoryId { get; set; }

        [Column( Name = "CategoryId" )]
        public PhotoAlbum PhotoAlbum { get; set; }

        public long OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column( Length = 50 )]
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        [Column( Length = 20 )]
        public String CreatorUrl { get; set; }

        [Column( Length = 50 )]
        [NotNull( Lang = "exTitle" )]
        public String Title { get; set; }

        [NotNull( Lang = "exPicUrl" )]
        public String DataUrl { get; set; }

        [LongText]
        public String Description { get; set; }
        public int Hits { get; set; }
        public int Replies { get; set; }

        /// <summary>
        /// 图片大小等信息，存储格式：s=68/68,sx=80/120,m=180/500
        /// </summary>
        public String SizeInfo { get; set; }

        //--------------------------------------------------------------------

        public int Likes { get; set; }
        public int Pins { get; set; }

        public String SrcName { get; set; } // 来源网站
        public String SrcUrl { get; set; } // 来源网址
        public String SrcTool { get; set; } // 采集工具

        public long ParentId { get; set; } // 转采来源 parent
        public long RootId { get; set; } // 转采来源 root

        //--------------------------------------------------------------------

        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        //--------------------------------------------------------------------

        private Dictionary<String, PhotoInfo> _sizeInfo;

        [NotSave]
        public PhotoInfo SizeS {
            get { return getSizeDic( "s" ); }
        }

        [NotSave]
        public PhotoInfo SizeSX {
            get { return getSizeDic( "sx" ); }
        }

        [NotSave]
        public PhotoInfo SizeM {
            get { return getSizeDic( "m" ); }
        }

        [NotSave]
        public PhotoInfo SizeB {
            get { return getSizeDic( "b" ); }
        }

        private PhotoInfo getSizeDic( string key ) {

            if (_sizeInfo == null) {
                _sizeInfo = ObjectContext.Create<PhotoInfoHelper>().GetInfo( this.SizeInfo );
            }

            PhotoInfo ret;
            _sizeInfo.TryGetValue( key, out ret );
            return ret;
        }

        //--------------------------------------------------------------------

        [NotSave]
        public String ImgUrl {
            get { return sys.Path.GetPhotoOriginal( this.DataUrl ); }
        }

        [NotSave]
        public String ImgMediumUrl {
            get {
                return sys.Path.GetPhotoThumb( this.DataUrl, "m" );
            }
        }

        [NotSave]
        public String ImgThumbUrl {
            get { return sys.Path.GetPhotoThumb( this.DataUrl, "sx" ); }
        }

        [NotSave]
        public String ImgSmallUrl {
            get { return sys.Path.GetPhotoThumb( this.DataUrl, "s" ); }
        }

        private TagTool _tag;
        [NotSave]
        public TagTool Tag {
            get {
                if (this._tag == null) this._tag = new TagTool( this );
                return this._tag;
            }
        }

        [NotSave]
        public String SysCategoryName {
            get {
                if (this.SysCategoryId <= 0) return "";
                PhotoSysCategory c = db.findById<PhotoSysCategory>( this.SysCategoryId );
                return c == null ? "" : c.Name;
            }
        }

        public IShareInfo GetShareInfo() {
            return new PhotoPostFeed( this );

        }

        public Type GetAppType() {
            return typeof( PhotoApp );
        }

    }
}

