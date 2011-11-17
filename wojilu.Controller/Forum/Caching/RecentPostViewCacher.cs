using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Members.Interface;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum.Caching {

    public class RecentPostViewCacher {

        public static void Update( IMember owner, int appId ) {

            String key = GetCacheKey( owner, appId );

            String content = getIndexCache( owner, appId );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.RecentController ).FullName + "_Post_app" + appId;
        }

        private static string getIndexCache( IMember owner, int appId ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Forum.RecentController().Post );

            return content;
        }

    }

}
