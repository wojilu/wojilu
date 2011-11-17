/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    [App( typeof( PhotoApp ) )]
    public class LayoutController : ControllerBase {

        public ISysPhotoService photoService { get; set; }
        public IPhotoSysCategoryService categoryService { get; set; }

        public LayoutController() {
            photoService = new SysPhotoService();
            categoryService = new PhotoSysCategoryService();
        }

        public override void Layout() {


            set( "listLink", to( new MainController().Index, -1 ) );
            set( "pickedLink", to( new MainController().Picked ) );
            set( "trashLink", to( new MainController().Trash ) );
            set( "commentLink", to( new CommentController().List ) + "?type=" + typeof( PhotoPostComment ).FullName );

            int trashCount = photoService.GetSystemDeleteCount();
            set( "trashCount", trashCount );

            set( "categoryLink", to( new SysCategoryController().List ) );


            IList categories = categoryService.GetAll();
            bindList( "categories", "c", categories, bindLink );

        }

        private void bindLink( IBlock tpl, int id ) {
            tpl.Set( "c.LinkCategory", to( new MainController().Index, id ) );
        }


    }

}
