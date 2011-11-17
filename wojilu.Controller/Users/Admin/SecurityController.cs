/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public class SecurityController : ControllerBase {
        
        public override void CheckPermission() {
            if (!(ctx.owner.obj is User)) echo( lang( "exSpaceVisitOnly" ) );
        }

    }
}

