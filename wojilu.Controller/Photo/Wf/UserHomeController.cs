/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Common;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;

namespace wojilu.Web.Controller.Photo.Wf {

    [App( typeof( PhotoApp ) )]
    public class UserHomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPhotoSysCategoryService sysCategoryService { get; set; }
        public PhotoLikeService likeService { get; set; }

        public UserHomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();
            sysCategoryService = new PhotoSysCategoryService();
            likeService = new PhotoLikeService();

            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
        }

        public override void Layout() {
            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );
            bindUserInfo( u );
        }

        private void bindUserInfo( User u ) {
            set( "u.Name", u.Name );
            set( "u.Created", u.Created.ToShortDateString() );
            set( "u.PicMedium", u.PicM );
            set( "u.Link", PhotoLink.ToUser( u ) );

            set( "u.Gender", u.GenderString );
            set( "u.Hobby", getUserHobby( u ) );

            set( "u.Pins", u.Pins );
            set( "u.Likes", u.Likes );
            set( "u.Albums", getUserAlbums( u ) );
            set( "u.Followers", u.FollowersCount );

            set( "u.LikeLink", PhotoLink.ToLike( u ) );
            set( "u.AlbumLink", PhotoLink.ToAlbumList( u ) );
            set( "u.FollowerLink", Link.To( u, new Users.FriendController().FollowerList ) );

            String followCmd = WebUtils.getFollowCmd( ctx, u.Id );
            set( "followCmd", followCmd );

            String lnkMsg;
            if (ctx.viewer.IsLogin) {
                lnkMsg = Link.To( ctx.viewer.obj, new Users.Admin.MsgController().New, u.Id );
            }
            else {
                lnkMsg = "#";
            }

            set( "sendMsgLink", lnkMsg );

            String shareLink = Link.To( ctx.owner.obj, new wojilu.Web.Controller.ShareController().Add );
            shareLink = shareLink + string.Format( "?url={0}&title={1}&pic={2}",
                getFullUrl( PhotoLink.ToUser( u ) ), u.Name + "的图片首页", u.PicO );

            set( "shareLink", shareLink );
        }

        private object getUserAlbums( User u ) {

            return ndb.count( typeof( PhotoAlbum ), "OwnerId=" + u.Id );
        }

        private object getUserHobby( User u ) {

            return string.Format( "<div>{0}</div><div>{1}</div><div>{2}</div><div>{3}</div><div>{4}</div><div>{5}</div>", u.Profile.Music, u.Profile.Movie, u.Profile.Sport, u.Profile.Eat, u.Profile.Book, u.Profile.OtherHobby );

        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }
        //-------------------------------------------------------------------------------------------

        private void setWaterfallView() {
            view( "/Photo/Wf/Home/Index" );
        }

        public void Index() {

            setWaterfallView();

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            DataPage<PhotoPost> list = postService.GetShowByUser( u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, u.Id );
        }

        public void Category( int id ) {

            setWaterfallView();

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            PhotoAlbum album = categoryService.GetById( id );
            set( "albumName", album.Name );
            set( "showAlbumCss", "display:block" );
            set( "albumDataCount", album.DataCount );


            DataPage<PhotoPost> list = postService.GetShowByUser( u.Id, id, 12 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, u.Id );
        }

        public void Like() {

            setWaterfallView();

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            DataPage<PhotoPost> list = likeService.GetByUser( u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount && list.PageCount > 0) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, ctx.viewer.Id );
        }

        public void Album() {

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            List<PhotoAlbum> albumList = categoryService.GetListByUser( u.Id );
            bindAlbumList( albumList );
        }

        private void bindAlbumList( List<PhotoAlbum> albumList ) {
            IBlock block = getBlock( "list" );
            foreach (PhotoAlbum album in albumList) {

                block.Set( "album.Name", album.Name );
                block.Set( "album.Link", PhotoLink.ToAlbumOne( album.OwnerUrl, album.Id ) );

                int dataCount = PhotoHelper.getDataCount( album );
                block.Set( "album.DataCount", dataCount );
                block.Set( "album.Updated", album.Created.ToShortDateString() );

                String desc = strUtil.HasText( album.Description ) ? "<div>" + album.Description + "</div>" : "";
                block.Set( "album.Description", desc );

                String coverImg = PhotoHelper.getCover( album );
                block.Set( "album.Cover", coverImg );


                block.Next();
            }
        }

        public void Follower() {
        }

    }

}
