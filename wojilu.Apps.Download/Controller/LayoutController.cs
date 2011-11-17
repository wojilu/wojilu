using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Download {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            DownloadApp app = ctx.app.obj as DownloadApp;

            set( "adminUrl", to( new Admin.FileController().List ) );
            set( "appUrl", to( new DownloadController().Index ) );

            set( "adminCheckUrl", t2( new SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( DownloadApp ).FullName );

        }
    }
}
