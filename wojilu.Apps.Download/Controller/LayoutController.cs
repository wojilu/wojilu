using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Download.Admin;

namespace wojilu.Web.Controller.Download {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            DownloadApp app = ctx.app.obj as DownloadApp;

            set( "lnkFiles", to( new Admin.FileController().List ) );
            set( "lnkAdd", to( new Admin.FileController().Add ) );

            set( "lnkCateAdmin", to( new Admin.CategoryController().List ) );
            set( "lnkSubCate", to( new Admin.SubCategoryController().List ) );
            set( "lnkLicense", to( new Admin.LicenseController().List ) );
            set( "lnkPlatform", to( new Admin.PlatformController().List ) );
            set( "lnkLang", to( new Admin.LangController().List ) );


            set( "adminCheckUrl", t2( new SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( DownloadApp ).FullName );


            // 当前app/module所有页面，所属的首页
            String[] moduleUrlList = new String[1];
            moduleUrlList[0] = to( new DownloadController().Index );

            ctx.SetItem( "_moduleUrl", moduleUrlList );

        }
    }
}
