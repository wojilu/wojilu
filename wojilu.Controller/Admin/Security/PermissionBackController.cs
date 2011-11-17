/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Web.Controller.Security;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppInstall;
using wojilu.Common.Security.Utils;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class PermissionBackController : ControllerBase {

        public ISiteRoleService SiteRoleService { get; set; }
        public IMemberAppService appService { get; set; }
        public IAppInstallerService appInfoService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public PermissionBackController() {

            SiteRoleService = new SiteRoleService();
            appInfoService = new AppInstallerService();

            appService = new SiteAppService();
            appService.menuService = new SiteMenuService();
            logService = new SiteLogService();
        }

        public void Index() {

            load( "generalAdmin", PermissionAdmin );
            load( "siteAppAdmin", AdminIndex );
            load( "userDataAdmin", UserDataAdmin );            
        }

        //---------------------------------------------------------

        public void PermissionAdmin() {
            target( SavePermissionAdminAll );

            SecurityTool tool = getSecurityTool();
            IList sysRoles = tool.GetRoles();
            bindRoleActions( sysRoles, tool );
        }

        [HttpPost, DbTransaction]
        public void SavePermissionAdminAll() {

            string[] actionIds = ctx.web.postValuesByKey( typeof( SiteAdminOperation ).Name );
            getSecurityTool().SaveActionAll( actionIds );
            log( SiteLogString.UpdateAdminPermission(), typeof( SiteAdminOperation ) );
            echoRedirect( lang( "saved" ) );
        }

        //---------------------------------------------------------

        public void AdminIndex() {

            target( SaveAppAdminRole );

            List<SiteRole> roles = SiteRoleService.GetAdminRoles();
            IList apps = appService.GetByMember( Site.Instance.Id );

            bindAppList( apps );
            bindRoleList( roles, apps );
        }


        [HttpPost, DbTransaction]
        public void SaveAppAdminRole() {

            String appRoles = ctx.Post( "appRole" );
            AppAdminRole.DeleteAll();

            if (strUtil.HasText( appRoles )) {

                string[] values = appRoles.Split( ',' );
                foreach (String str in values) {

                    if (strUtil.IsNullOrEmpty( str )) continue;
                    string[] arrItem = str.Split( '_' );
                    if (arrItem.Length != 2) continue;

                    int appId = cvt.ToInt( arrItem[0] );
                    int roleId = cvt.ToInt( arrItem[1] );
                    if (appId <= 0 || roleId <= 0) continue;

                    AppAdminRole ar = new AppAdminRole();
                    ar.AppId = appId;
                    ar.RoleId = roleId;
                    ar.insert();
                }
            }

            log( SiteLogString.UpdateAppAdminPermission(), typeof( AppAdminRole ) );

            echoRedirect( lang( "saved" ) );
        }

        //---------------------------------------------------------

        public void UserDataAdmin() {

            target( SaveSecuritySetting );

            List<SiteRole> roles = SiteRoleService.GetAdminRoles();
            List<AppInstaller> apps = appInfoService.GetUserDataAdmin();

            bindUaAppList( apps );
            bindUaRoleList( roles, apps );
        }

        [HttpPost, DbTransaction]
        public void SaveSecuritySetting() {

            String appRoles = ctx.Post( "appRole" );
            UserDataRole.DeleteAll();

            if (strUtil.HasText( appRoles )) {

                string[] values = appRoles.Split( ',' );
                foreach (String str in values) {

                    if (strUtil.IsNullOrEmpty( str )) continue;
                    string[] arrItem = str.Split( '_' );
                    if (arrItem.Length != 2) continue;

                    int appId = cvt.ToInt( arrItem[0] );
                    int roleId = cvt.ToInt( arrItem[1] );
                    if (appId <= 0 || roleId <= 0) continue;

                    UserDataRole ar = new UserDataRole();
                    ar.AppInfoId = appId;
                    ar.RoleId = roleId;
                    ar.insert();
                }
            }

            log( SiteLogString.UpdateUserDataPermission(), typeof( UserDataRole ) );

            echoRedirect( lang( "saved" ) );
        }

    }

}
