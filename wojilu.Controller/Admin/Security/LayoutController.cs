/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Security {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            set( "roleLink", to( new SecurityController().Index ) );


            set( "frontPermission", to( new PermissionFrontController().Index ) );
            set( "backPermission", to( new PermissionBackController().Index ) );

            //set( "resetRank", to( new SecurityController().ResetRank ) );

        }

    }

}
