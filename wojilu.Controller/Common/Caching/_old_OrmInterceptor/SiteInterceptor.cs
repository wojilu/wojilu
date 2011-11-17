//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.Members.Interface;
//using wojilu.Members.Sites.Domain;
//using wojilu.Web.Context;
//using wojilu.Web.Mvc.Utils;
//using wojilu.Caching;

//namespace wojilu.Web.Controller.Common.Caching.Sites {


//    public class SiteInterceptor {

//        public static void UpdateSiteLayout() {

//            String key = GetCacheKey(  );

//            IMember owner = Site.Instance;

//            String content = getLayoutCache( owner );

//            CacheManager.GetApplicationCache().Put( key, content );

//        }

//        public static String GetCacheKey( ) {
//            return "site_layout";
//        }

//        private static string getLayoutCache( IMember owner ) {
//            MvcContext ctx = MockContext.GetOne( owner );
//            String content = ControllerRunner.Run( ctx, new wojilu.Web.Controller.LayoutController().Layout );

//            return content;
//        }

//    }


//}
