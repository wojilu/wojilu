/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;

using wojilu.Common.AppInstall;
using wojilu.Common.Menus;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;

using wojilu.Web.Controller.Security;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Layouts {

    public partial class SiteLayoutController : ControllerBase {


        private void bindFooterMenus( List<FooterMenu> menus ) {

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < menus.Count; i++) {

                if (i > 0) builder.Append( " | " );

                FooterMenu menu = menus[i] as FooterMenu;
                builder.Append( "<a href=\"" );

                if (PathHelper.IsFullUrl( menu.Link ))
                    builder.Append( menu.Link );
                else
                    builder.Append( strUtil.Join( sys.Path.Root, menu.Link ) );

                builder.Append( "\">" );
                builder.Append( menu.Name );
                builder.Append( "</a>" );
            }

            set( "footerMenus", builder );


        }

        private void bindAdminMenus( List<SiteAdminMenu> menus, List<SiteAdminOperation> userActions, IList apps, List<AppInstaller> userDataApps ) {

            StringBuilder builder = new StringBuilder();

            foreach (SiteAdminMenu menu in menus) {

                // 检查是否有权限显示
                IList actionsByMenu = SiteAdminOperation.GetOperationsByMenu( userActions, menu.Id );
                if (actionsByMenu.Count == 0 && !menu.IsUserDataAdmin()) continue;

                if (strUtil.IsNullOrEmpty( menu.Url ) || menu.Url.Equals( "#" )) {

                    if (menu.IsUserDataAdmin()) {
                        String imgMenu = getLogo( "userdata" );
                        builder.AppendFormat( getUserDataAppList( userDataApps ), imgMenu, menu.Name );
                    } else
                        builder.AppendFormat( "<a href='#'>{0}</a> ", menu.Name );
                } else {
                    String link = strUtil.Join( sys.Path.Root, menu.Url ) + MvcConfig.Instance.UrlExt;
                    String imgMenu = getLogo( menu.Logo );
                    builder.AppendFormat( "<li><a href=\"{0}\" class='frmLink' loadto='adminMainBody' nolayout=1><div>{1}</div><div>{2}</div></a></li>", link, imgMenu, menu.Name );
                }

            }

            set( "menus", builder );
        }

        private String getLogo( String logoName ) {
            String url = string.Format( strUtil.Join( sys.Path.Img, "/admin/m/{0}.png" ), logoName );
            return "<img src=\"" + url + "\" />";
        }

        private String getDownImg() {
            return string.Format( "<img src=\"{0}\"/>", strUtil.Join( sys.Path.Img, "downWhite.gif" ) );
        }

        private String getAppList( IList apps ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "<li id=\"appAdminItem\"><div>{0}</div><div><span id=\"appAdmin\" class=\"menuMore\" list=\"appAdminMenus\">{1} " + getDownImg() + "</span></div></li>" );
            //builder.AppendFormat( " <img src=\"{0}\"/></span></li>", strUtil.Join( sys.Path.Img, "down.gif" ) );
            builder.Append( "<ul id=\"appAdminMenus\" class=\"menuItems\" style=\"display: none; \">" );
            foreach (IMemberApp app in apps) {

                if (!AppAdminRole.IsRoleInApp( ((User)ctx.viewer.obj).RoleId, app.Id )) continue;

                builder.Append( "<li>" );
                builder.Append( getSiteNameAndUrl( app ) );
                builder.Append( "</li>" );
            }

            builder.Append( "</ul>" );
            return builder.ToString();
        }

        private String getUserDataAppList( List<AppInstaller> apps ) {
            StringBuilder builder = new StringBuilder();
            builder.Append( "<li id=\"userDataAdminItem\"><div>{0}</div><div><span id=\"userDataAdmin\" list=\"userDataAdminMenus\" class=\"menuMore left10 right10\">{1} " + getDownImg() + "" );

            builder.Append( "<ul id=\"userDataAdminMenus\" class=\"menuItems\" style=\"display: none; width:100px;\">" );
            foreach (AppInstaller app in apps) {

                if (!UserDataRole.IsRoleInApp( ((User)ctx.viewer.obj).RoleId, app.Id )) continue;

                builder.Append( "<li>" );
                builder.Append( getUserDataAdminLink( app ) );
                builder.Append( "</li>" );
            }

            builder.Append( "</ul></span></div></li>" );
            return builder.ToString();
        }

        private String getUserDataAdminLink( AppInstaller app ) {
            return string.Format( "<a href='{1}' class='frmLink userDataLink' loadto='adminMainBody' nolayout=1>{0}</a>", app.Name, ToUserDataAdmin( app ) );
        }

        private static String ToUserDataAdmin( AppInstaller appInfo ) {
            // /bv/Admin/Apps/Blog/Main/Index.aspx
            String controller = "Admin/Apps/" + strUtil.TrimEnd( appInfo.TypeName, "App" ) + "/Main/Index";
            String result = strUtil.Join( sys.Path.Root, controller );
            return result + MvcConfig.Instance.UrlExt;
        }

        private String getSiteNameAndUrl( IMemberApp app ) {
            if (app.IsStop == 1) {
                return ("<span class='stop'>" + app.Name + "</span>");
            }
            return string.Format( "<a href='{1}'>{0}</a>", app.Name, alink.ToAppAdmin( Site.Instance, app ) );
        }



        //-------------------------------------------------------------


        private void bindLeftNav( IList apps ) {
            bindAppNavList( apps );

            List<SiteAdminOperation> actionOfUser = SiteAdminOperationConfig.Instance.GetActionsByUser( (User)ctx.viewer.obj );
            List<SiteDataAdminMenu> allAction = OperationDB.GetInstance().SiteDataAdminMenus;

            bindAppAndMenu( actionOfUser );

            bindSiteDataAdminMenus( allAction, actionOfUser );

            set( "cacheAdminLink", to( new CacheController().Index ) );
        }

        private void bindAppAndMenu( List<SiteAdminOperation> actionOfUser ) {
            String strHide = "display:none";

            // 是否显示app管理
            set( "lnkAppList", to( new Admin.AppController().Index ) );
            set( "lnkAppAdd", to( new Admin.AppController().Select ) );

            String appHide = "";
            if (OperationDB.GetMenu( 1 ).CanShow( actionOfUser ) == false) {
                appHide = strHide;
            }
            set( "appHide", appHide );

            // 是否显示菜单管理
            set( "lnkMenuList", to( new Admin.MenuController().Index ) );
            set( "lnkMenuAdd", to( new Admin.MenuController().AddMenu ) );

            String menuHide = "";
            if (OperationDB.GetMenu( 2 ).CanShow( actionOfUser ) == false) {
                menuHide = strHide;
            }
            set( "menuHide", menuHide );

            // 是否显示App和组件管理
            set( "lnkAppConfig", to( new Admin.AppConfigController().App ) );

            String appConfigHide = "";
            if (OperationDB.GetMenu( 4 ).CanShow( actionOfUser ) == false) {
                appConfigHide = strHide;
            }
            set( "appConfigHide", appConfigHide );
        }

        private void bindAppNavList( IList apps ) {
            IBlock block = getBlock( "apps" );
            foreach (IMemberApp app in apps) {

                if (AppAdminRole.CanAppAdmin( ctx.viewer.obj, app.Id ) == false) continue;

                block.Set( "app.NameAndUrl", getLeftSiteNameAndUrl( app ) );
                block.Next();
            }
        }


        private void bindSiteDataAdminMenus( List<SiteDataAdminMenu> allAction, List<SiteAdminOperation> actionOfUser ) {
            IBlock block = getBlock( "siteDataAdmin" );
            foreach (SiteDataAdminMenu m in allAction) {

                if (m.CanShow( actionOfUser ) == false) continue;

                String link = strUtil.Join( sys.Path.Root, m.Url ) + MvcConfig.Instance.UrlExt;

                block.Set( "m.Name", m.Name );
                block.Set( "m.Link", link );
                block.Set( "m.Logo", m.Logo );

                block.Next();
            }
        }

        private String getLeftSiteNameAndUrl( IMemberApp app ) {

            String iconPath = strUtil.Join( sys.Path.Img, "app/m/" );
            iconPath = strUtil.Join( iconPath, app.AppInfo.TypeFullName ) + ".png";
            String icon = string.Format( "<img src=\"{0}\" />", iconPath );

            if (app.IsStop == 1) {
                return ("<span class='stop'>" + icon + " " + app.Name + "</span>");
            }
            return string.Format( "<a href='{1}'>{2} {0}</a>", app.Name, alink.ToAppAdmin( Site.Instance, app ), icon );
        }
    }

}
