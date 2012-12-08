/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Web.Mvc;
using wojilu.Common;

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

            echoJson( map );

        }


    }

}
