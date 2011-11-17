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

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogIndexCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            return typeof( User ).FullName + owner.Url + "_blogapp" + appId;
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;


            String key = GetCacheKey( ctx, null );

            String content = getHomeCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static string getHomeCache( int appId, User owner ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( BlogApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.BlogController().Index );

            return content;
        }

    }


}
