using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Admin;
using wojilu.Web.Controller.Admin;
using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Common.Caching {

    public class SiteLayoutCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {
            return "site_layout";
        }

        public Dictionary<string, string> GetRelatedActions() {
            Dictionary<string, string> dic = new Dictionary<String, String>();
            dic.Add( typeof( MenuBaseController ).FullName, "SortMenu/Create/SaveSubMenu/Update/Delete" );
            dic.Add( typeof( SiteConfigController ).FullName, "BaseSave/LogoSave/DeleteLogo" );
            dic.Add( typeof( FooterMenuController ).FullName, "SaveSort/Create/Update/Delete" );
            return dic;
        }

        public void UpdateCache( MvcContext ctx ) {
            if ((ctx.owner.obj is Site) == false) return; // 空间菜单、群组菜单如有更新，不刷新网站缓存

            String key = GetCacheKey( ctx, null );

            IMember owner = Site.Instance;

            String content = getLayoutCache( owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }


        private static string getLayoutCache( IMember owner ) {
            MvcContext ctx = MockContext.GetOne( owner );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.LayoutController().Layout );

            return content;
        }

    }


}
