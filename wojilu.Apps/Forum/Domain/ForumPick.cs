/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Common.Picks;

namespace wojilu.Apps.Forum.Domain {

    public class ForumPick : DataPickBase {

        public override Type GetImgType() {
            return typeof( ForumPickedImg );
        }

    }

}
