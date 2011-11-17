//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.Members.Users.Domain;
//using wojilu.Web.Context;
//using wojilu.Web.Mvc.Utils;
//using wojilu.Caching;
//using wojilu.Web.Controller.Common.Caching;
//using wojilu.Members.Interface;
//using wojilu.Apps.Content.Domain;

//namespace wojilu.Web.Controller.Content.Caching {

//    public class IndexViewCacher {

//        public static void Update( IMember owner, int appId ) {


//            String key = GetCacheKey( owner, appId );

//            String content = getIndexCache( appId, owner );

//            CacheManager.GetApplicationCache().Put( key, content );

//        }

//        public static String GetCacheKey( IMember owner, int appId ) {
//            return owner.GetType().FullName + "_" + owner.Url + "_" + typeof( wojilu.Web.Controller.Content.ContentController ).FullName + "_" + appId;
//        }

//        private static string getIndexCache( int appId, IMember owner ) {
//            MvcContext ctx = MockContext.GetOne( owner, typeof( ContentApp ), appId );
//            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.Content.ContentController().Index );

//            return content;
//        }

//    }
//}
