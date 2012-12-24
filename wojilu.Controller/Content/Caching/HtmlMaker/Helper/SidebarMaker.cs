using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using System.IO;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching {

    public class SidebarMaker : HtmlMakerBase {

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
            file.Write( getPath( appId ), html );
        }

        private string getPath( int appId ) {
            return Path.Combine( GetDir(), appId + ".html" );
        }


    }

}
