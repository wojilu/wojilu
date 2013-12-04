/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Interface;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Photo.Admin {


    [App( typeof( PhotoApp ) )]
    public class MyController : ControllerBase {

        public virtual IFriendService friendService { get; set; }

        public virtual IPhotoPostService postService { get; set; }
        public virtual IPhotoAlbumService albumService { get; set; }
        public virtual IPhotoSysCategoryService categoryService { get; set; }

        public MyController() {

            base.HideLayout( typeof( Photo.LayoutController ) );

            friendService = new FriendService();

            postService = new PhotoPostService();
            albumService = new PhotoAlbumService();

            categoryService = new PhotoSysCategoryService();
        }

        public override void Layout() {

            IList categories = albumService.GetListByApp( ctx.app.Id );
            bindList( "categories", "c", categories, bindLink );

            set( "lnkDefaultAlbum", to( Category, 0 ) );

        }



        public virtual void My() {


            DataPage<PhotoPost> posts = postService.GetPostPage( ctx.owner.Id, ctx.app.Id, 20 );
            bindPhotoList( posts, -1 );

        }



        public virtual void Category( long categoryId ) {

            view( "My" );

            DataPage<PhotoPost> posts = postService.GetPostPageByAlbum( ctx.owner.Id, ctx.app.Id, categoryId, 20 );
            bindPhotoList( posts, categoryId );
        }

        private void bindPhotoList( DataPage<PhotoPost> posts, long categoryId ) {


            String albumName = "";
            if (categoryId > 0) {
                PhotoAlbum album = albumService.GetById( categoryId, ctx.owner.Id );
                if (album != null) albumName = "：" + album.Name;
            }

            set( "album.Title", albumName );

            setCategoryDropList();
            IBlock block = getBlock( "list" );
            foreach (PhotoPost post in posts.Results) {
                block.Set( "data.ImgThumbUrl", post.ImgThumbUrl );
                block.Set( "data.Title", strUtil.CutString( post.Title, 10 ) );
                block.Set( "data.Id", post.Id );
                block.Set( "data.Url", alink.ToAppData( post ) );
                block.Set( "data.EditUrl", to( new PostController().Edit, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
            target( Admin );
        }

        private void bindLink( IBlock tpl, String lbl, object obj ) {
            tpl.Set( "c.LinkPostAdmin", to( new MyController().Category, ((IEntity)obj).Id ) );
        }

        private void setCategoryDropList() {
            List<PhotoAlbum> albumList = albumService.GetListByApp( ctx.app.Id );
            PhotoAlbum album = new PhotoAlbum();
            album.Id = -1;
            album.Name = alang( "moveToAlbum" );
            List<PhotoAlbum> list = new List<PhotoAlbum>();
            list.Add( album );
            foreach (PhotoAlbum a in albumList) {
                list.Add( a );
            }
            dropList( "adminDropCategoryList", list, "Name=Id", null );
        }

        [HttpPost, DbTransaction]
        public virtual void Admin() {

            String ids = ctx.Post( "choice" );
            String cmd = ctx.Post( "action" );
            long categoryId = ctx.PostLong( "categoryId" );

            if (strUtil.IsNullOrEmpty( cmd )) {
                content( lang( "exCmd" ) );
                return;
            }
            if (cvt.IsIdListValid( ids ) == false) {
                content( lang( "exId" ) );
                return;
            }

            if (cmd.Equals( "category" )) {
                postService.UpdateAlbum( categoryId, ids, ctx.owner.Id, ctx.app.Id );
                content( "ok" );
            }
            else if (cmd.Equals( "deletetrue" )) {
                postService.DeleteTrue( ids, ctx.owner.Id );
                content( "ok" );
            }
            else {
                content( lang( "exUnknowCmd" ) );
            }

        }


    }
}

