using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using System.IO;

namespace wojilu.Common.Msg.Domain {

    public class MessageAttachment : ObjectBase<MessageAttachment> {

        public MessageData MessageData { get; set; }

        public String Name { get; set; }
        public String Url { get; set; }
        public DateTime Created { get; set; }

        public String Type { get; set; }
        public int FileSize { get; set; }


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
        public String FileUrl {
            get { return Path.Combine( sys.Path.Photo, this.Url ); }
        }

    }

}
