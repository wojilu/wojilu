/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Groups {

    public class LayoutController : ControllerBase {


        public override void Layout() {
            set( "groupAdminHome", to( new GroupController().Index ) );
            set( "postLink", to( new GroupController().PostAdmin ) );
            set( "groupLink", to( new GroupController().GroupAdmin, -1 ) );
            set( "groupCategoryLink", to( new CategoryController().List ) );
        }

    }

}
