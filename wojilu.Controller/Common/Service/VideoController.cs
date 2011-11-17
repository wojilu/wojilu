using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Net.Video;
using wojilu.Serialization;
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

            echoJson( JsonString.Convert( vi ) );

        }

    }

}
