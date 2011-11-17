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

namespace wojilu.Apps.Blog.Caching {

    public class LayoutCacher {

        public static void Update( User owner, int appId ) {

            String key = GetCacheKey( owner.Url, appId );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }


        public static String GetCacheKey( String userUrl, int appId ) {
            return typeof( User ).FullName + "_" + userUrl + "_app" + appId + "_" + "wojilu.Web.Controller.Blog.LayoutController.Layout";
        }

        private static String getLayoutCache( int appId, User objOwner ) {

            MvcContext ctx = MockContext.GetOne( objOwner, appId );
            String content = ControllerRunner.Run( "wojilu.Web.Controller.Blog.LayoutController", "Layout", ctx );

            return content;
        }





    }

}
