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

            set( "roleLink", Link.To( new SecurityController().Index ) );


            set( "frontPermission", Link.To( new PermissionFrontController().Index ) );
            set( "backPermission", Link.To( new PermissionBackController().Index ) );

            //set( "resetRank", to( new SecurityController().ResetRank ) );

        }

    }

}
