/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Apps.Blog.Domain {


    [Serializable]
    public class Blogroll : ObjectBase<Blogroll> {

        public int pkid { get; set; }

        public int AppId { get; set; }
        public int OwnerId { get; set; }
        public int OrderId { get; set; }

        public String Name { get; set; }
        [LongText]
        public String Description { get; set; }
        public String Link { get; set; }

        public DateTime Created { get; set; }

    }
}

