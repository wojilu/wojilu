/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.ORM;
using wojilu.Drawing;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentAttachmentTemp : ObjectBase<ContentAttachmentTemp> {

        public int AppId { get; set; }

        [Column( Name = "FileGuid", Length = 40 )]
        public String Guid { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public int FileSize { get; set; }

        public String Url { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public String FileName {
            get { return Path.GetFileName( this.Name ); }
        }

        [NotSave]
        public int FileSizeKB {
            get { return (this.FileSize / 1024); }
        }

        [NotSave]
        public String FileThumbUrl {
            get { return Img.GetThumbPath( this.FileUrl ); }
        }

        [NotSave]
        public String FileMediuUrl {
            get { return Img.GetThumbPath( this.FileUrl, ThumbnailType.Medium ); }
        }

        /// <summary>
        /// 完整的相对路径
        /// </summary>
        [NotSave]
        public String FileUrl {
            get { return Path.Combine( sys.Path.Photo, this.Name ); }
        }

        [NotSave]
        public Boolean IsImage {
            get {
                if (strUtil.IsNullOrEmpty( this.Type )) return false;
                if (this.Type.IndexOf( "image" ) < 0) return false;
                return true;
            }
        }

    }
}
