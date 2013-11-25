/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Data;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Common.MemberApp.Interface;
using wojilu.DI;
using System.Collections.Generic;
using wojilu.Common;

namespace wojilu.Web.Controller.Security {

    public class AppAdminRole : CacheObject {


        public long AppId { get; set; }
        public long RoleId { get; set; }

        //---------------------------------------------------------------------------------

        public static bool CanAppAdmin(IUser user, long appId) {
            long roleId = ((User)user).RoleId;
            return IsRoleInApp( roleId, appId );
        }

        public static bool CanAppAdmin(IUser user, Type appType, long appInstanceId) {

            long roleId = ((User)user).RoleId;

            IMemberAppService siteAppService = new SiteAppService();
            IMemberApp app = siteAppService.GetByApp( appType, appInstanceId );

            return IsRoleInApp( roleId, app.Id );
        }

        public static bool CanAppAdmin(IUser user, IMember owner, Type appType, long appInstanceId) {

            long roleId = ((User)user).RoleId;

            IMemberAppService appService = ServiceMap.GetUserAppService( owner.GetType() );
            IMemberApp app = appService.GetByApp( appType, appInstanceId );

            return IsRoleInApp( roleId, app.Id );
        }

        public static bool IsRoleInApp(long roleId, long appId) {


            IList configAll = new AppAdminRole().findAll();
            foreach (AppAdminRole ac in configAll) {

                if (ac.AppId == appId && ac.RoleId == roleId) return true;
            }
            return false;
        }

        public static void DeleteAll() {

            IList allAppRoles = new AppAdminRole().findAll();

            foreach (AppAdminRole ar in allAppRoles) {
                ar.delete();
            }
        }
        //---------------------------------------------------------------------------------


        public static void InitSiteAdmin( long appId ) {

            AppAdminRole admin = new AppAdminRole();
            admin.AppId = appId;
            admin.RoleId = SiteRole.Administrator.Id;
            admin.insert();
        }

    }

}
