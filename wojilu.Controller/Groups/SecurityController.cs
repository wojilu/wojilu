using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common;

namespace wojilu.Web.Controller.Groups {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {

            if (Component.IsEnableGroup() == false) {
                echo( "对不起，群组功能已停用" );
            }
        }


    }
}
