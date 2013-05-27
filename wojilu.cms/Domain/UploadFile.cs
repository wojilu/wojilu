using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.cms.Domain {
    
    public enum FileType {
        Pic, File
    }

    public class UploadFile : ObjectBase<UploadFile> {

        public string Name { get; set; }
        public int FileType { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public string FullPath {
            get { return sys.Path.GetPhotoOriginal( this.Path ); }
        }

        [NotSave]
        public string FullThumbPath {
            get { return sys.Path.GetPhotoThumb( this.Path ); }
        }

        [NotSave]
        public string PicO {
            get { return sys.Path.GetPhotoOriginal( this.Path ); }
        }

        [NotSave]
        public String PicS {
            get { return sys.Path.GetPhotoThumb( this.Path ); }
        }

        [NotSave]
        public String PicM {
            get { return sys.Path.GetPhotoThumb( this.Path, wojilu.Drawing.ThumbnailType.Medium ); }
        }

        [NotSave]
        public String Ext {
            get { return System.IO.Path.GetExtension( this.Path ); }
        }

    }

}
