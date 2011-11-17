/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Admin {

    public class SecurityController : ControllerBase {
        
        public override void CheckPermission() {
            if (!(ctx.owner.obj is Site)) echo( lang( "exOnlySitePermission" ) );
        }

    }
}

