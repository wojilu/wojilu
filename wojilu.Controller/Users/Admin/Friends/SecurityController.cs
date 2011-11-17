using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Users.Admin.Friends {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {


            Boolean isFriendClose = Component.IsClose( typeof( FriendApp ) );
            if (isFriendClose) {
                echo( "对不起，本功能已经停用" );
            }



        }


    }

}
