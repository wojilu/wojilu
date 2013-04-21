/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Polls.Domain;
using wojilu.Common.Comments;

namespace wojilu.Apps.Poll.Domain {

    [Serializable]
    public class PollData : PollBase, ICommentTarget {

        public int Replies { get; set; }

        public Type GetAppType() {
            return typeof( PollApp );
        }

    }

}
