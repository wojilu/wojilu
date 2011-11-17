/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;
using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class ReaderApp : ObjectBase<ReaderApp>, IApp, IAccessStatus, IHits {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        public int Hits { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

    }
}
