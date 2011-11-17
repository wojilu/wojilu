/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    [Table( "FeedSubscription" )]
    public class Subscription : ObjectBase<Subscription>, ISort {


        public User User { get; set; }
        public int OrderId { get; set; }
        public int AppId { get; set; }

        [CacheCount( "UserCount" )]
        public FeedSource FeedSource { get; set; }
        public FeedCategory Category { get; set; }

        public String Name { get; set; }


        public void updateOrderId() {
            base.update( "OrderId" );
        }

    }
}
