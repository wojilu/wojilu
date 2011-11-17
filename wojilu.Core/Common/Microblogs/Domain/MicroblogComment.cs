/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Comments;

namespace wojilu.Common.Microblogs.Domain {

    [Serializable]
    public class MicroblogComment : ObjectBase<MicroblogComment>, INode {

        public static readonly int ContentMax = 1000;

        public User User { get; set; }

        [CacheCount( "Replies" )]
        public Microblog Root { get; set; }

        public int ParentId { get; set; }

        [LongText]
        public String Content { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }


        public DateTime Created { get; set; }

        [NotSave]
        public String Name {
            get { return this.Content; }
            set { this.Content = value; }
        }

    }

}
