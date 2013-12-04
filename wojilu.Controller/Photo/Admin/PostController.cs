/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Members.Users.Interface;
using wojilu.Apps.Photo.Interface;
using wojilu.Web.Controller.Admin;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs;

namespace wojilu.Web.Controller.Photo.Admin {


    [App( typeof( PhotoApp ) )]
    public class PostController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PhotoController ) );

        public virtual IFriendService friendService { get; set; }

        public virtual IPhotoPostService postService { get; set; }
        public virtual IPhotoAlbumService albumService { get; set; }
        public virtual IPhotoSysCategoryService categoryService { get; set; }
        public virtual IMicroblogService microblogService { get; set; }

        public PostController() {

            base.HideLayout( typeof( Photo.LayoutController ) );

            friendService = new FriendService();

            postService = new PhotoPostService();
            albumService = new PhotoAlbumService();

            categoryService = new PhotoSysCategoryService();
            microblogService = new MicroblogService();
        }

        public virtual void Add() {

            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );
            String uploadUrl = strUtil.Join( ctx.url.SiteUrl, to( SaveUpload ) );
            set( "uploadLink", uploadUrl );
            set( "feedLink", to( AddFeed ) );


            IList albumList = albumService.GetListByApp( ctx.app.Id );
            if (albumList.Count == 0) {
                echoRedirectPart( alang( "exAlbumRequire" ), to( new AlbumController().Add ), 1 );
                return;
            }

            target( NewPost, 0 );

            List<PhotoSysCategory> categories = categoryService.GetForDroplist();

            dropList( "PhotoAlbumId", albumList, "Name=Id", 1 );
            dropList( "SystemCategoryId", categories, "Name=Id", null );

            set( "PhotoAlbumAddUrl", to( new AlbumController().Add ) );
            //set( "batchUploadLink", to( BatchAdd ) );

            // swf上传跨域问题
            set( "jsPath", sys.Path.DiskJs );
        }

        public virtual void NewPost( long albumId ) {

            if (albumId <= 0) {
                albumId = ctx.PostLong( "PhotoAlbumId" );
            }

            PhotoAlbum album = albumService.GetById( albumId, ctx.owner.Id );
            if (album == null) { echo( lang( "exDataNotFound" ) + ": albumId=" + albumId ); return; }

            long systemCategoryId = ctx.PostLong( "SystemCategoryId" );
            if (systemCategoryId <= 0) {
                errors.Add( alang( "exSysCategoryRequire" ) );
                run( Add );
                return;
            }

            set( "photoAlbum", album.Name );

            set( "PhotoAlbumId", album.Id );
            set( "SystemCategoryId", systemCategoryId );

            target( Create );
            int count = 5;
            IBlock block = getBlock( "list" );
            for (int i = 1; i < (count + 1); i++) {
                block.Set( "photoIndex", i );
                block.Next();
            }
        }

        // flash上传(逐个保存)
        public virtual void SaveUpload() {

            long albumId = ctx.PostLong( "PhotoAlbumId" );
            long systemCategoryId = ctx.PostLong( "SystemCategoryId" );

            if (ctx.HasUploadFiles == false) {
                echoText( lang( "exPlsUpload" ) );
                return;
            }

            HttpFile file = ctx.GetFileSingle();

            logger.Info( file.FileName + "=>" + file.ContentType );

            Result result = Uploader.SaveImg( file );
            if (result.HasErrors) {
                errors.Join( result );
                echoError( result );
            }
            else {
                PhotoPost post = newPost( Path.GetFileNameWithoutExtension( file.FileName ), result.Info.ToString(), albumId, systemCategoryId );
                PhotoApp app = ctx.app.obj as PhotoApp;
                postService.CreatePost( post, app );

                // 统计
                User user = ctx.owner.obj as User;
                user.Pins = PhotoPost.count( "OwnerId=" + user.Id );
                user.update( "Pins" );

                Dictionary<String, long> dic = new Dictionary<String, long>();
                dic.Add( "Id", post.Id );

                echoJsonMsg( "ok", true, dic );
            }
        }

        [HttpPost, DbTransaction]
        public virtual void AddFeed() {
            String ids = ctx.PostIdList( "ids" );
            long albumId = ctx.PostLong( "albumId" );

            if (strUtil.IsNullOrEmpty( ids )) return;

            List<PhotoPost> posts = PhotoPost.find( "Id in (" + ids + ")" ).list();
            addFeedInfo( posts, albumId );

            echoAjaxOk();
        }

        // 普通上传(批量)
        [HttpPost, DbTransaction]
        public virtual void Create() {

            long albumId = ctx.PostLong( "PhotoAlbumId" );
            long systemCategoryId = ctx.PostLong( "SystemCategoryId" );

            if (albumId <= 0) {
                errors.Add( alang( "exAlbumSelect" ) );
                run( NewPost, albumId );
                return;
            }

            if (systemCategoryId <= 0) {
                errors.Add( alang( "exSysCategoryRequire" ) );
                run( NewPost, albumId );
                return;
            }

            if (ctx.GetFiles().Count <= 0) {
                errors.Add( alang( "exUploadImg" ) );
                run( NewPost, albumId );
                return;
            }

            List<PhotoPost> imgs = new List<PhotoPost>();
            for (int i = 0; i < ctx.GetFiles().Count; i++) {

                HttpFile file = ctx.GetFiles()[i];

                if (file.ContentLength < 10) continue;

                // 发生任何错误，则返回
                Result result = Uploader.SaveImg( file );
                if (result.HasErrors) {
                    echo( result.ErrorsHtml );
                    return;
                }

                PhotoPost post = newPost( ctx.Post( "Text" + (i + 1) ), result.Info.ToString(), albumId, systemCategoryId );
                PhotoApp app = ctx.app.obj as PhotoApp;
                postService.CreatePost( post, app );
                imgs.Add( post );
            }

            // 如果没有上传的图片
            if (imgs.Count == 0) {
                errors.Add( alang( "exUploadImg" ) );
                run( NewPost, albumId );
                return;
            }

            // 统计
            User user = ctx.owner.obj as User;
            user.Pins = PhotoPost.count( "OwnerId=" + user.Id );
            user.update( "Pins" );

            // feed消息
            addFeedInfo( imgs, albumId );

            echoRedirectPart( lang( "opok" ), to( new MyController().My ), 1 );
        }

        private void addFeedInfo( List<PhotoPost> imgs, long albumId ) {
            String msg = postService.GetFeedMsg( imgs );
            microblogService.AddSimple( imgs[0].Creator, msg, typeof( PhotoPost ).FullName, imgs[0].Id, ctx.Ip );
        }

        private PhotoPost newPost( String photoName, String imgPath, long albumId, long systemCategoryId ) {

            PhotoAlbum album = new PhotoAlbum();
            album.Id = albumId;

            PhotoPost photo = new PhotoPost();

            photo.AppId = ctx.app.Id;
            photo.SysCategoryId = systemCategoryId;
            photo.PhotoAlbum = album;

            photo.Creator = (User)ctx.viewer.obj;
            photo.CreatorUrl = ctx.viewer.obj.Url;
            photo.OwnerId = ctx.owner.Id;
            photo.OwnerUrl = ctx.owner.obj.Url;
            photo.OwnerType = ctx.owner.obj.GetType().FullName;

            photo.Title = photoName;
            photo.DataUrl = imgPath;
            photo.Ip = ctx.Ip;

            return photo;
        }

        public virtual void Edit( long id ) {

            PhotoPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            target( Update, id );

            set( "data.Title", post.Title );
            set( "data.ImgUrl", post.ImgThumbUrl );
            set( "data.Description", post.Description );
            set( "data.TagList", post.Tag.TextString );

            dropList( "PhotoAlbumId", albumService.GetListByApp( ctx.app.Id ), "Name=Id", post.PhotoAlbum.Id );
            dropList( "SystemCategoryId", categoryService.GetForDroplist(), "Name=Id", post.SysCategoryId );

            set( "returnUrl", ctx.web.PathReferrer );
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
        public virtual void Update( long id ) {

            PhotoPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) { echoRedirect( lang( "exDataNotFound" ) ); return; }

            PhotoAlbum album = new PhotoAlbum();
            album.Id = ctx.PostLong( "PhotoAlbumId" );

            post.SysCategoryId = ctx.PostLong( "SystemCategoryId" );
            post.Title = ctx.Post( "Title" );
            post.PhotoAlbum = album;
            post.Description = ctx.Post( "Description" );

            if (post.SysCategoryId <= 0) errors.Add( alang( "exSysCategoryRequire" ) );
            if (strUtil.IsNullOrEmpty( post.Title )) errors.Add( alang( "exPhotoName" ) );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            Result result = postService.Update( post );
            if (result.IsValid) {
                TagService.SaveDataTag( post, ctx.Post( "TagList" ) );
                echoToParentPart( lang( "opok" ), to( new MyController().My ) );
            }
            else {
                echoError();
            }

        }


    }
}

