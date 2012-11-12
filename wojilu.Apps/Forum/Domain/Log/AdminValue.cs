/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Domain {

    public class AdminValue {

        public String Ids { get; set; }
        public int ActionId { get; set; }

        public int AppId { get; set; }
        public User User { get; set; }

        public String Reason { get; set; }
        public String Ip { get; set; }
        public Boolean IsSendMsg { get; set; }

        public String Condition {
            get { return "Id in (" + Ids + ")"; }
        }
    }

}
