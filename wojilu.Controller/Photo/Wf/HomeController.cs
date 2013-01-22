/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo;


namespace wojilu.Web.Controller.Photo.Wf {

    [App( typeof( PhotoApp ) )]
    public class HomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public IPhotoService photoService { get; set; }
        public ISysPhotoService sysPostService { get; set; }

        public PhotoLikeService likeService { get; set; }

        public HomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();
            pickedService = new PickedService();
            likeService = new PhotoLikeService();
            photoService = new PhotoService();
            sysPostService = new SysPhotoService();

            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
        }

        public override void Layout() {
        }

        public void Index() {

            ctx.Page.Title = PhotoAppSetting.Instance.MetaTitle;
            ctx.Page.Keywords = PhotoAppSetting.Instance.MetaKeywords;
            ctx.Page.Description = PhotoAppSetting.Instance.MetaDescription;

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.Wf.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            DataPage<PhotoPost> list = sysPostService.GetShowRecent( 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }

        public void Category( int categoryId ) {

            view( "Index" );

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.Wf.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            DataPage<PhotoPost> list = sysPostService.GetShowByCategory( categoryId, 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );

        }

        public void Hot() {

            view( "Index" );

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.Wf.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            DataPage<PhotoPost> list = sysPostService.GetShowHot( 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }


        public void Pick() {

            view( "Index" );

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.Wf.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            DataPage<PhotoPost> list = pickedService.GetShowAll( 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }

        //------------------------------------------------------------------------------------------


        [Login]
        public void Add() {

            User u = ctx.viewer.obj as User;

            PhotoApp app = photoService.GetByUser( u.Id );
            if (app == null) {
                echoError( "请先添加PhotoApp" );
                return;
            }

            redirectUrl( Link.To( u, new Photo.Admin.PostController().Add, app.Id ) );
        }


        [Login]
        public void Following() {

            view( "Index" );

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
                HideLayout( typeof( wojilu.Web.Controller.Photo.Wf.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            // 关注的图片
            DataPage<PhotoPost> list = postService.GetFollowing( ctx.viewer.Id, 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }

        //------------------------------------------------------------------------------------------

        [Data( typeof( PhotoPost ) )]
        public void Post( int id ) {


            PhotoPost x = ctx.Get<PhotoPost>();
            postService.AddtHits( x );

            ctx.Page.Title = x.Title;
            ctx.Page.Keywords = x.Tag.TextString;

            User owner = x.Creator;

            if (ctx.viewer.IsFollowing( owner.Id )) {
                set( "lblFollow", "已经关注" );
                set( "clsFollow", "btnUnFollow" );
            }
            else {
                set( "lblFollow", "关注" );
                set( "clsFollow", "btnFollow" );
            }

            Boolean isLiked = likeService.IsLiked( ctx.viewer.Id, id );

            List<int> ids = new List<int>();
            if (isLiked) {
                ids.Add( id );
            }

            PhotoBinder.BindPostSingleFull( ctx, base.utils.getCurrentView(), x, ids );
            set( "lnkPrevNext", getPreNextHtml( x ) );

            bindAlbumPosts( x );
            bindOtherPosts();

            String commentUrl = t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + PhotoLink.ToPost( x.Id )
                + "&dataType=" + typeof( PhotoPost ).FullName
                + "&dataTitle=" + x.Title
                + "&dataUserId=" + x.Creator.Id
                + "&dataId=" + x.Id
                + "&appId=" + x.AppId;
            
            set( "thisUrl", commentUrl );
        }

        public String getPreNextHtml( PhotoPost post ) {

            PhotoPost prev = postService.GetPre( post );
            PhotoPost next = postService.GetNext( post );

            String prenext;
            if (prev == null && next == null)
                prenext = "";
            else if (prev == null)
                prenext = "<a href=\"" + PhotoLink.ToPost( next.Id ) + "\">" + alang( "nextPhoto" ) + "</a> ";
            else if (next == null)
                prenext = "<a href=\"" + PhotoLink.ToPost( prev.Id ) + "\">" + alang( "prevPhoto" ) + "</a> ";
            else
                prenext = "<a href=\"" + PhotoLink.ToPost( prev.Id ) + "\">" + alang( "prevPhoto" ) + "</a> | <a href=\"" + PhotoLink.ToPost( next.Id ) + "\">" + alang( "nextPhoto" ) + "</a>";
            return prenext;
        }

        private void bindAlbumPosts( PhotoPost post ) {

            IBlock block = getBlock( "cposts" );
            if (post.PhotoAlbum == null) return;

            List<PhotoPost> list = postService.GetByAlbum( post.PhotoAlbum.Id, 9 );
            foreach (PhotoPost x in list) {
                PhotoBinder.BindPostSingle( ctx, block, x, new List<int>() );
                block.Next();
            }

        }

        private void bindOtherPosts() {
            List<PhotoPost> list = postService.GetNew( 15 );
            IBlock block = getBlock( "xposts" );
            foreach (PhotoPost x in list) {
                PhotoBinder.BindPostSingle( ctx, block, x, new List<int>() );
                block.Next();
            }
        }


        //------------------------------------------------------------------------------------------

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void Like( int postId ) {

            Boolean isLiked = likeService.IsLiked( ctx.viewer.Id, postId );

            if (isLiked) {
                echoText( "对不起，已经喜欢" );
            }
            else {

                PhotoPost post = ctx.Get<PhotoPost>();
                likeService.Like( ctx.viewer.obj as User, post );

                echoAjaxOk();
            }
        }

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void UnLike( int postId ) {

            Boolean isLiked = likeService.IsLiked( ctx.viewer.Id, postId );

            if (!isLiked) {
                echoText( "对不起，尚未喜欢" );
            }
            else {
                PhotoPost post = ctx.Get<PhotoPost>();

                likeService.UnLike( ctx.viewer.obj as User, post );

                echoAjaxOk();

            }
        }

        [Login, Data( typeof( PhotoPost ) )]
        public void Repin( int postId ) {

            target( RepinSave, postId );

            set( "lnkAlbumAdd", to( AlbumAdd ) );

            PhotoPost x = ctx.Get<PhotoPost>();

            Boolean isPin = postService.IsPin( ctx.viewer.Id, x );
            String pinInfo = isPin ? "<div class='wfWarning'>提醒：您已经收集过本图片</div>" : "";
            set( "pinInfo", pinInfo );

            set( "x.Pic", x.ImgThumbUrl );

            List<PhotoAlbum> categories = categoryService.GetListByUser( ctx.viewer.Id );

            dropList( "categoryId", categories, "Name=Id", null );
        }

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void RepinSave( int postId ) {

            PhotoPost x = ctx.Get<PhotoPost>();
            PhotoPost photo = getFormPosted();

            postService.SavePin( x, photo, ctx.Post( "tagList" ) );
        }

        private PhotoPost getFormPosted() {

            PhotoPost photo = new PhotoPost();

            PhotoAlbum album = categoryService.GetById( ctx.PostInt( "categoryId" ) );

            photo.PhotoAlbum = album;
            photo.Description = ctx.Post( "description" );
            photo.AppId = album.AppId;

            User user = ctx.viewer.obj as User;

            photo.Creator = user;
            photo.CreatorUrl = user.Url;
            photo.OwnerId = user.Id;
            photo.OwnerUrl = user.Url;
            photo.OwnerType = user.GetType().FullName;

            photo.Ip = ctx.Ip;

            return photo;
        }



        [Login]
        public void AlbumAdd() {
            target( AlbumSave );
        }

        [Login]
        public void AlbumSave() {

            PhotoAlbum album = ctx.PostValue<PhotoAlbum>();
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            album.AppId = getAlbumAppId( ctx.viewer.Id );
            album.OwnerId = ctx.viewer.Id;
            album.OwnerUrl = ctx.viewer.obj.Url;

            Result result = categoryService.Create( album );

            List<PhotoAlbum> categories = categoryService.GetListByApp( album.AppId );
            dropList( "categoryId", categories, "Name=Id", album.Id );
            set( "cid", album.Id );
        }

        private int getAlbumAppId( int userId ) {
            PhotoApp app = photoService.GetByUser( userId );
            if (app == null) return 0;
            return app.Id;

        }



    }
}
