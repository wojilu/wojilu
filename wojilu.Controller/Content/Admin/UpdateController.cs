using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class UpdateController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UpdateController ) );


    }

}
