/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Tags {


    [Serializable]
    public class MemberTagShip : ObjectBase<MemberTagShip> {


        public User Member { get; set; }

        [CacheCount( "UserCount" )]
        public Tag Tag { get; set; }

        public DateTime CreateTime { get; set; }

    }
}

