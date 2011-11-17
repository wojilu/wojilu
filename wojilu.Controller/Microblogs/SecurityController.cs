using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Web.Controller.Microblogs {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {

            Boolean isMicroblogClose = Component.IsClose( typeof( MicroblogApp ) );
            if (isMicroblogClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

    }

}
