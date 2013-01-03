using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;

namespace wojilu.weibo.Controller.Weibo.Admin
{
    public class LayoutController : ControllerBase
    {
        public override void Layout()
        {
            set("userWeiboIndexLink", to(new UserWeiboSettingController().Index));
        }
    }
}
