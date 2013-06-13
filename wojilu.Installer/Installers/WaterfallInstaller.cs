/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Drawing;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Controller.Photo;
using wojilu.Apps.Photo.Service;

namespace wojilu.Web.Controller {

    public class WaterfallInstaller : BaseInstaller {

        private static readonly ILog logger = LogManager.GetLogger( typeof( WaterfallInstaller ) );

        public IPhotoPostService postService { get; set; }

        public WaterfallInstaller() {
            postService = new PhotoPostService();
        }

        public void Init( MvcContext ctx, String appName, String fUrl ) {

            base.AddMenu( ctx, appName, PhotoLink.ToHome(), fUrl );

            // 初始化基本效果图片
            addPhotoPosts( ctx );

        }

        private void addPhotoPosts( MvcContext ctx ) {


            User creator = ctx.viewer.obj as User;
            PhotoApp app = getPhotoAppByUser( creator );
            PhotoAlbum album = createUserAlbum( creator, app );

            List<PhotoSysCategory> cats = PhotoSysCategory.findAll();

            for (int i = 0; i < 20; i++) {

                PhotoPost x = new PhotoPost();
                x.AppId = app.Id;
                x.DataUrl = Img.CopyToUploadPath( "/__installer/pic/pic" + i + ".jpg" );
                x.Title = Path.GetFileName( x.DataUrl );

                if (cats.Count > 0) {
                    int idxCat = getCatIndex( cats.Count, i );
                    x.SysCategoryId = cats[idxCat].Id;
                }

                x.PhotoAlbum = album;

                x.Creator = creator;
                x.CreatorUrl = creator.Url;
                x.OwnerId = creator.Id;
                x.OwnerType = creator.GetType().FullName;
                x.OwnerUrl = creator.Url;

                postService.CreatePost( x, app );

            }

        }

        private static PhotoAlbum createUserAlbum( User creator, PhotoApp app ) {
            PhotoAlbum album = new PhotoAlbum();
            album.Name = "我的图片";
            album.OwnerId = creator.Id;
            album.OwnerUrl = creator.Url;
            album.OwnerId = creator.Id;
            album.OwnerUrl = creator.Url;
            album.AppId = app.Id;

            album.insert();
            return album;
        }

        private PhotoApp getPhotoAppByUser( User creator ) {

            PhotoApp app = PhotoApp.find( "OwnerId=" + creator.Id ).first();

            return app;
        }

        private int getCatIndex( int catsCount, int photoId ) {

            if (photoId < catsCount) return photoId;

            int m = photoId % catsCount;
            return m;
        }



    }

}
