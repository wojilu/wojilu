/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.Drawing;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Web.Utils;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentAttachment : ObjectBase<ContentAttachment>, ISort {

        public int AppId { get; set; }
        public int PostId { get; set; }
        
        [Column( Name="FileGuid", Length=40 )]
        public String Guid { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        // 带路径的文件名
        public String Name { get; set; }

        // 自定义的文件名
        public String Description { get; set; } 
        public String Type { get; set; }
        public int FileSize { get; set; }

        public int Downloads { get; set; }

        // TODO:使用外部网址(盗链)
        public String Url { get; set; } 

        public DateTime Created { get; set; }

        // 不带路径的文件名
        [NotSave]
        public String FileName {
            get { return Path.GetFileName( this.Name ); } 
        }

        // 显示名称
        public String GetFileShowName() {
            return strUtil.HasText( this.Description ) ? this.Description : this.FileName;
        }

        [NotSave]
        public int FileSizeKB {
            get {
                int size = this.FileSize / 1024;
                if (size == 0) size = 1;
                return size;
            }
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
                return Uploader.IsImage( this.Type );
            }
        }

        public int OrderId { get; set; }

        public void updateOrderId() {
            this.update( "OrderId" );
        }

    }
}

