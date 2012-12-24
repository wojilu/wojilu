using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc;
using System.IO;

namespace wojilu.Web.Controller.Content.Caching {

    public class HomeMaker : HtmlMakerBase {

        public HomeMaker( MvcContext ctx )
            : base( ctx ) {
        }

        private int appId;

        protected override string GetDir() {
            String staticDir = GetlAppDirName( appId );
            return PathHelper.Map( "/" + staticDir + "/" );
        }

        public void Process( int appId ) {

            this.appId = appId;
            base.CheckDir();
            String addr = strUtil.Join( siteUrl, _ctx.link.To( new ContentController().Index ) );

            String html = makeHtml( addr );
            file.Write( getHomePageAbs(), html );
        }


        public static String GetlAppDirName( int appId ) {
            ContentApp app = ContentApp.findById( appId );
            if (app == null) throw new Exception( "app not found: Content.AppId=" + appId );
            return HtmlLink.GetStaticDir( app );
        }

        private string getHomePageAbs() {
            return Path.Combine( GetDir(), "default.html" );
        }

    }

}
