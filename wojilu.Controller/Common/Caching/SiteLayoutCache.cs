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
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Common.Caching {

    public class SiteLayoutCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            if (!(ctx.owner.obj is Site)) return null;
            if (ctx.route.isAdmin || ctx.route.isUserDataAdmin) return null;

            return getKey();
        }

        private String getKey() {
            return "action_site_layout";
        }

        public override void ObserveActions() {

            AppBaseController ab = new AppBaseController();
            observe( ab.Start );
            observe( ab.Stop );
            observe( ab.Create );
            observe( ab.Update );
            observe( ab.Delete );

            MenuBaseController mb = new MenuBaseController();
            observe( mb.SortMenu );
            observe( mb.Create );
            observe( mb.SaveSubMenu );
            observe( mb.Update );
            observe( mb.Delete );

            SiteConfigController sc = new SiteConfigController();
            observe( sc.BaseSave );
            observe( sc.LogoSave );
            observe( sc.DeleteLogo );

            FooterMenuController fm = new FooterMenuController();
            observe( fm.SaveSort );
            observe( fm.Create );
            observe( fm.Update );
            observe( fm.Delete );

            observe( new wojilu.Web.Controller.Admin.Credits.CreditController().UpdateKeyRule );
            observe( new wojilu.Web.Controller.Admin.Credits.CurrencyController().UpdateKeyCurrency );

            AdController ad = new AdController();
            observe( ad.Create );
            observe( ad.Update );
            observe( ad.Delete );
            observe( ad.Start );
            observe( ad.Stop );

        }

        public override void AfterAction( MvcContext ctx ) {

            if ((ctx.owner.obj is Site) == false) return; // 空间菜单、群组菜单如有更新，不刷新网站缓存

            //String key = getKey();
            //IMember owner = Site.Instance;
            //String content = getLayoutCache( owner );
            //CacheManager.GetApplicationCache().Put( key, content );

            CacheManager.GetApplicationCache().Remove( getKey() );
        }


        //private static string getLayoutCache( IMember owner ) {
        //    MvcContext ctx = MockContext.GetOne( owner );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.LayoutController().Layout );

        //    return content;
        //}

    }


}
