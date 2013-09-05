using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Microblogs.Domain {

    public class MicroblogLike : ObjectBase<MicroblogLike> {

        public User User { get; set; }

        public Microblog Microblog { get; set; }

        public DateTime Created { get; set; }

        public String Ip { get; set; }
    }
}
