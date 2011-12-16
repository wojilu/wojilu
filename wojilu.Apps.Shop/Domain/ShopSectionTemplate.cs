/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Data;

namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopSectionTemplate : CacheObject {

        public int OrderId { get; set; }

        public String TemplateName { get; set; }
        public String Description { get; set; }

        public String ThumbUrl { get; set; }

    }
}

