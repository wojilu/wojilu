using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentPostSection : ObjectBase<ContentPostSection> {

        [Column( Name = "PostId" )]
        public ContentPost Post { get; set; }

        [Column( Name = "SectionId" )]
        public ContentSection Section { get; set; }

        public int SaveStatus { get; set; }

    }

}
