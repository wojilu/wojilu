/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class Blacklist : ObjectBase<Blacklist> {

        public User User { get; set; }
        public User Target { get; set; }

        public DateTime Created { get; set; }


    }

}
