/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Controller.Security;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Admin {

    public class MainController : ControllerBase {

        public IUserService userService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public MainController() {


            HidePermission( typeof( wojilu.Web.Controller.SecurityController ) );
            HidePermission( typeof( wojilu.Web.Controller.Admin.SecurityController ) );

            userService = new UserService();
            logService = new SiteLogService();
        }

        public void Login() {

            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );

            target( CheckLogin );

            String returnUrl = ctx.web.UrlDecode( ctx.Get( "returnUrl" ) );
            set( "returnUrl", returnUrl );
        }

        public void Welcome() {

            set( "dateTime", DateTime.Now.ToString( "D" ) + ", " +DateTime.Now.ToString( "dddd" )   );

            String dashboardUrl = ctx.link.T2( new Sys.DashboardController().Index );

            if (SiteAdminService.HasAdminPermission( ctx.viewer.obj, dashboardUrl )) {

                redirectUrl( dashboardUrl );
            }

        }


        [HttpPost, DbTransaction]
        public void CheckLogin() {

            if( SiteRole.IsInAdminGroup( ctx.viewer.obj.RoleId) ==false ) {
                echoRedirect( lang( "exNoPermission" ) );
                return;
            }

            String name = ctx.Post( "Name" );
            String pwd = ctx.Post( "Password1" );

            if ( ctx.viewer.obj.Name.Equals(name)==false || userService.IsNamePwdCorrect( name, pwd ) == null) {
                errors.Add( lang( "exUserNamePwdError" ) );
            }

            if (ctx.HasErrors) {
                logService.Add( (User)ctx.viewer.obj, SiteLogString.LoginError(), ctx.Ip, SiteLogCategory.Login );
                run( Login );
                return;
            }

            AdminSecurityUtils.SetSession( ctx );
            logService.Add( (User)ctx.viewer.obj, SiteLogString.LoginOk(), ctx.Ip, SiteLogCategory.Login );

            String returnUrl = ctx.Post( "returnUrl" );
            returnUrl = returnUrl.Replace( "&amp;", "&" );
            if (strUtil.IsNullOrEmpty( returnUrl )) {
                //redirectUrl( t2( new DashboardController().Index ) );
                redirectUrl( t2( Welcome ) );
            }
            else {
                redirectUrl( returnUrl );
            }

        }

        public void Logout() {
            AdminSecurityUtils.ClearSession( ctx );
            logService.Add( (User)ctx.viewer.obj, SiteLogString.Logout(), ctx.Ip, SiteLogCategory.Login );
            echoRedirect( lang( "logoutok" ), ctx.url.SiteAndAppPath );
        }



    }
}
