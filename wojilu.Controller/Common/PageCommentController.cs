/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Pages.Domain;

namespace wojilu.Web.Controller.Common {
    public class PageCommentController : CommentController<PageComment> {

        protected override string getTargetLink( PageComment post ) {

            return t2( new PageController().Show, post.RootId );
        }

    }
}
