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

namespace wojilu.Web.Controller.Photo.Wf {

    public class HomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }

        public HomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();

            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );
        }



        [Data( typeof( PhotoPost ) )]
        public void Post( int id ) {


            PhotoPost x = ctx.Get<PhotoPost>();

            Boolean isLiked = PhotoLike.find( "UserId=" + ctx.viewer.Id + " and PostId=" + id ).first() != null;

            List<int> ids = new List<int>();
            if (isLiked) {
                ids.Add( id );
            }

            bindPostSingle( base.utils.getCurrentView(), x, ids );
        }


        //-------------------------------------------------------------------------------------------

        public void Index() {

            // 从第二页开始，是ajax获取，所以不需要多余的layout内容
            if (CurrentRequest.getCurrentPage() > 1) {
                HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
            }

            // 1) 超过最大滚页数，则不再自动翻页
            int maxPage = 10;
            if (CurrentRequest.getCurrentPage() > maxPage) {
                echoText( "." );
                return;
            }

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal, 20 );

            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount) {
                echoText( "." );
                return;
            }

            bindPhotoList( list );
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

                echoAjaxOk();

            }
        }

        [HttpPost, Login, Data( typeof( PhotoPost ) )]
        public void UnLike( int postId ) {

            PhotoLike p = PhotoLike.find( "PostId=:pid and UserId=:uid" )
                .set( "pid", postId )
                .set( "uid", ctx.viewer.Id )
                .first();

            if (p == null) {

                echoText( "对不起，尚未收藏" );
            }
            else {

                p.delete();
                echoAjaxOk();

            }
        }

        [Login, Data( typeof( PhotoPost ) )]
        public void Repin( int postId ) {

            target( RepinSave, postId );

            PhotoPost x = ctx.Get<PhotoPost>();

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

            photo.Creator = (User)ctx.viewer.obj;
            photo.CreatorUrl = ctx.viewer.obj.Url;
            photo.OwnerId = photo.Creator.Id;
            photo.OwnerUrl = photo.Creator.Url;
            photo.OwnerType = photo.Creator.GetType().FullName;

            photo.Title = x.Title;
            photo.DataUrl = x.DataUrl;
            photo.Ip = ctx.Ip;

            return photo;
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
            block.Set( "x.Link", to( Post, x.Id ) );
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

            block.Set( "x.RepinLink", to( Repin, x.Id ) );
            block.Set( "x.LikeLink", to( Like, x.Id ) );
            block.Set( "x.UnLikeLink", to( UnLike, x.Id ) );

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
