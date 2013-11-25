/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Data;
using wojilu.Common.AppInstall;

namespace wojilu.Web.Controller.Security {

    public class UserDataRole : CacheObject {


        public int AppInfoId { get; set; }
        public int RoleId { get; set; }

        //---------------------------------------------------------------------------------


        public static Boolean IsRoleInApp( long roleId, String appName ) {
            long appId = getAppInfoId( appName );
            return IsRoleInApp( roleId, appId );
        }

        private static long getAppInfoId(string appName) {
            String appType = strUtil.Append( appName, "App" );
            AppInstaller appInfo = new AppInstallerService().GetByTypeName( appType );
            if (appInfo != null) return appInfo.Id;
            return -1;
        }


        public static bool IsRoleInApp(long roleId, long appInfoId) {


            IList configAll = new UserDataRole().findAll();
            foreach (UserDataRole ac in configAll) {

                if (ac.AppInfoId == appInfoId && ac.RoleId == roleId) return true;
            }
            return false;
        }

        public static void DeleteAll() {

            IList allAppRoles = new UserDataRole().findAll();

            foreach (UserDataRole ar in allAppRoles) {
                ar.delete();
            }
        }

    }

}
