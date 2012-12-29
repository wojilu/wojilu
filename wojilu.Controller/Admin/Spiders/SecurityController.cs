/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Spiders {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {

            if (ctx.viewer.IsAdministrator() == false) {
                echo( "没有权限" );
            }
        }

    }

}
