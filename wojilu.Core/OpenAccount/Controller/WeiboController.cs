using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Controller.Weibo
{
    public class WeiboController : ControllerBase
    {
        public void Index()
        {
            //redirect(new UserWeiboSettingController().Index); 
        }
    }
}
