/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Tags {


    [Serializable]
    public class DataTagShip : ObjectBase<DataTagShip> {

        public int DataId { get; set; }
        public Tag Tag { get; set; }
        public String TypeFullName { get; set; }
        public DateTime Created { get; set; }

    }
}

