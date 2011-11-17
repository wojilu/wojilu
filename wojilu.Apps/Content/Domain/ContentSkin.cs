using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentSkin : ObjectBase<ContentSkin> {

        public String Name { get; set; }
        public String Description { get; set; }
        public String ThumbUrl { get; set; }
        public String StylePath { get; set; }

        [LongText]
        public String Body { get; set; }


    }

}
