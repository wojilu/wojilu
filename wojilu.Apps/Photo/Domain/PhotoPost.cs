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

namespace wojilu.Apps.Photo.Domain {

    [Serializable]
    public class PhotoPost : ObjectBase<PhotoPost>, IAppData, IShareData, IHits {

        public int AppId { get; set; }
        public int SysCategoryId { get; set; }

        private PhotoAlbum _album = null;

        [Column( Name = "CategoryId" )]
        public PhotoAlbum PhotoAlbum { get; set; }

        public int OwnerId { get; set; }
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


        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        [NotSave]
        public String ImgUrl {
            get { return sys.Path.GetPhotoOriginal( this.DataUrl ); }
        }

        [NotSave]
        public String ImgMediumUrl {
            get {
                return sys.Path.GetPhotoThumb( this.DataUrl, wojilu.Drawing.ThumbnailType.Medium );
            }
        }

        [NotSave]
        public String ImgThumbUrl {
            get { return sys.Path.GetPhotoThumb( this.DataUrl ); }
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

    }
}

