/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Common.Comments;
using wojilu.Web.Controller.Common;
using wojilu.Apps.Photo.Interface;
using wojilu.ORM;
using wojilu.Data;

namespace wojilu.Web.Controller.Photo {

    [App( typeof( PhotoApp ) )]
    public class LayoutController : ControllerBase {

        public IPhotoAlbumService albumService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IOpenCommentService commentService { get; set; }

        public LayoutController() {

            albumService = new PhotoAlbumService();
            postService = new PhotoPostService();
            commentService = new OpenCommentService();
        }

        public override void Layout() {

            set( "adminUrl", to( new Admin.MyController().My ) );

            set( "photo.AppUrl", to( new PhotoController().Index ) );

            bindNewPhoto();

            bindComments( "comment" );

            bindAdminLink();
        }

        private void bindNewPhoto() {

            IBlock block = getBlock( "list" );

            List<PhotoPost> list = postService.GetNew( ctx.owner.Id, 10 );
            foreach (PhotoPost x in list) {

                block.Set( "x.Link", alink.ToAppData( x ) );
                block.Set( "x.Title", x.Title );
                block.Set( "x.Description", x.Description );

                block.Set( "x.Pic", x.ImgThumbUrl );
                block.Set( "x.PicM", x.ImgMediumUrl );
                block.Set( "x.PicO", x.ImgUrl );
                block.Set( "x.PicS", x.ImgSmallUrl );


                block.Next();
            }
        }


        private void bindAdminLink() {
            set( "friendsPhotoLink", to( new Admin.PhotoController().Friends, -1 ) );
            set( "myLink", to( new Admin.MyController().My ) );
            set( "categoryAdmin", to( new Admin.AlbumController().List ) );
            set( "categoryAdd", to( new Admin.AlbumController().Add ) );
            set( "uploadLink", to( new Admin.PostController().Add ) );
        }

        private void bindComments( String blockName ) {

            IBlock block = getBlock( blockName );

            List<OpenComment> comments = commentService.GetByApp( typeof( PhotoPost ), ctx.app.Id, 7 );

            foreach (OpenComment x in comments) {
                block.Set( "comment.Title", strUtil.SubString( x.Content, 14 ) );
                block.Set( "comment.Url", to( new PostController().Show, x.TargetDataId ) + "#comments" );
                block.Next();
            }

            String lnkMore = PhotoCommentController.GetCommentMoreLink( comments.Count, ctx );
            set( "commentMoreLink", lnkMore );

        }


    }
}
