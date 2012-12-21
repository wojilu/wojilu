using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Caching;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogMainCache : ActionCache {

        // 设计这个缓存的 key
        public override string GetCacheKey( MvcContext ctx, String actionName ) {

            return "__action_blog_main_index";
        }

        // 让这个缓存监控(observe)其他action，一旦其他action发生操作，则自动更新缓存
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

        // 当被监控(observe)的action执行之后，这个 UpdateCache 也会被执行
        public override void AfterAction( MvcContext ctx ) {
            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );
        }

    }


}
