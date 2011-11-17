/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

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

            WebUtils.pageTitle( this, post.Title );
            Page.Keywords = post.Tag.TextString;


            Post postshow = Post.Fill( post, ctx, postService );
            bind( postshow );

            DataPage<PhotoPost> list = postService.GetSingle( ctx.owner.Id, id );
            bind( "stats", list );

            set( "photoStats", string.Format( alang( "photoStats" ), list.Current, list.RecordCount ) );

            ctx.SetItem( "createAction", to( new PhotoCommentController().Create, id ) );
            ctx.SetItem( "commentTarget", post );
            load( "commentSection", new PhotoCommentController().ListAndForm );

            ctx.SetItem( "visitor", new PhotoPostVisitor() );
            ctx.SetItem( "visitTarget", post );
            load( "visitorList", new VisitorController().List );
        }


    }
}

