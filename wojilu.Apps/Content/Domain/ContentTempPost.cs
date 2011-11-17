using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentTempPost : ObjectBase<ContentTempPost> {


        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public User Creator { get; set; }
        public int AppId { get; set; }
        public int SectionId { get; set; }


        public String Title { get; set; }
        public String TypeName { get; set; }

        public String Author { get; set; }
        public String SourceLink { get; set; }

        [LongText]
        public String Content { get; set; }
        [LongText]
        public String Summary { get; set; }

        public String ImgLink { get; set; }

        public String TagList { get; set; }

        public String Ip { get; set; }
        public DateTime Created { get; set; }

        public int Status { get; set; }


    }

}
