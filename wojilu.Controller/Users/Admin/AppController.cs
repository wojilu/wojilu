/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common;

namespace wojilu.Web.Controller.Users.Admin {

    public class AppController : wojilu.Web.Controller.Common.Admin.AppBaseController {

        public IAdminLogService<UserLog> logService { get; set; }

        public AppController()
            : base() {

            userAppService = new UserAppService();
            menuService = new UserMenuService();
            logService = new UserLogService();
        }

        public override void CheckPermission() {

            Boolean isUserAppAdmin = Component.IsClose( typeof( UserAppAdmin ) );
            if (isUserAppAdmin) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public override void log( String msg, IMemberApp app ) {
            String dataInfo = "{Id:" + app.Id + ", Name:'" + app.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, app.GetType().FullName, ctx.Ip );
        }

    }

}
