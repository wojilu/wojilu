using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Blog.Domain {

    public class BlogPicPick : ObjectBase<BlogPicPick> {

        public int FileId { get; set; }

        public BlogPost BlogPost { get; set; }

        public String Title { get; set; }

        [NotNull]
        public String Pic { get; set; }

        //---------------------------------------------------------------------

        [NotSave]
        public String PicS {
            get { return sys.Path.GetPhotoThumb( this.Pic ); }
        }

        [NotSave]
        public String PicM {
            get { return sys.Path.GetPhotoThumb( this.Pic, wojilu.Drawing.ThumbnailType.Medium ); }
        }

        [NotSave]
        public String PicO {
            get { return sys.Path.GetPhotoOriginal( this.Pic ); }
        }

        public DateTime Created { get; set; }

    }

}
