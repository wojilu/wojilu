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

        public LayoutController() {

            albumService = new PhotoAlbumService();

            commentService = new CommentService<PhotoPostComment>();
        }

        public override void Layout() {

            set( "adminUrl", to( new Admin.MyController().My ) );

            set( "photo.AppUrl", Link.To( new PhotoController().Index ) );
            IBlock block = getBlock( "list" );
            List<PhotoAlbum> albumList = albumService.GetListByApp( ctx.app.Id );
            foreach (PhotoAlbum album in albumList) {
                block.Set( "category.Title", album.Name );
                block.Set( "category.Url", Link.To( new PhotoController().Album, album.Id ) );
                String coverImg = PhotoHelper.getCover( album );
                block.Set( "category.Cover", coverImg );
                block.Next();
            }


            bindComments( "comment" );
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
                commentblock.Set( "comment.Url", Link.To( new PostController().Show, comment.RootId ) + "#comments" );
                commentblock.Next();
            }

            String commentMoreLink = PhotoCommentController.GetCommentMoreLink( newComments.Count, ctx );
            set( "commentMoreLink", commentMoreLink );

        }


    }
}
