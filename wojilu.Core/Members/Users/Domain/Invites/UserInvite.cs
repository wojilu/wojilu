/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class UserInvite : ObjectBase<UserInvite> {

        public User Inviter { get; set; } // 邀请人
        public String ReceiverMail { get; set; } // 收信人的email

        [LongText]
        public String MailBody { get; set; }
        public DateTime Created { get; set; }

        public int SendStatus { get; set; }

    }

}
