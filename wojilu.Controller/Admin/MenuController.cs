/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;
using wojilu.Common.Menus.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Security;
using System.IO;

namespace wojilu.Web.Controller.Admin {


    public class MenuController : wojilu.Web.Controller.Common.Admin.MenuBaseController {

        public IAdminLogService<SiteLog> logService { get; set; }

        public MenuController() {
            menuService = new SiteMenuService();
            logService = new SiteLogService();
        }

        public override void log( String msg, IMenu menu ) {
            String dataInfo = "{Id:" + menu.Id + ", Name:'" + menu.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, menu.GetType().FullName, ctx.Ip );
        }

        public override String GetCommonLink() {
            return to( new Sys.DashboardController().Links );
        }

        public override void Update( int id ) {

            base.Update( id );

            checkDefaultHtmlPage();
        }

        private void checkDefaultHtmlPage() {

            IMenu menu = ctx.GetItem( "currentMenu" ) as IMenu;
            if (menu == null) return;

            if ("default".Equals( menu.Url ) == false) return;

            if (menu.RawUrl.EndsWith( "/" )) {
                makeDefaultHtml( menu );
            }
            else {
                deleteDefaulHtml( menu );
            }
        }

        private void makeDefaultHtml( IMenu menu ) {
            String staticFile = getStaticPath( menu );

            if (staticFile != null) {
                file.Copy( staticFile, getDefaultPagePath(), true );
            }
        }

        private void deleteDefaulHtml( IMenu menu ) {
            String defaultHtml = getDefaultPagePath();
            if (file.Exists( defaultHtml )) {
                file.Delete( defaultHtml );
            }
        }

        private String getStaticPath( IMenu menu ) {

            if (menu.RawUrl.EndsWith( "/" ) && menu.RawUrl.IndexOf( "http" ) < 0) {
                String staticFile = Path.Combine( PathHelper.Map( menu.RawUrl ), "default.html" );
                if (file.Exists( staticFile )) return staticFile;
            }

            return null;
        }

        private String getDefaultPagePath() {
            return PathHelper.Map( "default.html" );
        }

    }
}

