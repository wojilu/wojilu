/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.Common.Polls.Domain {

    [Serializable]
    public abstract class PollResultBase : ObjectBase<PollResultBase> {

        public int PollId { get; set; }

        [Column( Name = "MemberId" )]
        public User User { get; set; }

        public String Answer { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public DateTime Created { get; set; }


    }
}

