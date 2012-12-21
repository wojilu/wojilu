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

    public class BlogLayoutCache : ActionCache {

        public override string GetCacheKey( MvcContext ctx, string actionName ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            return "__action_"+ typeof( User ).FullName + "_" + owner.Url + "_app" + appId + "_" + typeof( wojilu.Web.Controller.Blog.LayoutController ).FullName + ".Layout";
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

            Admin.BlogrollController r = new wojilu.Web.Controller.Blog.Admin.BlogrollController();
            observe( r.Create );
            observe( r.Update );
            observe( r.Delete );

            Admin.CategoryController c = new wojilu.Web.Controller.Blog.Admin.CategoryController();
            observe( c.Create );
            observe( c.Update );
            observe( c.Insert );
            observe( c.Delete );
            observe( c.SaveSort );

            Admin.SettingController s = new wojilu.Web.Controller.Blog.Admin.SettingController();
            observe( s.Save );
        }

        public override void AfterAction( MvcContext ctx ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            String key = GetCacheKey( ctx, null );

            CacheManager.GetApplicationCache().Remove( key );

            //String content = getLayoutCache( appId, owner );

            //CacheManager.GetApplicationCache().Put( key, content );
        }

        //private static String getLayoutCache( int appId, User objOwner ) {

        //    MvcContext ctx = MockContext.GetOne( objOwner, typeof( BlogApp ), appId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.LayoutController().Layout );

        //    return content;
        //}

    }

}
