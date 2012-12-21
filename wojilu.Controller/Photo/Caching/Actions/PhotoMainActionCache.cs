using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Photo.Caching {

    public class PhotoMainActionCache : ActionCache {


        public override string GetCacheKey( wojilu.Web.Context.MvcContext ctx, string actionName ) {
            return "__action_photo_main_index";
        }

        public override void ObserveActions() {

            observe( new Admin.MyController().Admin );

            Admin.PostController p = new wojilu.Web.Controller.Photo.Admin.PostController();
            observe( p.Create );
            observe( p.SaveUpload );
            observe( p.Update );

            observe( new wojilu.Web.Controller.Admin.Apps.Photo.MainController().Admin );

            wojilu.Web.Controller.Admin.Apps.Photo.SysCategoryController cat = new wojilu.Web.Controller.Admin.Apps.Photo.SysCategoryController();
            observe( cat.Create );
            observe( cat.SaveSort );
            observe( cat.Update );
            observe( cat.Delete );

        }

        public override void AfterAction( wojilu.Web.Context.MvcContext ctx ) {

            CacheManager.GetApplicationCache().Remove( this.GetCacheKey( null, null ) );

        }


    }

}
