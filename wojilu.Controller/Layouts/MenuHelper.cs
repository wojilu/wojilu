using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Menus.Interface;
using wojilu.Web.Context;
using System.Collections;
using wojilu.Web.Url;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Layouts {

    public class MenuHelper {

        public static string getCurrentClass( IMenu menu, Object currentModuleUrl, String currentClass ) {
            return getCurrentClass( menu.RawUrl, currentModuleUrl, currentClass );
        }

        public static string getCurrentClass( String rawUrl, Object currentModuleUrl, String currentClass ) {

            if (currentModuleUrl == null) return "";

            if (currentModuleUrl is String) {

                return getCssClassByString( rawUrl, currentModuleUrl.ToString(), currentClass );

            }
            else if (currentModuleUrl is String[]) {

                return getCssClassByArray( rawUrl, currentModuleUrl as String[], currentClass );
            }
            else {
                return "";
            }
        }

        private static String getCssClassByArray( String rawUrl, String[] arrUrl, String currentClass ) {

            foreach (String cUrl in arrUrl) {
                if (cUrl == null) continue;
                String cssClass = getCssClassByString( rawUrl, cUrl, currentClass );
                if (strUtil.HasText( cssClass )) {
                    return cssClass;
                }
            }
            return "";
        }

        private static String getCssClassByString( String rawUrl, String currentModuleUrl, String currentClass ) {
            String cUrl = strUtil.TrimEnd( currentModuleUrl, MvcConfig.Instance.UrlExt ).TrimStart( '/' );
            return strUtil.EqualsIgnoreCase( cUrl, rawUrl ) ? currentClass : "";
        }

        public static void bindMenuSingle( IBlock block, IMenu menu, MvcContext ctx ) {

            block.Set( "menu.Id", menu.Id );
            block.Set( "menu.Name", menu.Name );
            block.Set( "menu.Style", menu.Style );
            block.Set( "menu.Link", UrlConverter.toMenu( menu, ctx ) );

            String lnkTarget = menu.OpenNewWindow == 1 ? lnkTarget = " target=\"_blank\"" : "";
            block.Set( "menu.LinkTarget", lnkTarget );

            block.Next();
        }

        public static void bindSubMenus( IBlock block, List<IMenu> list, MvcContext ctx ) {

            foreach (IMenu menu in list) {
                bindMenuSingle( block, menu, ctx );
            }
        }

        public static List<IMenu> getSubMenus( IList menus, IMenu menu ) {

            List<IMenu> list = new List<IMenu>();
            if (menu == null) return list;

            foreach (IMenu m in menus) {
                if (m.ParentId == menu.Id) list.Add( m );
            }
            return list;
        }

        public static List<IMenu> getRootMenus( IList menus ) {

            List<IMenu> list = new List<IMenu>();
            foreach (IMenu m in menus) {
                if (m.ParentId == 0) list.Add( m );
            }
            return list;
        }


        //--------------------------------------------------------------------------------------------------

        public static IMenu getCurrentRootMenu( List<IMenu> list, MvcContext ctx ) {

            IMenu m = getCurrentMenuByUrl( list, ctx );

            if (m != null) {

                if (m.ParentId == 0)
                    return m;
                else {
                    return getParentMenu( list, m, ctx );
                }
            }
            else {

                return getRootMenuByAppAndNs( list, ctx );
            }
        }

        //--------------------------------------------

        private static IMenu getCurrentMenuByUrl( List<IMenu> list, MvcContext ctx ) {

            String currentPath = strUtil.TrimEnd( ctx.url.Path, MvcConfig.Instance.UrlExt );
            currentPath = currentPath.TrimStart( '/' );

            Boolean isHomepage = false;
            if (strUtil.IsNullOrEmpty( currentPath )) isHomepage = true;// 在无后缀名的情况下，首页是空""

            foreach (IMenu menu in list) {

                if (isHomepage) {
                    if ("default".Equals( menu.Url )) return menu;
                    continue;
                }

                if (currentPath.Equals( menu.Url ) || currentPath.Equals( menu.RawUrl )) { // 未设置友好网址的url也是空
                    return menu;
                }
            }
            return null;
        }

        private static IMenu getParentMenu( List<IMenu> list, IMenu menu, MvcContext ctx ) {
            foreach (IMenu m in list) {
                if (m.Id == menu.ParentId) return m;
            }
            return null;
        }

        //--------------------------------------------

        private static IMenu getRootMenuByAppAndNs( List<IMenu> list, MvcContext ctx ) {

            IMenu menu = getRootMenuByNs( list, ctx );
            if (menu == null) return null;

            // 如果是app，则还要比较appId
            if (ctx.app.Id > 0) {
                Route menuRoute = RouteTool.RecognizePath( menu.RawUrl );
                if (menuRoute == null) return null;
                if (ctx.app.Id != menuRoute.appId) return null;
            }

            return menu;
        }

        private static IMenu getRootMenuByNs( List<IMenu> list, MvcContext ctx ) {

            // 先找到同一命名空间的
            IMenu menu = getMenuBySameNs( list, ctx );
            if (menu == null) return null;

            // 找到父菜单
            if (menu.ParentId > 0) menu = getParentMenu( list, menu, ctx );
            return menu;
        }

        private static IMenu getMenuBySameNs( List<IMenu> list, MvcContext ctx ) {

            foreach (IMenu m in list) {

                if (m == null || strUtil.IsNullOrEmpty( m.RawUrl )) continue;
                if (m.RawUrl.StartsWith( "http:" )) continue;

                Route rt = RouteTool.RecognizePath( m.RawUrl );
                if (rt == null) return null;

                if (!ctx.route.ns.StartsWith( rt.ns )) continue;

                if (ctx.app.Id <= 0 || ctx.app.Id == rt.appId) return m;

            }
            return null;
        }




    }

}
