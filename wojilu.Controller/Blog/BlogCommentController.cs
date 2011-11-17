/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public class BlogCommentController : Common.CommentController<BlogPostComment> {

        internal static String GetCommentMoreLink( int commentCount, MvcContext ctx ) {

            if (commentCount == 0) return "";

            String link;
            if (ctx.viewer.IsLogin && (ctx.viewer.Id == ctx.owner.Id))
                link = ctx.GetLink().To( new BlogCommentController().AdminList );
            else
                link = ctx.GetLink().To( new BlogCommentController().List );

            return string.Format( "<a href='{0}'>" + wojilu.lang.get( "more" ) + "…</a>", link );

        }

    }

}
