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
        public ICommentService<PhotoPostComment> commentService { get; set; }
        public IPhotoPostService postService { get; set; }

        public LayoutController() {

            albumService = new PhotoAlbumService();
            postService = new PhotoPostService();
            commentService = new CommentService<PhotoPostComment>();
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
                    

                block.Next();
            }
        }


        private void bindAdminLink() {
            set( "friendsPhotoLink", to( new Admin.PhotoController().Index, -1 ) );
            set( "myLink", to( new Admin.MyController().My ) );
            set( "categoryAdmin", to( new Admin.AlbumController().List ) );
            set( "categoryAdd", to( new Admin.AlbumController().Add ) );
            set( "uploadLink", to( new Admin.PostController().Add ) );
        }

        private void bindComments( String blockName ) {

            IBlock commentblock = getBlock( blockName );
            //List<PhotoPostComment> newComments = commentService.GetNew( ctx.owner.Id, ctx.app.Id, 7 );
            List<PhotoPostComment> newComments = PhotoPostComment.find( "AppId=" + ctx.app.Id ).list( 7 );
            if (ctx.app.Id == 0) {
                String tblc = Entity.GetInfo( typeof( PhotoPostComment ) ).TableName;
                String tblp = Entity.GetInfo( typeof( PhotoPost ) ).TableName;
                String sql = "select a.* from " + tblc + " a, " + tblp + " b where a.AppId=" + ctx.app.Id + " and a.RootId=b.Id and b.OwnerId=" + ctx.owner.Id;

                EntityInfo ei = Entity.GetInfo( typeof( PhotoPostComment ) );
                sql = ei.Dialect.GetLimit( sql, 7 );
                
                newComments = db.findBySql<PhotoPostComment>( sql );
            }

            foreach (PhotoPostComment comment in newComments) {
                commentblock.Set( "comment.Title", strUtil.SubString( comment.Content, 14 ) );
                commentblock.Set( "comment.Url", to( new PostController().Show, comment.RootId ) + "#comments" );
                commentblock.Next();
            }

            String commentMoreLink = PhotoCommentController.GetCommentMoreLink( newComments.Count, ctx );
            set( "commentMoreLink", commentMoreLink );

        }


    }
}
