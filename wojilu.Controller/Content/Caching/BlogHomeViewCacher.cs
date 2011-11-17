using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Content.Caching {

    public class ContentHomeViewCacher {

        public static void Update( IMember owner, int appId ) {


            String key = GetCacheKey( owner, appId );

            String content = getHomeCache( appId, owner );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( IMember owner, int appId ) {
            return owner.GetType().FullName + "_" + owner.Url + "_contentApp" + appId;
        }

        private static string getHomeCache( int appId, IMember owner ) {
            MvcContext ctx = MockContext.GetOne( owner, appId );
            String content = ControllerRunner.Run( new wojilu.Web.Controller.Content.ContentController().Index, ctx );

            return content;
        }

    }
}
