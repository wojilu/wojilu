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
using wojilu.Web.Utils;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class AttachmentJson {

        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public int FileSize { get; set; }

        public String FileName {
            get { return Path.GetFileName( this.Name ); }
        }

        public int FileSizeKB {
            get { return (this.FileSize / 1024); }
        }

        public String FileThumbUrl {
            get { return Img.GetThumbPath( this.FileUrl ); }
        }

        public String FileMediuUrl {
            get { return Img.GetThumbPath( this.FileUrl, ThumbnailType.Medium ); }
        }

        public String FileUrl {
            get { return Path.Combine( sys.Path.Photo, this.Name ); }
        }

        public Boolean IsImage {
            get {
                return Uploader.IsImage( this.Type, this.FileName );
            }
        }

    }

    [Serializable]
    public class AttachmentTemp : ObjectBase<AttachmentTemp> {

        public AttachmentJson GetJsonObject() {
            AttachmentJson obj = new AttachmentJson();
            obj.Id = this.Id;
            obj.Name = this.Name;
            obj.Description = this.Description;
            obj.Type = this.Type;
            obj.FileSize = this.FileSize;
            return obj;
        }


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

        public int Price { get; set; }
        public int ReadPermission { get; set; }

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
                return Uploader.IsImage( this.Type, this.FileName );
            }
        }

    }
}
