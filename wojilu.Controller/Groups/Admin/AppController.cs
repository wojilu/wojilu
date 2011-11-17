/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Service;

namespace wojilu.Web.Controller.Groups.Admin {

    public class AppController : wojilu.Web.Controller.Common.Admin.AppBaseController {

        public IAdminLogService<GroupLog> logService { get; set; }

        public AppController()
            : base() {

            userAppService = new GroupAppService();
            menuService = new GroupMenuService();
            logService = new GroupLogService();
        }

        public override void log( String msg, IMemberApp app ) {
            String dataInfo = "{Id:" + app.Id + ", Name:'" + app.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, app.GetType().FullName, ctx.Ip );
        }

    }

}
