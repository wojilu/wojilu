using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Download.Admin {

    public class LayoutController : ControllerBase {

        public LayoutController() {
            HideLayout( typeof( wojilu.Web.Controller.Download.LayoutController ) );
        }

        public override void Layout() {


            set( "fileLink", to( new FileController().List ) );
            set( "viewByCategoryLink", to( new SubCategoryController().Files ) );

            set( "categoryLink", to( new CategoryController().List ) );
            set( "subCategoryLink", to( new SubCategoryController().List ) );
            set( "licenseLink", to( new LicenseController().List ) );
            set( "platformLink", to( new PlatformController().List ) );
            set( "langLink", to( new LangController().List ) );
        }

    }

}
