using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Context;
using wojilu.Web;
using wojilu.Web.Mvc.Routes;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common.Caching;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogLayoutViewCacher {

        public static void Update( User owner, int appId ) {

            String key = GetCacheKey( owner.Url, appId );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }


        public static String GetCacheKey( String userUrl, int appId ) {
            return typeof( User ).FullName + "_" + userUrl + "_app" + appId + "_" + typeof( wojilu.Web.Controller.Blog.LayoutController ).FullName + ".Layout";
        }

        private static String getLayoutCache( int appId, User objOwner ) {

            MvcContext ctx = MockContext.GetOne( objOwner, typeof( BlogApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Blog.LayoutController().Layout );

            return content;
        }





    }

}
