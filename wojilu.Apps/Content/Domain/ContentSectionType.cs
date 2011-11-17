/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Data;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentSectionType : CacheObject {

        public int OrderId { get; set; }

        // 缩略图
        public String ThumbUrl { get; set; }         

        // 对应的绑定器（其实就是一个controller）
        public String TypeFullName { get; set; }  

        public String Description { get; set; }

    }
}

