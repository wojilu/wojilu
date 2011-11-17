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

    public class BlogLayoutCache : IActionCache {

        public string GetCacheKey( MvcContext ctx, string actionName ) {

            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            return typeof( User ).FullName + "_" + owner.Url + "_app" + appId + "_" + typeof( wojilu.Web.Controller.Blog.LayoutController ).FullName + ".Layout";
        }

        public Dictionary<string, string> GetRelatedActions() {
            throw new NotImplementedException();
        }

        public void UpdateCache( MvcContext ctx ) {
            User owner = ctx.owner.obj as User;
            int appId = ctx.app.Id;

            String key = GetCacheKey( ctx, null );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );
        }

        private static String getLayoutCache( int appId, User objOwner ) {

            MvcContext ctx = MockContext.GetOne( objOwner, typeof( BlogApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.LayoutController().Layout );

            return content;
        }

    }

}
