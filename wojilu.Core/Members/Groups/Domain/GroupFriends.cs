/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Groups.Domain {


    [Serializable]
    public class GroupFriends : ObjectBase<GroupFriends> {

        public Group Group { get; set; }

        [CacheCount( "FriendCount" )]
        public Group Friend { get; set; }

        public int Type { get; set; }
        public DateTime CreateTime { get; set; }


    }
}

