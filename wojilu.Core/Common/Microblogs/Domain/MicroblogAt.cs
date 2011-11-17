using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Microblogs.Domain {

    public class MicroblogAt : ObjectBase<MicroblogAt> {

        public Microblog Microblog { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }

    }

}
