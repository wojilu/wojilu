using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Common.Menus.Interface;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Common {

    public class CacheRemoveBase {

        protected MyCacher cacheHelper = new MyCacher();
        protected IMember owner;
        protected ControllerBase controller;

        protected void removeCacheList( List<String> urls ) {
            foreach (String url in urls) {
                removeCacheSingle( url );
            }
        }

        protected void removeCacheSingle( String url ) {

            cacheHelper.DeleteCache( url );

            String menuFriendUrl = getMenuFriendUrl( url, this.owner );
            if (menuFriendUrl != null) cacheHelper.DeleteCache( url );

        }

        private static String getMenuFriendUrl( String url, IMember owner ) {

            // 检查网站菜单中的部分
            SiteMenuService menuService = new SiteMenuService();
            List<IMenu> menus = menuService.GetList( owner );
            foreach (IMenu menu in menus) {
                if (strUtil.IsNullOrEmpty( menu.RawUrl )) continue;
                if (strUtil.IsNullOrEmpty( menu.Url )) continue;
                if (menu.RawUrl.Equals( strUtil.TrimEnd( url.TrimStart( '/' ), MvcConfig.Instance.UrlExt ) )) {
                    String key = "/" + menu.Url + MvcConfig.Instance.UrlExt; // 缓存中的key是完整的网址
                    return key;
                }
            }

            return null;
        }

    }

}
