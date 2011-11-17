/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class Follower : ObjectBase<Follower> {

        public User User { get; set; }
        public User Target { get; set; }

        public int CategoryId { get; set; }

        public DateTime Created { get; set; }

    }

}
