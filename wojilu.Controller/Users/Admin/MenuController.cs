/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Common.Menus.Interface;
using wojilu.Common;

namespace wojilu.Web.Controller.Users.Admin {

    public class MenuController : wojilu.Web.Controller.Common.Admin.MenuBaseController {

        public IAdminLogService<UserLog> logService { get; set; }

        public MenuController() {
            menuService = new UserMenuService();
            logService = new UserLogService();
        }

        public override void CheckPermission() {

            Boolean isUserMenuAdmin = Component.IsClose( typeof( UserMenuAdmin ) );
            if (isUserMenuAdmin) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public override void log( String msg, IMenu menu ) {
            String dataInfo = "{Id:" + menu.Id + ", Name:'" + menu.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, menu.GetType().FullName, ctx.Ip );
        }

        public override String GetCommonLink() {
            return to( new Users.Admin.MyLinkController().Index );
        }

    }

}
