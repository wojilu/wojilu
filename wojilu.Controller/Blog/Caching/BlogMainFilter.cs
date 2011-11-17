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

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogMainCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {
            return typeof( BlogApp ).FullName + "_main_" + actionName;
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {
            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;
            String key = GetCacheKey( ctx, null );

            String content = getMainCache( appId );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static string getMainCache( int appId ) {
            MvcContext ctx = MockContext.GetOne( Site.Instance, typeof( BlogApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.MainController().Index );

            return content;
        }

    }


}
