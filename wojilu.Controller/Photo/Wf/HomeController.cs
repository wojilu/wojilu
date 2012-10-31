using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Common;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Photo.Wf {

    [App( typeof( PhotoApp ) )]
    public class HomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public IFollowerService followerService { get; set; }

        public HomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();
            pickedService = new PickedService();
            followerService = new FollowerService();

            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
        }

        public override void Layout() {
        }

        [Data( typeof( PhotoPost ) )]
        public void Post( int id ) {


            PhotoPost x = ctx.Get<PhotoPost>();
            postService.AddtHits( x );

            WebUtils.pageTitle( this, x.Title );
            Page.Keywords = x.Tag.TextString;

            User owner = x.Creator;

            if (ctx.viewer.IsFollowing( owner.Id )) {
                set( "lblFollow", "已经关注" );
                set( "clsFollow", "btnUnFollow" );
            }
            else {
                set( "lblFollow", "关注" );
                set( "clsFollow", "btnFollow" );
            }

            Boolean isLiked = PhotoLike.find( "UserId=" + ctx.viewer.Id + " and PostId=" + id ).first() != null;

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
                + "&dataId=" + x.Id;
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

            List<PhotoPost> list = PhotoPost.find( "CategoryId=" + post.PhotoAlbum.Id + " and SaveStatus=" + SaveStatus.Normal ).list( 9 );
            foreach (PhotoPost x in list) {
                PhotoBinder.BindPostSingle( ctx, block, x, new List<int>() );
                block.Next();
            }

        }

        private void bindOtherPosts() {
            List<PhotoPost> list = PhotoPost.find( "SaveStatus=" + SaveStatus.Normal ).list( 15 );
            IBlock block = getBlock( "xposts" );
            foreach (PhotoPost x in list) {
                PhotoBinder.BindPostSingle( ctx, block, x, new List<int>() );
                block.Next();
            }
        }

        //-------------------------------------------------------------------------------------------

        [Login]
        public void Add() {

            User u = ctx.viewer.obj as User;

            PhotoApp app = PhotoApp.find( "OwnerId=" + u.Id ).first();
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
            String ids = followerService.GetFollowingIds( ctx.viewer.Id );
            ids = strUtil.HasText( ids ) ? ids + "," + ctx.viewer.Id : ctx.viewer.Id.ToString();
            DataPage<PhotoPost> list = PhotoPost.findPage( "OwnerId in (" + ids + ") and SaveStatus=" + SaveStatus.Normal, 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }


        public void Index() {

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

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal, 20 );

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

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal + " and SysCategoryId=" + categoryId, 20 );

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

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal + " order by Likes desc, Pins desc, Hits desc, Replies desc", 20 );

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

            DataPage<PhotoPost> list = pickedService.GetAll( 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }

        //------------------------------------------------------------------------------------------

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void Like( int postId ) {

            PhotoLike p = PhotoLike.find( "PostId=:pid and UserId=:uid" )
                .set( "pid", postId )
                .set( "uid", ctx.viewer.Id )
                .first();

            if (p != null) {

                echoText( "对不起，已经收藏" );
            }
            else {

                PhotoPost post = ctx.Get<PhotoPost>();

                PhotoLike x = new PhotoLike();
                x.Post = post;
                x.User = ctx.viewer.obj as User;
                x.insert();

                post.Likes = PhotoLike.count( "PostId=" + postId );
                post.update();

                User user = ctx.viewer.obj as User;
                user.Likes = PhotoLike.count( "UserId=" + user.Id );
                user.update( "Likes" );

                echoAjaxOk();

            }
        }

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void UnLike( int postId ) {

            PhotoPost post = ctx.Get<PhotoPost>();

            PhotoLike p = PhotoLike.find( "PostId=:pid and UserId=:uid" )
                .set( "pid", postId )
                .set( "uid", ctx.viewer.Id )
                .first();

            if (p == null) {

                echoText( "对不起，尚未收藏" );
            }
            else {

                p.delete();

                post.Likes = PhotoLike.count( "PostId=" + postId );
                post.update();

                User user = ctx.viewer.obj as User;
                user.Likes = PhotoLike.count( "UserId=" + user.Id );
                user.update( "Likes" );

                echoAjaxOk();

            }
        }

        [Login, Data( typeof( PhotoPost ) )]
        public void Repin( int postId ) {

            target( RepinSave, postId );

            set( "lnkAlbumAdd", to( AlbumAdd ) );

            PhotoPost x = ctx.Get<PhotoPost>();

            String condition = "OwnerId=" + ctx.viewer.Id + " and ";
            condition += x.RootId > 0
                ? "(RootId=" + postId + " or RootId=" + x.RootId + ")"
                : "RootId=" + postId;

            Boolean isPin = (x.OwnerId == ctx.viewer.Id || PhotoPost.find( condition ).first() != null);
            String pinInfo = isPin ? "<div class='wfWarning'>提醒：您已经收集过本图片</div>" : "";
            set( "pinInfo", pinInfo );

            set( "x.Pic", x.ImgThumbUrl );

            List<PhotoAlbum> categories = categoryService.GetListByUser( ctx.viewer.Id );

            dropList( "categoryId", categories, "Name=Id", null );
        }

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void RepinSave( int postId ) {

            PhotoPost x = ctx.Get<PhotoPost>();

            PhotoPost photo = newPost( x );

            photo.insert();
            photo.Tag.Save( ctx.Post( "tagList" ) );
            // TODO 动态消息

            x.Pins = PhotoPost.count( "RootId=" + postId + " or ParentId=" + postId );
            x.update( "Pins" );

            User user = ctx.viewer.obj as User;
            user.Pins = PhotoPost.count( "OwnerId=" + user.Id );
            user.update( "Pins" );

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
            PhotoApp app = PhotoApp.find( "OwnerId=" + userId ).first();
            if (app == null) return 0;
            return app.Id;
        }

        private PhotoPost newPost( PhotoPost x ) {

            PhotoPost photo = new PhotoPost();

            PhotoAlbum album = categoryService.GetById( ctx.PostInt( "categoryId" ) );

            photo.PhotoAlbum = album;
            photo.Description = ctx.Post( "description" );

            //----------------------------------------------------------

            photo.ParentId = x.Id;
            photo.RootId = x.RootId > 0 ? x.RootId : x.Id;
            photo.AppId = album.AppId;

            //----------------------------------------------------------

            photo.SysCategoryId = x.SysCategoryId;

            User user = ctx.viewer.obj as User;

            photo.Creator = user;
            photo.CreatorUrl = user.Url;
            photo.OwnerId = user.Id;
            photo.OwnerUrl = user.Url;
            photo.OwnerType = user.GetType().FullName;

            photo.Title = x.Title;
            photo.DataUrl = x.DataUrl;
            photo.Ip = ctx.Ip;

            return photo;
        }

        //------------------------------------------------------------------------------------------


    }
}
