using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Poll.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Apps.Poll {

    [App( typeof( PollApp ) )]
    public class MainController : ControllerBase {

        public void Index() {
            content( "TODO...建设中" );
        }

    }

}
