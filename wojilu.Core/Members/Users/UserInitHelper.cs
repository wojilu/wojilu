using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Members.Users {

    public class UserInitHelper : IInitHelper {

        public Type GetMemberType() {
            return typeof( User );
        }

        public IMember getOwnerByUrl( MvcContext ctx ) {

            if (strUtil.EqualsIgnoreCase( ctx.route.owner, ctx.viewer.obj.Url )) {
                return ctx.viewer.obj;
            }

            UserService userService = new UserService();
            User member = userService.GetByUrl( ctx.route.owner );

            return member;
        }

        public Boolean IsAppRunning( MvcContext ctx ) {

            UserAppService userAppService = new UserAppService();
            IMemberApp app = userAppService.GetByApp( (IApp)ctx.app.obj );

            if (app == null || app.IsStop == 1) {
                return false;
            }

            return true;
        }

        public List<IMenu> GetMenus( IMember owner ) {
            UserMenuService menuService = new UserMenuService();
            return menuService.GetList( owner );
        }

    }
}
