/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;

namespace wojilu.Web.Controller.Admin {

    public class AdminSecurityUtils {

        //public static Boolean HasSession( MvcContext ctx ) {

        //    return (ctx.web.SessionGet( "adminSite" ) != null);
        //}

        //public static void SetSession( MvcContext ctx ) {
        //    ctx.web.SessionSet( "adminSite", "abc" );
        //}

        //public static void SetSession( IWebContext webContext ) {
        //    webContext.SessionSet( "adminSite", "abc" );
        //}


        //public static void ClearSession( MvcContext ctx ) {
        //    ctx.web.SessionSet( "adminSite", null );
        //}

        public static readonly String adminCookieName = "_wojiluSiteAdmin";

        public static Boolean HasSession( MvcContext ctx ) {

            int adminId = ctx.web.UserId( adminCookieName );
            return adminId>0 && adminId == ctx.viewer.Id;
        }

        public static void SetSession( MvcContext ctx ) {
            ctx.web.UserLogin( adminCookieName,  ctx.viewer.Id, "", wojilu.Common.LoginTime.Never );
        }

        public static void SetSession( IWebContext ctx ) {
            ctx.UserLogin( adminCookieName, ctx.UserId() , "", wojilu.Common.LoginTime.Never );
        }


        public static void ClearSession( MvcContext ctx ) {
            ctx.web.UserLogout( adminCookieName );
        }

        public static String GetAuthCookieJson( MvcContext ctx ) {

            return "{ \"" + ctx.web.CookieAuthName() + "\":\"" + ctx.web.CookieAuthValue() + "\", \"" + SystemInfo.clientSessionID + "\":\"" + ctx.web.SessionId + "\", \"" + adminCookieName + "\":\"" + ctx.web.CookieAuthValue( adminCookieName ) + "\" }";

        }



    }

}
