/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class UserConfirm : ObjectBase<UserConfirm> {

        public User User { get; set; }
        public String Code { get; set; }

        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

    }

}
