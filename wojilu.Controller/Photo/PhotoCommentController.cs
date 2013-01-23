/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Photo {

    [App( typeof( PhotoApp ) )]
    public class PhotoCommentController : Open.CommentListController<PhotoPost> {

        internal static String GetCommentMoreLink( int commentCount, MvcContext ctx ) {

            if (commentCount == 0) return "";

            String link;
            if (ctx.viewer.IsLogin && (ctx.viewer.Id == ctx.owner.Id))
                link = ctx.link.To( new PhotoCommentAdminController().List );
            else
                link = ctx.link.To( new PhotoCommentController().Index );

            return string.Format( "<a href='{0}'>" + wojilu.lang.get( "more" ) + "…</a>", link );

        }

    }

    [App( typeof( PhotoApp ) )]
    public class PhotoCommentAdminController : Open.Admin.CommentBaseController<PhotoPost> {

        public override void CheckPermission() {

            if (ctx.viewer.IsAdministrator()) return;

            if (!ctx.viewer.IsLogin && ctx.viewer.IsOwnerAdministrator( ctx.owner.obj ) == false) {


                echoError( "没有权限" );
            }


        }

    }

}
