/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Photo.Vo;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Web.Controller.Photo {

    [App( typeof( PhotoApp ) )]
    public class PostController : ControllerBase {

        public IPhotoPostService postService { get; set; }

        public PostController() {
            postService = new PhotoPostService();
        }

        public void Show( int id ) {

            PhotoPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            postService.AddtHits( post );

            ctx.Page.Title = post.Title;
            ctx.Page.Keywords = post.Tag.TextString;


            Post postshow = Post.Fill( post, ctx, postService );
            bind( postshow );

            DataPage<PhotoPost> list = postService.GetSingle( ctx.owner.Id, id );
            bind( "stats", list );

            set( "photoStats", string.Format( alang( "photoStats" ), list.Current, list.RecordCount ) );

            bindComment( post );

            ctx.SetItem( "visitor", new PhotoPostVisitor() );
            ctx.SetItem( "visitTarget", post );
            load( "visitorList", new VisitorController().List );
        }

        private void bindComment( PhotoPost post ) {
            String commentUrl = t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + PhotoLink.ToPost( post.Id )
                + "&dataType=" + typeof( PhotoPost ).FullName
                + "&dataTitle=" + post.Title
                + "&dataUserId=" + post.Creator.Id
                + "&dataId=" + post.Id
                + "&appId=" + post.AppId;
            set( "commentUrl", commentUrl );
        }


    }
}

