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

namespace wojilu.Web.Controller.Photo {

    public class HomeController : ControllerBase {

        public IUserService userService { get; set; }
        public IPhotoSysCategoryService categoryService { get; set; }

        public HomeController() {
            userService = new UserService();
            categoryService = new PhotoSysCategoryService();
            HideLayout( typeof( LayoutController ) );
        }

        public override void Layout() {

            set( "lnkNew", to( Index ) );

            bindCategories();

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

        private void bindCategories() {
            List<PhotoSysCategory> categories = categoryService.GetAll();
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
                block.Set( "x.Link", alink.ToAppData( x ) );
                block.Set( "x.Title", x.Title );
                block.Set( "x.Pic", x.ImgThumbUrl );

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
                block.Next();
            }

            set( "page", list.PageBar );
        }
    }
}
