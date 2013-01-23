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
    public class BlogCommentController : Open.CommentListController<BlogPost> {

        internal static String GetMoreLink( int commentCount, MvcContext ctx ) {

            if (commentCount == 0) return "";

            String link;
            if (ctx.viewer.IsLogin && ctx.viewer.IsOwnerAdministrator( ctx.owner.obj ))
                link = ctx.link.To( new BlogCommentAdminController().List );
            else
                link = ctx.link.To( new BlogCommentController().Index );

            return string.Format( "<a href='{0}'>" + wojilu.lang.get( "more" ) + "…</a>", link );

        }

    }


    [App( typeof( BlogApp ) )]
    public class BlogCommentAdminController : Open.Admin.CommentBaseController<BlogPost> {

        public override void CheckPermission() {

            if (ctx.viewer.IsAdministrator()) return;

            if (!ctx.viewer.IsLogin && ctx.viewer.IsOwnerAdministrator( ctx.owner.obj )==false) {


                echoError( "没有权限" );
            }


        }

    }

}
