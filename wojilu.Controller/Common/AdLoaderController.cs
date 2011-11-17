using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Common {

    public class AdLoaderController : ControllerBase {

        public void Index() {

            String ads = ctx.Post( "adItems" );

            if (strUtil.IsNullOrEmpty( ads )) {
                echoError( "无效的ad" );
                return;
            }

            Dictionary<String, String> map = new Dictionary<string, string>();
            String[] arr = ads.Split( ',' );
            foreach (String item in arr) {
                map[item] = AdItem.GetAdByName( item );
            }

            echoJson( JsonString.ConvertDictionary( map ) );

        }


    }

}
