/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Forum.Domain {


    [Serializable]
    public class ForumTag : ObjectBase<ForumTag>, IAppTag {

        public int AppId { get; set; }
        public Tag Tag { get; set; }
        public int PostCount { get; set; }
        public DateTime Created { get; set; }

    }
}

