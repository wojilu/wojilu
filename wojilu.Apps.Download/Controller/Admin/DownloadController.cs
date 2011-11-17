using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Download.Admin {


    [App( typeof( DownloadApp ) )]
    public class DownloadController : ControllerBase {

        public void Index() {
            redirect( new FileController().List );
        }

    }

}
