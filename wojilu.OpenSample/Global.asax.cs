using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using wojilu.Data;

namespace wojilu.OtherSite {
    public class Global : System.Web.HttpApplication {

        protected void Application_Start( object sender, EventArgs e ) {

        }

        protected void Session_Start( object sender, EventArgs e ) {

        }

        protected void Application_BeginRequest( object sender, EventArgs e ) {

        }

        protected void Application_EndRequest( object sender, EventArgs e ) {
            DbContext.closeConnectionAll();
            wojilu.LogManager.Flush();

        }

        protected void Application_AuthenticateRequest( object sender, EventArgs e ) {

        }

        protected void Application_Error( object sender, EventArgs e ) {

        }

        protected void Session_End( object sender, EventArgs e ) {

        }

        protected void Application_End( object sender, EventArgs e ) {

        }
    }
}