/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Members.Interface;
using wojilu.Members.Groups.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Menus.Interface;

namespace wojilu.Members.Groups {

    public class GroupInitHelper : IInitHelper {

        public Type GetMemberType() {
            return typeof( Group );
        }

        public IMember getOwnerByUrl( MvcContext ctx ) {

            Group group = new GroupService().GetByUrl( ctx.route.owner );
            if (group == null) {
                throw ctx.ex( HttpStatus.NotFound_404, "owner error : " + lang.get( "exGroupNotFound" ) );
            }
            return group;
        }

        public Boolean IsAppRunning( MvcContext ctx ) {
            GroupAppService userAppService = new GroupAppService();
            IMemberApp app = userAppService.GetByApp( (IApp)ctx.app.obj );
            if (app == null || app.IsStop == 1) {
                return false;
            }

            return true;
        }

        public List<IMenu> GetMenus( IMember owner ) {
            GroupMenuService menuService = new GroupMenuService();
            return menuService.GetList( owner );
        }

    }

}
