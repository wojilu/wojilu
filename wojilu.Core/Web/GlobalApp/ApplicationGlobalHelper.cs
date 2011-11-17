/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace wojilu.Web.GlobalApp {

    public class ApplicationGlobalHelper : AppGlobalLogger {
              

        public ApplicationGlobalHelper( object app ) {
            this.app = app as HttpApplication;
        }

        public override void MailError() {

            StringBuilder sb = base.getErrorInfo( app );

            AppErrorJob.Send( sb.ToString() );
        } 


    }

}
