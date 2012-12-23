using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using System.IO;

namespace wojilu.Web.Controller.Content.Caching {

    public class SidebarMaker : HtmlMakerBase {

        protected override string GetDir() {
            return PathHelper.Map( "/html/sidebar/" );
        }

        public void Process( MvcContext ctx ) {

            base.CheckDir( ctx.app.Id );

            String addr = strUtil.Join( ctx.url.SiteAndAppPath, ctx.link.To( new SidebarController().Index ) ) + "?ajax=true";

            String html = makeHtml( addr );
            file.Write( getPath( ctx.app.Id ), html );
        }

        private string getPath( int appId ) {
            return Path.Combine( GetDir(), appId + ".html" );
        }


    }

}
