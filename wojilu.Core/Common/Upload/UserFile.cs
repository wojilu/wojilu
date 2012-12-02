using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Drawing;
using wojilu.Members.Users.Domain;
using wojilu.ORM;
using System.IO;

namespace wojilu.Common.Upload {

    public class UserFile : ObjectBase<UserFile> {

        public int DataId { get; set; }
        public String DataType { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String PathRelative { get; set; }
        public int FileSize { get; set; }

        public String ContentType { get; set; }
        public String Ext { get; set; }

        public int IsPic { get; set; }
        
        public String Ip { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public int FileSizeKB {
            get {
                int size = this.FileSize / 1024;
                if (size == 0) size = 1;
                return size;
            }
        }

        // 不带路径的文件名
        [NotSave]
        public String FileName {
            get { return Path.GetFileName( this.Name ); }
        }

        /// <summary>
        /// 完整的相对路径
        /// </summary>
        [NotSave]
        public String PathFull {
            get { return Path.Combine( sys.Path.Photo, this.PathRelative ); }
        }

        [NotSave]
        public String PicS {
            get { return sys.Path.GetPhotoThumb( this.PathRelative ); }
        }

        [NotSave]
        public String PicM {
            get { return sys.Path.GetPhotoThumb( this.PathRelative, wojilu.Drawing.ThumbnailType.Medium ); }
        }

        [NotSave]
        public String PicO {
            get { return sys.Path.GetPhotoOriginal( this.PathRelative ); }
        }




    }

}
