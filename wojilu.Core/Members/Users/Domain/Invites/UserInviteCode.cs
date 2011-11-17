/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Service;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class UserInviteCode : ObjectBase<UserInviteCode> {

        public User User { get; set; }
        public String Code { get; set; }
        public DateTime Created { get; set; }

    }

}
