/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Sites.Service;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Admin {

    public class AppController : wojilu.Web.Controller.Common.Admin.AppBaseController {

        public IAdminLogService<SiteLog> logService { get; set; }

        public AppController()
            : base() {

            userAppService = new SiteAppService();
            menuService = new SiteMenuService();
            logService = new SiteLogService();
        }

        public override void log( String msg, IMemberApp app ) {
            String dataInfo = "{Id:" + app.Id + ", Name:'" + app.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, app.GetType().FullName, ctx.Ip );
        }

    }

}
