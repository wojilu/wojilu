using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentCustomTemplate : ObjectBase<ContentCustomTemplate> {

        public String Name { get; set; }
        public String Description { get; set; }

        public User Creator { get; set; }
        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        [LongText]
        public String Content { get; set; }
        public DateTime Created { get; set; }


    }

}
