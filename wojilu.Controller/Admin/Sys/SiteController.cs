using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Admin.Sys {

    public class SiteController : ControllerBase {

        public SiteController() {
            LayoutControllerType = typeof( SiteConfigController );
        }

        public void BeginRestart() {
            target( Restart );
        }

        [HttpPost]
        public void Restart() {
            HttpRuntime.UnloadAppDomain();
            echoRedirect( lang( "restartDone" ), ctx.url.SiteAndAppPath );
        }

    }

}
