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
        }

        private void bindUserInfo( User u ) {
            set( "u.Name", u.Name );
            set( "u.Created", u.Created.ToShortDateString() );
            set( "u.PicMedium", u.PicMedium );
            set( "u.Link", PhotoLinker.getUserLink( u ) );

            set( "u.LikeLink", PhotoLinker.getUserLikeLink( u ) );
            set( "u.AlbumLink", PhotoLinker.getUserAlbumLink( u ) );
            set( "u.FollowerLink", PhotoLinker.getUserFollowerLink( u ) );
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

            bindPhotoList( list );
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

            bindPhotoList( list );

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

            bindPhotoList( getPostPage( list ) );
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
                block.Set( "album.Link", PhotoLinker.getCategoryLink( album.OwnerUrl, album.Id ) );

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

        //------------------------------------------------------------------------------------------


        private void bindPhotoList( DataPage<PhotoPost> list ) {
            IBlock block = getBlock( "list" );

            List<int> likedIds = getLikedIds( list.Results );

            foreach (PhotoPost x in list.Results) {
                bindPostSingle( block, x, likedIds );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        private List<int> getLikedIds( List<PhotoPost> list ) {

            List<int> ids = new List<int>();

            if (list.Count == 0) return ids;

            String postIds = "";
            foreach (PhotoPost x in list) {
                postIds += x.Id + ",";
            }
            postIds = postIds.TrimEnd( ',' );

            List<PhotoLike> likeList = PhotoLike.find( "UserId=" + ctx.viewer.Id + " and PostId in (" + postIds + ")" ).list();

            foreach (PhotoLike x in likeList) {
                ids.Add( x.Post.Id );
            }

            return ids;
        }



        private void bindPostSingle( IBlock block, PhotoPost x, List<int> likedIds ) {
            block.Set( "x.Link", to( new HomeController().Post, x.Id ) );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Pic", x.ImgThumbUrl );
            block.Set( "x.PicM", x.ImgMediumUrl );

            if (x.PhotoAlbum != null) {
                block.Set( "x.AlbumName", x.PhotoAlbum.Name );
                block.Set( "x.AlbumLink", PhotoLinker.getCategoryLink( x.PhotoAlbum.OwnerUrl, x.PhotoAlbum.Id ) );
            }
            else {
                block.Set( "x.AlbumName", "" );
                block.Set( "x.AlbumLink", "#" );
            }

            block.Set( "x.CreatorName", x.Creator.Name );
            block.Set( "x.CreatorPic", x.Creator.PicSmall );
            block.Set( "x.CreatorLink", PhotoLinker.getUserLink( x.Creator ) );
            block.Set( "x.Created", cvt.ToTimeString( x.Created ) );

            block.Set( "x.RepinLink", to( new HomeController().Repin, x.Id ) );
            block.Set( "x.LikeLink", to( new HomeController().Like, x.Id ) );
            block.Set( "x.UnLikeLink", to( new HomeController().UnLike, x.Id ) );

            if (likedIds.Contains( x.Id )) {
                block.Set( "x.LikedCss", "wfpost-liked disabled" );
                block.Set( "x.LikeName", "已喜欢" );
            }
            else {
                block.Set( "x.LikedCss", "wfpost-like" );
                block.Set( "x.LikeName", "喜欢" );
            }

        }


    }

}
