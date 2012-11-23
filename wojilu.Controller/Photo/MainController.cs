/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Photo.Caching;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Photo {

    [App( typeof( PhotoApp ) )]
    public class MainController : ControllerBase {

        public ISysPhotoService photoService { get; set; }
        public IPickedService PickedService { get; set; }
        public IPhotoSysCategoryService categoryService { get; set; }
        public IPhotoRankService rankService { get; set; }


        public MainController() {

            photoService = new SysPhotoService();
            PickedService = new PickedService();
            categoryService = new PhotoSysCategoryService();
            rankService = new PhotoRankService();

            HideLayout( typeof( LayoutController ) );

        }

        [CacheAction( typeof( PhotoMainLayoutCache ) )]
        public override void Layout() {
            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );
        }


        private void bindUsers( List<User> userRanks ) {
            IBlock block = getBlock( "users" );
            foreach (User user in userRanks) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

        private void bindPhotos( String blockName, List<PhotoPost> photos ) {
            IBlock block = getBlock( blockName );
            foreach (PhotoPost p in photos) {
                block.Set( "p.ImgThumbUrl", p.ImgThumbUrl );
                block.Set( "p.LinkShow", alink.ToAppData( p ) );
                block.Next();
            }
        }

        [CachePage( typeof( PhotoMainPageCache ) )]
        [CacheAction( typeof( PhotoMainActionCache ) )]
        public void Index() {

            ctx.Page.Title = lang( "photo" );

            bindPicked();

            List<User> userRanks = rankService.GetTop( 9 );
            bindUsers( userRanks );

            List<PhotoSysCategory> categories = categoryService.GetAll();
            IBlock block = getBlock( "categories" );
            foreach (PhotoSysCategory c in categories) {

                List<PhotoPost> photos = photoService.GetSysTop( c.Id, 8 );
                bindOneCategory( block, c, photos );
                block.Next();

            }
        }

        private void bindPicked() {
            List<PhotoPost> pickedlist = PickedService.GetTop( 4 );
            IBlock pickeBlock = getBlock( "picked" );
            foreach (PhotoPost p in pickedlist) {
                pickeBlock.Set( "p.LinkShow", alink.ToAppData( p ) );
                pickeBlock.Set( "p.ImgThumbUrl", p.ImgThumbUrl );
                pickeBlock.Set( "p.ImgUrl", p.ImgUrl );
                pickeBlock.Next();
            }
        }

        private void bindOneCategory( IBlock block, PhotoSysCategory c, List<PhotoPost> photos ) {

            block.Set( "category.Name", c.Name );
            block.Set( "category.LinkShow", to( List, c.Id ) );

            IBlock pblock = block.GetBlock( "photos" );
            foreach (PhotoPost p in photos) {

                if (p.Creator == null) continue;

                pblock.Set( "p.LinkShow", alink.ToAppData( p ) );
                pblock.Set( "p.ImgThumbUrl", p.ImgThumbUrl );
                pblock.Set( "p.CreatorName", p.Creator.Name );
                pblock.Set( "p.CreatorLink", toUser( p.Creator ) );
                pblock.Set( "p.Hits", p.Hits );
                pblock.Next();
            }
        }

        public void List( int categoryId ) {

            set( "appLink", to( Index ) );

            PhotoSysCategory category = categoryService.GetById( categoryId );
            ctx.Page.SetTitle( category.Name, lang( "photo" ) );

            DataPage<PhotoPost> list = photoService.GetSysPostPage( categoryId, 20 );
            bindOneCategory( this.utils.getCurrentView(), category, list.Results );
            set( "page", list.PageBar );
        }

        public void Recent() {

            view( "List" );

            DataPage<PhotoPost> list = photoService.GetSysPostPage( 0, 35 );

            set( "category.Name", "最新图片" );

            IBlock pblock = getBlock( "photos" );
            foreach (PhotoPost p in list.Results) {

                if (p.Creator == null) continue;

                pblock.Set( "p.LinkShow", alink.ToAppData( p ) );
                pblock.Set( "p.ImgThumbUrl", p.ImgThumbUrl );
                pblock.Set( "p.CreatorName", p.Creator.Name );
                pblock.Set( "p.CreatorLink", toUser( p.Creator ) );
                pblock.Set( "p.Hits", p.Hits );
                pblock.Next();
            }

            set( "page", list.PageBar );

            ctx.Page.SetTitle( "最新", lang( "photo" ) );

        }


    }

}
