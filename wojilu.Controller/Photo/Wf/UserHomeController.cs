using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Interface;
using wojilu.Apps.Photo.Interface;
using wojilu.Members.Users.Service;
using wojilu.Apps.Photo.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Photo.Wf {



    public class UserHomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPhotoSysCategoryService sysCategoryService { get; set; }

        public UserHomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();
            sysCategoryService = new PhotoSysCategoryService();

            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
        }

        public override void Layout() {
            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );
            bindUserInfo( u );

            bindAdminCmd( u );
        }

        private void bindAdminCmd( User u ) {

            String adminCmd = "";
            if (u.Id == ctx.viewer.Id) {
                adminCmd = string.Format( "<a href='{0}' target='_blank'><i class='icon-picture'></i> 管理图片</a> <a href='{1}' class='left20' target='_blank'><i class='icon-th'></i> 管理分类</a>", PhotoLink.ToAdminPost( u ), PhotoLink.ToAdminAlbum( u ) );
            }

            set( "adminCmd", adminCmd );
        }

        private void bindUserInfo( User u ) {
            set( "u.Name", u.Name );
            set( "u.Created", u.Created.ToShortDateString() );
            set( "u.PicMedium", u.PicMedium );
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
                getFullUrl( PhotoLink.ToUser( u ) ), u.Name + "的图片首页", u.PicOriginal );

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


        public void Index() {

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }


            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal + " and OwnerId=" + u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, u.Id );
        }

        public void Category( int id ) {

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            PhotoAlbum album = categoryService.GetById( id );
            set( "albumName", album.Name );

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal + " and CategoryId=" + id + " and OwnerId=" + u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, list, u.Id );

        }

        public void Like() {

            view( "Index" );

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            DataPage<PhotoLike> list = PhotoLike.findPage( "UserId=" + u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount) {
                echoText( "." );
                return;
            }

            PhotoBinder.BindPhotoList( this, getPostPage( list ), ctx.viewer.Id );
        }

        private DataPage<PhotoPost> getPostPage( DataPage<PhotoLike> list ) {

            DataPage<PhotoPost> results = new DataPage<PhotoPost>( list );
            List<PhotoPost> xlist = new List<PhotoPost>();
            foreach (PhotoLike x in list.Results) {
                xlist.Add( x.Post );
            }
            results.Results = xlist;
            return results;
        }

        public void Album() {

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            List<PhotoAlbum> albumList = db.find<PhotoAlbum>( "OwnerId=" + u.Id ).list();
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
