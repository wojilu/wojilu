/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Data;

namespace wojilu.Members.Users.Service {

    [NotSave]
    public class VisitItem : CacheObject {

        public VisitItem() {
            this.IsUpdated = false;
        }

        public long VisitorId { get; set; }
        public long TargetId { get; set; }
        public DateTime VisitTime { get; set; }
        public Boolean IsUpdated { get; set; }
    }

}
