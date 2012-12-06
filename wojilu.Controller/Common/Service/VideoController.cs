/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Net.Video;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Common.Service {

    public class VideoController : ControllerBase {

        [Login]
        public void PlayUrl() {

            String playUrl = ctx.Get( "playUrl" );
            if (strUtil.IsNullOrEmpty( playUrl )) {
                echoJson( "{}" );
                return;
            }

            WojiluVideoSpider s = new WojiluVideoSpider();
            VideoInfo vi = s.GetInfo( playUrl );

            echoJson( Json.Serialize( vi ) );

        }

    }

}
