using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogHomeViewCacher {


        public static void Update( User owner, int appId ) {


            String key = GetCacheKey( owner.Url, appId );

            String content = getHomeCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( String userUrl, int appId ) {
            return typeof( User ).FullName + userUrl + "_blogapp" + appId;
        }

        private static string getHomeCache( int appId, User owner ) {
            MvcContext ctx = MockContext.GetOne( owner, typeof( BlogApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.BlogController().Index );

            return content;
        }

    }
}
