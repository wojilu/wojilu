/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Web.Controller.Security;
using wojilu.Members.Users.Domain;
using wojilu.Common.Security;
using wojilu.Common.MemberApp.Interface;

namespace wojilu.Web.Controller.Admin.Security {

    public partial class PermissionFrontController : ControllerBase {


        private void bindAppList( IList apps ) {
            IBlock applist = getBlock( "applist" );
            foreach (IMemberApp app in apps) {
                applist.Set( "app.Name", app.Name );
                applist.Next();
            }
        }

        private void bindRoleList( List<IRole> roles, IList apps ) {
            IBlock roleBlock = getBlock( "roles" );
            foreach (IRole role in roles) {
                roleBlock.Set( "role.Name", role.Name );

                IBlock appBlock = roleBlock.GetBlock( "apps" );

                foreach (IMemberApp app in apps) {

                    String strChecked = AppRole.IsRoleInApp( role.Role.Id, role.Role.GetType().FullName, app.Id ) ? "checked=\"checked\"" : "";
                    appBlock.Set( "checkedString", strChecked );
                    appBlock.Set( "app.Id", app.Id );
                    appBlock.Set( "role.Id", role.Role.Id );
                    appBlock.Set( "role.Type", role.Role.GetType().FullName );

                    appBlock.Next();
                }

                roleBlock.Next();

            }
        }


        private void log( String msg ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", typeof( AppRole ).FullName, ctx.Ip );
        }


    }



}
