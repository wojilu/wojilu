/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Common.Tags;

namespace wojilu.Common.AppBase.Interface {

    public interface IAppTag {

        int AppId { get; set; }
        Tag Tag { get; set; }

        int PostCount { get; set; }

        DateTime Created { get; set; }
    }

}

