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


    public class RecentViewCacher {

        public static void Update( IMember owner, int appId, String viewName ) {

            String key = GetCacheKey( owner, appId, viewName );

            String content = getIndexCache( owner, appId, viewName );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( IMember owner, int appId, String viewName ) {
            return owner.GetType().FullName + "_" + owner.Url.Replace( "/", "" ) + "_" + typeof( wojilu.Web.Controller.Forum.RecentController ).FullName + "_" + viewName + "_app" + appId;
        }

        private static string getIndexCache( IMember owner, int appId, String viewName ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( ForumApp ), appId );
            String content = ControllerRunner.Run( ctx, typeof( wojilu.Web.Controller.Forum.RecentController ).FullName, viewName );

            return content;
        }


    }




}
