/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Apps.Blog.Domain {

    [Serializable]
    public class BlogPostPicked : ObjectBase<BlogPostPicked> {

        public BlogPost Post { get; set; }
        public int Status { get; set; }
        public String TitleStyle { get; set; }
        public String Abstract { get; set; }

    }


}
