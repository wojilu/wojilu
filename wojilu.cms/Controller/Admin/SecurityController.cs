using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web;

namespace wojilu.cms.Controller.Admin {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {

            // 你也可以跳转到登录页面
            string url = to( new LoginController().Login ) + "?returnUrl=" + ctx.url.PathAndQuery;
            if (ctx.web.UserIsLogin == false) echoRedirect( "非法访问，请先登录。", url );
        }
    }

}
