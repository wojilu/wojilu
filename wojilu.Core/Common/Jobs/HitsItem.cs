/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Web.Jobs;

namespace wojilu.Common.Jobs {

    [Serializable]
    [NotSave]
    public class HitsItem : CacheObject {

        public HitsItem() {
            this.IsUpdated = false;
        }

        public IHits Target { get; set; }

        public Boolean IsUpdated { get; set; }

        public DateTime Updated { get; set; }


    }

}
