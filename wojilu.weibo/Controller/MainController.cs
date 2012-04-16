using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.weibo.Controller.Weibo;

namespace wojilu.weibo.Controller.Weibo
{
   public class MainController : ControllerBase
    {
       public void Index()
       {
           redirect(new UserWeiboSettingController().Index);
       }
    }
}
