/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller.Admin {


    public class MenuController : wojilu.Web.Controller.Common.Admin.MenuBaseController {

        public IAdminLogService<SiteLog> logService { get; set; }

        public MenuController() {
            menuService = new SiteMenuService();
            logService = new SiteLogService();
        }

        public override void log( String msg, IMenu menu ) {
            String dataInfo = "{Id:" + menu.Id + ", Name:'" + menu.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, menu.GetType().FullName, ctx.Ip );
        }



    }
}

