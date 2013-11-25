using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Content.Domain {

    public class ContentDigg : ObjectBase<ContentDigg> {

        public long UserId { get; set; }
        public long PostId { get; set; }

        public DateTime Created { get; set; }
        public String Ip { get; set; }

        public int TypeId { get; set; } // 0=up, 1=down


    }

}
