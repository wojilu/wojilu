/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Interface;
using System.Collections.Generic;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Photo.Vo;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Photo.Admin {

    [App( typeof( PhotoApp ) )]
    public class AlbumController : ControllerBase {

        public IPhotoAlbumService albumService { get; set; }
        public IPhotoPostService postService { get; set; }

        public AlbumController() {
            base.HideLayout( typeof( Photo.LayoutController ) );
            albumService = new PhotoAlbumService();
            postService = new PhotoPostService();
        }

        public void OrderList() {

            set( "addLink", to( Add ) );
            set( "sortAction", to( SaveSort ) );

            IBlock block = getBlock( "list" );
            IList albumList = albumService.GetListByApp( ctx.app.Id );
            foreach (PhotoAlbum album in albumList) {
                block.Set( "category.Id", album.Id );
                block.Set( "category.Title", album.Name );
                block.Set( "category.OrderId", album.OrderId );
                block.Set( "category.Description", album.Description );
                block.Set( "category.EditUrl", to( Edit, album.Id ) );
                block.Set( "category.DeleteUrl", to( Delete, album.Id ) );
                block.Next();
            }
        }

        public void List() {

            set( "addLink", to( Add ) );
            set( "sortLink", to( OrderList ) );

            List<PhotoAlbum> albumList = albumService.GetListByApp( ctx.app.Id );
            IBlock block = getBlock( "list" );
            foreach (PhotoAlbum album in albumList) {

                block.Set( "album.Name", album.Name );
                block.Set( "album.Link", to( Edit, album.Id ) );

                int dataCount = getDataCount( album );
                block.Set( "album.DataCount", dataCount );
                block.Set( "album.Updated", album.Created.ToShortDateString() );

                String desc = strUtil.HasText( album.Description ) ? "<div class=\"descriptionInfo\">" + album.Description + "</div>" : "";
                block.Set( "album.Description", desc );

                String coverImg = PhotoHelper.getCover( album );
                block.Set( "album.Cover", coverImg );


                block.Next();
            }

        }

        private int getDataCount( PhotoAlbum album ) {
            int count = PhotoPost.find( "AppId=" + ctx.app.Id + " and PhotoAlbum.Id=" + album.Id ).count();
            album.DataCount = count;
            if (album.DataCount != count) {
                album.update( "DataCount" );
            }
            return count;
        }


        [HttpPost, DbTransaction]
        public virtual void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            PhotoAlbum acategory = db.findById<PhotoAlbum>( id );
            String condition = (ctx.app == null ? "" : "AppId=" + ctx.app.Id);

            List<PhotoAlbum> list = db.find<PhotoAlbum>( condition + " order by OrderId desc, Id asc" ).list();

            if (cmd == "up") {

                new SortUtil<PhotoAlbum>( acategory, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {

                new SortUtil<PhotoAlbum>( acategory, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        public void Add() {
            view( "Add" );
            target( Create );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            PhotoAlbum album = ctx.PostValue<PhotoAlbum>();
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            album.AppId = ctx.app.Id;
            album.OwnerId = ctx.owner.Id;
            album.OwnerUrl = ctx.owner.obj.Url;

            Result result = albumService.Create( album );
            if (result.IsValid) {
                echoRedirectPart( lang( "opok" ), to( List ) );
            }
            else {
                errors.Join( result );
                run( Add );
            }
        }

        public void Edit( int id ) {
            PhotoAlbum album = albumService.GetById( id, ctx.owner.Id );
            if (album == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            target( Update, id );
            set( "category.Title", album.Name );
            set( "category.Description", album.Description );

            set( "album.Name", album.Name );

            DataPage<PhotoPost> postPage = postService.GetPostPageByAlbum( ctx.owner.Id, ctx.app.Id, id, 12 );

            IBlock block = getBlock( "list" );
            foreach (PhotoPost post in postPage.Results) {
                block.Set( "data.Title", post.Title );
                block.Set( "data.ImgThumbUrl", post.ImgThumbUrl );
                block.Set( "data.EditLink", to( new PostController().Edit, post.Id ) );

                String lblCover = string.Format( "<a href=\"{0}\" class=\"putCmd\">设为专辑封面</a>", to( SetCover, post.Id ) );
                if (post.DataUrl.Equals( album.Logo )) lblCover = "<span class=\"currentCover\">封面</span>";

                block.Set( "data.SetCoverLink", lblCover );


                block.Next();
            }

            set( "page", postPage.PageBar );
        }

        [HttpPut, DbTransaction]
        public void SetCover( int id ) {

            PhotoPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            PhotoAlbum album = albumService.GetById( post.PhotoAlbum.Id, ctx.owner.Id );

            album.Logo = post.DataUrl;
            album.update( "Logo" );

            echoRedirectPart( lang( "opok" ), to( Edit, album.Id ) );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {
            PhotoAlbum album = albumService.GetById( id, ctx.owner.Id );
            if (album == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            //getByPostValue( album );
            album = ctx.PostValue( album ) as PhotoAlbum;
            if (ctx.HasErrors) { echoError(); return; }

            Result result = albumService.Update( album );
            if (result.IsValid) {
                echoRedirect( lang( "opok" ) );
            }
            else {
                errors.Join( result );
                echoError();
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            PhotoAlbum album = albumService.GetById( id, ctx.owner.Id );
            if (album == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            albumService.Delete( album );
            echoRedirectPart( lang( "opok" ), to( List ) );
        }

    }
}

