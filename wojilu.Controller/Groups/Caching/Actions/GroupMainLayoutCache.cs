using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Groups.Caching {

    public class GroupMainLayoutCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_group_main_layout";
        }

        public override void ObserveActions() {

            wojilu.Web.Controller.Admin.Groups.CategoryController c = new wojilu.Web.Controller.Admin.Groups.CategoryController();
            observe( c.Create );
            observe( c.Update );
            observe( c.Delete );
            observe( c.SaveSort );
        }

        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );
        }

    }

}
