/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Web.Controller.Photo.Vo;
using wojilu.Apps.Photo.Interface;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Photo {


    [App( typeof( PhotoApp ) )]
    public class PhotoController : ControllerBase {

        public IPhotoPostService postService { get; set; }
        public IPhotoAlbumService albumService { get; set; }

        public PhotoController() {
            postService = new PhotoPostService();
            albumService = new PhotoAlbumService();
        }

        public void Index() {
            ctx.Page.Title = ctx.app.Name;
            List<PhotoAlbum> albumList = albumService.GetListByApp( ctx.app.Id );
            bindAlbumList( albumList );
        }


        public void Album( int id ) {
            bindPhotoPosts( id );
        }

        public void ViewBig( int id ) {
            bindPhotoPosts( id );
        }

        public void Slider( int id ) {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );
            HideLayout( typeof( wojilu.Web.Controller.Photo.LayoutController ) );

            bindPhotoPosts( id );

            set( "user.Name", ctx.owner.obj.Name );
            set( "user.Url", Link.ToMember( ctx.owner.obj ) );
            set( "siteLink", ctx.url.SiteAndAppPath );
            set( "statsJs", config.Instance.Site.GetStatsJs() );
        }

        //---------------------------------------------------------------------------------------------------

        private void bindAlbumList( List<PhotoAlbum> albumList ) {
            IBlock block = getBlock( "list" );
            foreach (PhotoAlbum album in albumList) {

                block.Set( "album.Name", album.Name );
                block.Set( "album.Link", to( Album, album.Id ) );

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

        private void bindPhotoPosts( int id ) {
            PhotoAlbum album = albumService.GetByIdWithDefault( id, ctx.owner.Id );

            String albumName = "";
            if (album != null) {
                albumName = album.Name;
            }

            ctx.Page.Title = albumName;
            set( "appLink", to( Index ) );
            set( "album.Name", albumName );

            DataPage<PhotoPost> postPage = postService.GetPostPageByAlbum( ctx.owner.Id, ctx.app.Id, id, 12 );
            List<PostVo> volist = PostVo.Fill( postPage.Results );
            bindList( "list", "data", volist );
            set( "page", postPage.PageBar );

            set( "viewNormalLink", to( Album, id ) );
            set( "viewBigLink", to( ViewBig, id ) );
            set( "viewSliderLink", to( Slider, id ) );
        }

    }
}

