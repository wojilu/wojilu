/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Jobs;

namespace wojilu.Common.Jobs {

    public class RefreshServerJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RefreshServerJob ) );

        public void Execute() {

            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute > 0 && DateTime.Now.Minute <= 20) {
                logger.Info( "clear all cache" );
                sys.Clear.ClearAll();
            }

            String url = sys.Url.SchemeStr + SystemInfo.Authority;
            url = strUtil.Join( url, "refresh.aspx" );
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
                request.UserAgent = typeof( RefreshServerJob ).FullName;
                request.GetResponse();
                logger.Info( "refresh server done : " + url );
            }
            catch (Exception ex) {
                logger.Error( "RefreshServer=>" + url + Environment.NewLine + ex.ToString() );
            }

        }

        public void End() {
        }

    }

}
