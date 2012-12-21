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
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogIndexCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            if (actionName != "Index") return null;
            if (ctx.route.page > 1) return null; // 只缓存第一页

            return getCacheKey( ctx );
        }

        private String getCacheKey( MvcContext ctx ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            return "__action_"+ typeof( User ).FullName + "_" + owner.Url + "_blogapp" + appId;

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


            Admin.SettingController s = new wojilu.Web.Controller.Blog.Admin.SettingController();
            observe( s.Save );
        }

        public override void AfterAction( MvcContext ctx ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;


            String key = getCacheKey( ctx );

            CacheManager.GetApplicationCache().Remove( key );

            //String content = getHomeCache( appId, owner );

            //CacheManager.GetApplicationCache().Put( key, content );
        }

        //private static String getHomeCache( int appId, User owner ) {
        //    MvcContext ctx = MockContext.GetOne( owner, typeof( BlogApp ), appId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.BlogController().Index );

        //    return content;
        //}

    }


}
