using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Sites {

    public class SiteInitHelper : IInitHelper {

        public Type GetMemberType() {
            return typeof( Site );
        }

        public IMember getOwnerByUrl( MvcContext ctx ) {
            return Site.Instance;
        }

        public bool IsAppRunning( MvcContext ctx ) {
            SiteAppService userAppService = new SiteAppService();
            IMemberApp app = userAppService.GetByApp( (IApp)ctx.app.obj );
            if (app == null || app.IsStop == 1) {
                return false;
            }

            return true;
        }

        public List<IMenu> GetMenus( IMember owner ) {
            SiteMenuService menuService = new SiteMenuService();
            return menuService.GetList( owner );
        }

    }
}
