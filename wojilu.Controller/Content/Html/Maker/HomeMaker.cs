/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class HomeMaker : HtmlMaker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HomeMaker ) );

        private int appId;

        protected override string GetDir() {
            String staticDir = getAppDirName( appId );
            if (staticDir == "/") {
                return PathHelper.Map( "/" );
            }
            else {
                return PathHelper.Map( "/" + staticDir + "/" );
            }
        }

        public void Process( int appId ) {

            this.appId = appId;
            base.CheckDir();
            String addr = Link.To( Site.Instance, new ContentController().Index, appId );

            String html = makeHtml( addr );
            String htmlPath = getHomePageAbs( appId );
            file.Write( htmlPath, html );
            logger.Info( "make ContentApp html done =>" + htmlPath );
        }

        private string getHomePageAbs( int appId ) {
            return PathHelper.Map( GetAppPath( appId ) );
        }

        private static String getAppDirName( int appId ) {
            ContentApp app = ContentApp.findById( appId );
            if (app == null) throw new Exception( "app not found: Content.AppId=" + appId );
            return HtmlLink.GetStaticDir( app );
        }

        public static String GetAppPath( int appId ) {
            ContentApp app = ContentApp.findById( appId );
            if (app == null) throw new Exception( "app not found: Content.AppId=" + appId );
            return HtmlLink.ToApp( app );
        }


    }

}
