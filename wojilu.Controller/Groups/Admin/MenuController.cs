/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Service;

namespace wojilu.Web.Controller.Groups.Admin {

    public class MenuController : wojilu.Web.Controller.Common.Admin.MenuBaseController {

        public IAdminLogService<GroupLog> logService { get; set; }

        public MenuController() {
            menuService = new GroupMenuService();
            logService = new GroupLogService();
        }

        public override void log( String msg, IMenu menu ) {
            String dataInfo = "{Id:" + menu.Id + ", Name:'" + menu.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, menu.GetType().FullName, ctx.Ip );
        }

        public override String GetCommonLink() {
            return to( new Groups.Admin.GLinkController().Index );
        }
    }

}
