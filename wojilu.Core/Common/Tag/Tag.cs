/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;


namespace wojilu.Common.Tags {

    [Serializable]
    public class Tag : ObjectBase<Tag> {

        public int CreatorId { get; set; }

        [Column( Length = 50 )]
        public String Name { get; set; }

        public int DataCount { get; set; }
        public int UserCount { get; set; }

        public DateTime CreateTime { get; set; }


    }
}

