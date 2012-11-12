/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Common.AppBase.Interface;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Forum.Domain {


    [Serializable]
    public class ForumLink : ObjectBase<ForumLink>, ISort {

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }

        public int AppId { get; set; }

        public int OrderId { get; set; }

        [NotNull( Lang = "exName" )]
        public String Name { get; set; }

        public String Description { get; set; }

        [NotNull( Lang = "exUrl" )]
        public String Url { get; set; }

        public String Logo { get; set; }

        public DateTime Created { get; set; }


        public void updateOrderId() {
            db.update( this, "OrderId" );
        }

    }
}

