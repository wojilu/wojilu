using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Members.Users.Domain {

    public class UserTag : ObjectBase<UserTag> {


        public int CreatorId { get; set; }

        public String Name { get; set; }
        public DateTime Created { get; set; }

        public int UserCount { get; set; }

    }

    public class UserTagShip : ObjectBase<UserTagShip> {

        public User User { get; set; }
        public UserTag Tag { get; set; }
        public DateTime Created { get; set; }

    }


}
