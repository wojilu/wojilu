/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Users.Admin {

    public class LayoutController : ControllerBase {

        public override void CheckPermission() {

            if (ctx.owner.Id != ctx.viewer.Id) echo( lang( "exNoPermission" ) );
        }

    }

}
