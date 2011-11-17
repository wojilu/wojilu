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
using wojilu.Common.Security;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class PermissionFrontController : ControllerBase {

        public ISiteRoleService siteRoleService { get; set; }
        public IMemberAppService appService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public PermissionFrontController() {

            siteRoleService = new SiteRoleService();
            appService = new SiteAppService();
            appService.menuService = new SiteMenuService();
            logService = new SiteLogService();
        }


        public void Index() {

            target( SaveAppRole );

            List<IRole> roles = siteRoleService.GetRoleAndRank();
            IList apps = appService.GetByMember( Site.Instance.Id );

            bindAppList( apps );
            bindRoleList( roles, apps );

        }

        [HttpPost, DbTransaction]
        public void SaveAppRole() {

            String appRoles = ctx.Post( "appRole" );
            AppRole.DeleteAll();

            if (strUtil.HasText( appRoles )) {

                string[] values = appRoles.Split( ',' );
                foreach (String str in values) {

                    if (strUtil.IsNullOrEmpty( str )) continue;
                    string[] arrItem = str.Split( '_' );
                    if (arrItem.Length != 3) continue;

                    int appId = cvt.ToInt( arrItem[0] );
                    int roleId = cvt.ToInt( arrItem[1] );
                    if (appId <= 0 || roleId < 0) continue;

                    String roleType = arrItem[2];

                    AppRole ar = new AppRole();
                    ar.AppId = appId;
                    ar.RoleId = roleId;
                    ar.RoleType = roleType;

                    ar.insert();
                }
            }
            log( SiteLogString.UpdateFrontPermission() );

            echoRedirect( lang( "saved" ) );

        }


    }



}
