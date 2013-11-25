/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Forum.Domain {


    [Serializable]
    public class ForumCategory : ObjectBase<ForumCategory>, ISort {

        public ForumCategory() { }

        public ForumCategory(long id) {
            base.Id = id;
        }

        public ForumCategory(long id, string name) {
            base.Id = id;
            this.Name = name;
        }

        public long AppId { get; set; }
        public long BoardId { get; set; }
        public int OrderId { get; set; }

        public long OwnerId { get; set; }
        public String OwnerType { get; set; }

        public User Creator { get; set; }

        [NotNull( Lang = "exName" )]
        public String Name { get; set; }

        public String NameColor { get; set; }

        public int TopicCount { get; set; }
        public DateTime Created { get; set; }



        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

