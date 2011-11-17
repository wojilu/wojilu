/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Poll.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Poll {

    public class PollCommentController : CommentController<PollComment> {

        protected override String getTargetLink( PollComment post ) {
            return to( new PollController().Show, post.RootId );
        }

    }

}
