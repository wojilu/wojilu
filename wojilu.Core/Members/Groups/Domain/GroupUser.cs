/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Groups.Domain {

    [Serializable]
    public class GroupUser : ObjectBase<GroupUser> {

        public Group Group { get; set; }

        public User Member { get; set; }

        public int PostCount { get; set; }
        public int TopicCount { get; set; }
        public int Status { get; set; }

        public String Msg { get; set; }

        [TinyInt]
        public int IsFounder { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        [NotSave]
        public String RoleString {
            get {
                GroupRole role = GroupRole.GetById( this.Status );
                return role.Name; 
            }
        }


    }
}

