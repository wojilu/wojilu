/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Data;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Security {

    public class AppRole : CacheObject {

        public int AppId { get; set; }
        public int RoleId { get; set; }
        public String RoleType { get; set; }

        //---------------------------------------------------------------------------------

        public static Boolean IsRoleInApp( int roleId, String roleType, int appId ) {
            IList configAll = new AppRole().findAll();
            foreach (AppRole ac in configAll) {

                if (ac.AppId == appId && ac.RoleId == roleId && 
                    (ac.RoleType != null && ac.RoleType.Equals(roleType))
                    )
                    return true;
            }
            return false;
        }

        public static void DeleteAll() {

            IList allAppRoles = new AppRole().findAll();

            foreach (AppRole ar in allAppRoles) {
                ar.delete();
            }
        }
        //---------------------------------------------------------------------------------

        public static void InitSiteFront( int appId ) {

            AppRole guest = new AppRole();
            guest.AppId = appId;
            guest.RoleId = SiteRole.Guest.Id;
            guest.RoleType = typeof( SiteRole ).FullName;
            guest.insert();

            AppRole normal = new AppRole();
            normal.AppId = appId;
            normal.RoleId = SiteRole.NormalMember.Id;
            normal.RoleType = typeof( SiteRole ).FullName;
            normal.insert();

            AppRole admin = new AppRole();
            admin.AppId = appId;
            admin.RoleId = SiteRole.Administrator.Id;
            admin.RoleType = typeof( SiteRole ).FullName;
            admin.insert();
        }


    }

}
