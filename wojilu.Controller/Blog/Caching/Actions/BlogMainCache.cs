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

        public override string GetCacheKey( MvcContext ctx, String actionName ) {

            return "__action_blog_main_index";
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
            observe( m.UnDelete );
        }

        public override void UpdateCache( MvcContext ctx ) {

            CacheManager.GetApplicationCache().Remove( GetCacheKey( null, null ) );

            //User owner = ctx.owner.obj as User;
            //int appId = ctx.app.Id;
            //String key = GetCacheKey( ctx, null );

            //String content = getMainCache( appId );

            //CacheManager.GetApplicationCache().Put( key, content );
        }

        //private static string getMainCache( int appId ) {
        //    MvcContext ctx = MockContext.GetOne( Site.Instance, typeof( BlogApp ), appId );
        //    String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.MainController().Index );

        //    return content;
        //}

    }


}
