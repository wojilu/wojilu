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

namespace wojilu.Web.Controller.Photo {

    public class HomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoAlbumService categoryService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPhotoSysCategoryService sysCategoryService { get; set; }

        public HomeController() {
            userService = new UserService();
            categoryService = new PhotoAlbumService();
            postService = new PhotoPostService();
            sysCategoryService = new PhotoSysCategoryService();
          
            HideLayout( typeof( LayoutController ) );
        }

        public override void Layout() {

            set( "lnkNew", to( Index ) );

            bindCategories();

        }

        [Data( typeof( PhotoPost ) )]
        public void Post( int id ) {


            PhotoPost x = ctx.Get<PhotoPost>();


            bindPostSingle( base.utils.getCurrentView(), x );



        }

        public void Space() {

            String userUrl = ctx.route.getItem( "user" );
            User u = userService.GetByUrl( userUrl );

            if (u == null) {
                echoRedirect( "用户不存在" );
                return;
            }

            set( "u.Name", u.Name );
            set( "u.Created", u.Created.ToShortDateString() );
            set( "u.PicMedium", u.PicMedium );
            set( "u.Link", getUserLink( u ) );

            DataPage<PhotoPost> list = PhotoPost.findPage( "SaveStatus=" + SaveStatus.Normal + " and OwnerId=" + u.Id, 12 );
            // 2) 或者超过实际页数，也不再自动翻页
            if (CurrentRequest.getCurrentPage() > list.PageCount) {
                echoText( "." );
                return;
            }

            bindPhotoList( list );
        }

        private String getUserLink( User u ) {
            return string.Format( "/photo/{0}.aspx", u.Url );
        }

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
        public void Like( int  postId ) {


            PhotoPost post = ctx.Get<PhotoPost>();

            PhotoLike x = new PhotoLike();
            x.Post = post;
            x.User = ctx.viewer.obj as User;
            x.insert();

            echoAjaxOk();
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

        private void bindCategories() {
            List<PhotoSysCategory> categories = sysCategoryService.GetAll();
            IBlock cblock = getBlock( "categories" );
            foreach (PhotoSysCategory x in categories) {

                cblock.Set( "x.Name", x.Name );
                //cblock.Set( "x.LinkShow", to( List, x.Id ) );
                cblock.Next();
            }
        }

        private void bindPhotoList( DataPage<PhotoPost> list ) {
            IBlock block = getBlock( "list" );

            foreach (PhotoPost x in list.Results) {
                bindPostSingle( block, x );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        private void bindPostSingle( IBlock block, PhotoPost x ) {
            block.Set( "x.Link", to( Post, x.Id ) );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Pic", x.ImgThumbUrl );
            block.Set( "x.PicM", x.ImgMediumUrl );

            if (x.PhotoAlbum != null) {
                block.Set( "x.AlbumName", x.PhotoAlbum.Name );
                block.Set( "x.AlbumLink", Link.To( x.Creator, new PhotoController().Album, x.PhotoAlbum.Id, x.AppId ) );
            }
            else {
                block.Set( "x.AlbumName", "" );
                block.Set( "x.AlbumLink", "#" );
            }

            block.Set( "x.CreatorName", x.Creator.Name );
            block.Set( "x.CreatorPic", x.Creator.PicSmall );
            block.Set( "x.CreatorLink", getUserLink( x.Creator ) );
            block.Set( "x.Created", cvt.ToTimeString( x.Created ) );

            block.Set( "x.RepinLink", to( Repin, x.Id ) );
            block.Set( "x.LikeLink", to( Like, x.Id ) );
        }
    }
}
