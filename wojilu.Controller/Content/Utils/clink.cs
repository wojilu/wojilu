/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Context;
using wojilu.Web.Controller.Content.Htmls;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Content.Utils {

    /// <summary>
    /// 为 ContentPost 专门定制的 html 结果页面的链接
    /// </summary>
    public class clink {

        public static String toApp( IApp app ) {
            return HtmlLink.ToApp( app );
        }

        public static String toAppData( IAppData data ) {
            return HtmlLink.ToAppData( data );
        }

        public static String toRecent( MvcContext ctx ) {

            if (HtmlHelper.IsMakeHtml( ctx )) {
                return string.Format( "/html/recent/{0}.html", ctx.app.Id );
            }
            else {
                return ctx.link.To( new PostController().Recent );
            }
        }

        //------------------------------------------------------------------------------------------

        public static String toSection( int sectionId, MvcContext ctx ) {

            if (HtmlHelper.IsMakeHtml( ctx )) {
                return toSection( sectionId );
            }
            else {
                return ctx.link.To( new SectionController().Show, sectionId );
            }
        }


        public static String toSection( int sectionId ) {
            return string.Format( "/html/list/{0}.html", sectionId );
        }


        //------------------------------------------------------------------------------------------

        public static String toSidebar( MvcContext ctx ) {

            if (HtmlHelper.IsMakeHtml( ctx )) {
                return string.Format( "/html/sidebar/{0}.html", ctx.app.Id );
            }
            else {
                return ctx.link.To( new SidebarController().Index );
            }
        }


    }

}
