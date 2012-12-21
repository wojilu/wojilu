using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Caching;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogMainLayoutCache : ActionCache {

        public override String GetCacheKey( MvcContext ctx, String actionName ) {

            return getCacheKey();
        }

        private String getCacheKey() {
            return "__action_main_blog_layout";
        }

        public override void ObserveActions() {

            Admin.MyListController my = new wojilu.Web.Controller.Blog.Admin.MyListController();
            observe( my.Admin );

            Admin.PostController p = new wojilu.Web.Controller.Blog.Admin.PostController();
            observe( p.Create );
            observe( p.Update );

            Admin.DraftController d = new wojilu.Web.Controller.Blog.Admin.DraftController();
            observe( d.PublishDraft );
            observe( d.Admin );

            wojilu.Web.Controller.Admin.Apps.Blog.MainController m = new wojilu.Web.Controller.Admin.Apps.Blog.MainController();
            observe( m.Admin );
            observe( m.Delete );

            wojilu.Web.Controller.Admin.Apps.Blog.TrashController trash = new wojilu.Web.Controller.Admin.Apps.Blog.TrashController();
            observe( trash.UnDelete );

        }

        public override void AfterAction( MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( getCacheKey() );
        }

    }

}
