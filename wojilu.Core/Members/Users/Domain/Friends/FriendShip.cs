/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;

using wojilu.ORM;

namespace wojilu.Members.Users.Domain {


    [Serializable]
    public class FriendShip : ObjectBase<FriendShip> {

        [Column( Name = "MemberId" )]
        public User User { get; set; }
        public int CategoryId { get; set; }
        public String Description { get; set; }

        public User Friend { get; set; }
        public int CategoryIdFriend { get; set; }
        public String DescriptionFriend { get; set; }

        public String Msg { get; set; }

        public int Status { get; set; }
        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

    }
}

