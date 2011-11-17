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
using wojilu.Members.Interface;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching {


    public class ContentLayoutViewCacher {

        public static void Update( IMember owner, int appId ) {

            String key = GetCacheKey( owner, appId );

            String content = getLayoutCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }


        public static String GetCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.Section.LayoutController ).FullName + ".Layout" + "_app" + appId;
        }

        private static String getLayoutCache( int appId, IMember owner ) {

            MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.Section.LayoutController().Layout );

            return content;
        }





    }

}
