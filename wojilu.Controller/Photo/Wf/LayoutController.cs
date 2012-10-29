using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;

namespace wojilu.Web.Controller.Photo.Wf {

    public class LayoutController : ControllerBase {

        public IPhotoSysCategoryService sysCategoryService { get; set; }

        public LayoutController() {
            sysCategoryService = new PhotoSysCategoryService();
        }

        public override void Layout() {

            set( "lnkNew", PhotoLink.ToHome() );

            bindCategories();
        }

        private void bindCategories() {
            List<PhotoSysCategory> categories = sysCategoryService.GetAll();
            IBlock cblock = getBlock( "categories" );
            foreach (PhotoSysCategory x in categories) {

                cblock.Set( "x.Name", x.Name );
                //cblock.Set( "x.LinkShow", to( List, x.Id ) );
                cblock.Next();
            }
        }

    }

}
