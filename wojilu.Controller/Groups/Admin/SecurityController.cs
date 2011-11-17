/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;

namespace wojilu.Web.Controller.Groups.Admin {

    public class SecurityController : ControllerBase {
        
        public override void CheckPermission() {
            if (!(ctx.owner.obj is Group)) echo( lang( "exGroupVisitOnly" ) );
        }

    }
}

