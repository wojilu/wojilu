using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Forum.Utils {

    public class LinkUtil {

        public static String appendListPage( String url, MvcContext ctx ) {
            //if (ctx.route.page > 1) url = url + "?lp=" + ctx.route.page;
            return url;
        }

        public static String appendListPageToTopic( String url, MvcContext ctx ) {
            //if (ctx.GetInt( "lp" ) > 0) return url = url + "?lp=" + ctx.GetInt( "lp" );
            return url;
        }

        public static String appendListPageToBoard( String url, MvcContext ctx ) {
            //if ( ctx.GetInt( "lp" ) > 0) url = appendPage( url, ctx.GetInt( "lp" ) );
            return url;
        }

        //private static string appendPage( string url, int page ) {
        //    String urlNoExt = strUtil.TrimEnd( url, MvcConfig.Instance.UrlExt );
        //    return strUtil.Join( urlNoExt, "p" + page ) + MvcConfig.Instance.UrlExt;
        //}

    }

}
