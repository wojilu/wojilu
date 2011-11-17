/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Photo {

    public class PhotoCommentController : Common.CommentController<PhotoPostComment> {

        internal static String GetCommentMoreLink( int commentCount, MvcContext ctx ) {

            if (commentCount == 0) return "";

            String link;
            if (ctx.viewer.IsLogin && (ctx.viewer.Id == ctx.owner.Id))
                link = ctx.GetLink().To( new PhotoCommentController().AdminList );
            else
                link = ctx.GetLink().To( new PhotoCommentController().List );

            return string.Format( "<a href='{0}'>" + wojilu.lang.get( "more" ) + "…</a>", link );

        }

    }

}
