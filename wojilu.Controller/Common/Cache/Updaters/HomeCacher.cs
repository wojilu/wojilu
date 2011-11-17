using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;

namespace wojilu.Apps.Blog.Caching {

    public class HomeCacher {


        public static void Update( User owner, int appId ) {


            String key = GetCacheKey( owner.Url, appId );

            String content = getHomeCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( String userUrl, int appId ) {
            return typeof( User ).FullName + userUrl + "_blogapp" + appId;
        }

        private static string getHomeCache( int appId, User owner ) {
            MvcContext ctx = MockContext.GetOne( owner, appId );
            String content = ControllerRunner.Run( "wojilu.Web.Controller.Blog.BlogController", "Index", ctx );

            return content;
        }

    }
}
