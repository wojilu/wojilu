using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.weibo.Domain;

namespace wojilu.weibo.Controller.Weibo.Admin
{
    public class WeiboController : ControllerBase
    {
        public void Index()
        {
            redirect(new WeiboTypeController().List);
        }

    }
}
