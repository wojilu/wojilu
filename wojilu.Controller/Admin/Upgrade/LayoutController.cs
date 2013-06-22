using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class LayoutController : ControllerBase {


        public override void Layout() {

            set( "lnkUpdateTable", to( new StartController().UpdateTable ) );

            set( "lnkCms", to( new ContentUpdateController().Index ) );
            set( "lnkComment", to( new CommentImportController().Index ) );
            set( "lnkPhoto", to( new PhotoController().Index ) );

        }

    }

}
