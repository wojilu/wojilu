/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Web.Controller.Security;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppInstall;
using wojilu.Common.Security.Utils;
using wojilu.Common.Security;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class PermissionBackController : ControllerBase {

        private void log( String msg, Type t ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", t.FullName, ctx.Ip );
        }

        private void bindRoleActions( IList sysRoles, SecurityTool tool ) {

            IBlock sblock = getBlock( "sysroles" );
            foreach (IRole role in sysRoles) {
                String actions = tool.GetActionStringAll( role );
                sblock.Set( "role.Name", role.Name );
                sblock.Set( "actions", actions );
                sblock.Next();
            }
        }

        private static SecurityTool getSecurityTool() {
            SecurityTool tool = new SecurityTool( SiteAdminOperationConfig.Instance, new SiteAdminOperation(), new SiteRoleService().GetAdminRoles() );
            return tool;
        }


        private void bindAppList( IList apps ) {
            IBlock applist = getBlock( "applist" );
            foreach (IMemberApp app in apps) {
                applist.Set( "app.Name", app.Name );
                applist.Next();
            }
        }

        private void bindRoleList( List<SiteRole> roles, IList apps ) {
            IBlock roleBlock = getBlock( "roles" );
            foreach (SiteRole role in roles) {
                roleBlock.Set( "role.Name", role.Name );

                IBlock appBlock = roleBlock.GetBlock( "apps" );

                foreach (IMemberApp app in apps) {

                    String strChecked = AppAdminRole.IsRoleInApp( role.Id, app.Id ) ? "checked=\"checked\"" : "";
                    appBlock.Set( "checkedString", strChecked );
                    appBlock.Set( "app.Id", app.Id );
                    appBlock.Set( "role.Id", role.Id );

                    appBlock.Next();
                }

                roleBlock.Next();

            }
        }


        private void bindUaAppList( List<AppInstaller> apps ) {
            IBlock applist = getBlock( "applist" );
            foreach (AppInstaller app in apps) {
                applist.Set( "app.Name", app.Name );
                applist.Next();
            }
        }

        private void bindUaRoleList( List<SiteRole> roles, List<AppInstaller> apps ) {
            IBlock roleBlock = getBlock( "roles" );
            foreach (SiteRole role in roles) {

                roleBlock.Set( "role.Name", role.Name );

                IBlock appBlock = roleBlock.GetBlock( "apps" );

                foreach (AppInstaller app in apps) {
                    String strChecked = UserDataRole.IsRoleInApp( role.Id, app.Id ) ? "checked=\"checked\"" : "";
                    appBlock.Set( "checkedString", strChecked );
                    appBlock.Set( "app.Id", app.Id );
                    appBlock.Set( "role.Id", role.Id );
                    appBlock.Next();
                }

                roleBlock.Next();

            }
        }


    }

}
