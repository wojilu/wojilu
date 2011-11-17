/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace wojilu.Web.Handler {

    public class RefreshServerHandler : IHttpHandler {

        public Boolean IsReusable {
            get { return true; }
        }

        public void ProcessRequest( HttpContext context ) {
            context.Response.Write( "wsojyilyu" );
            context.ApplicationInstance.CompleteRequest();
        }

    }
}
