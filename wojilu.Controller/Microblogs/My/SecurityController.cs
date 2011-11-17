using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Microblogs.My {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {

            if (ctx.viewer.IsLogin == false) {
                echo( "请先登录" );
                return;
            }


            if (ctx.viewer.Id != ctx.owner.Id) {
                echo( "没有权限" );
            }


        }

    }

}
