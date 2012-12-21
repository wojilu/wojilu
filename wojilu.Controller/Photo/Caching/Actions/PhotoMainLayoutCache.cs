using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Photo.Caching {

    public class PhotoMainLayoutCache : ActionCache {

        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_photo_main_layout";
        }

        public override void ObserveActions() {
            Admin.PostController p = new wojilu.Web.Controller.Photo.Admin.PostController();
            observe( p.Create );
            observe( p.SaveUpload );
            observe( p.Update );
        }

        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {

            CacheManager.GetApplicationCache().Remove( this.GetCacheKey( null, null ) );

        }

    }

}
