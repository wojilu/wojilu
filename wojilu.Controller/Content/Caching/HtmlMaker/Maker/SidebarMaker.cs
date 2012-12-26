/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.Web.Context;

namespace wojilu.Web.Controller.Content.Caching {

    public class SidebarMaker : HtmlMakerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SidebarMaker ) );

        public SidebarMaker( MvcContext ctx )
            : base( ctx ) {
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/sidebar/" );
        }

        public void Process( int appId ) {

            base.CheckDir();

            String addr = strUtil.Join( siteUrl, _ctx.link.To( new SidebarController().Index ) ) + "?ajax=true";

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
