/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Htmls {

    public class SidebarMaker : HtmlMaker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SidebarMaker ) );

        protected override string GetDir() {
            return PathHelper.Map( "/html/sidebar/" );
        }

        public void Process( int appId ) {

            base.CheckDir();

            String addr = Link.To( Site.Instance, new SidebarController().Index, appId ) + "?ajax=true";

            String html = makeHtml( addr );
            String htmlPath = getPath( appId );
            file.Write( htmlPath, html );

            logger.Info( "make html done=>" + htmlPath );
        }

        private string getPath( int appId ) {
            return Path.Combine( GetDir(), appId + ".html" );
        }


    }

}
