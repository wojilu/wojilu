/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Web.Security;
using System.Web;
using wojilu.Common;

namespace wojilu.Members.Users.Utils {

    public class LoginUtil {


        public static void Login( int userId, String userName, LoginTime loginTime ) {

            Boolean isPersistent = true;
            if (loginTime == LoginTime.Never) {
                isPersistent = false;
            }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket( 1, userName, DateTime.Now, getExpiration( loginTime ), isPersistent, userId.ToString(), FormsAuthentication.FormsCookiePath );
            String str = FormsAuthentication.Encrypt( ticket );
            HttpCookie cookie = new HttpCookie( FormsAuthentication.FormsCookieName, str );
            if (ticket.IsPersistent) {
                cookie.Expires = ticket.Expiration;
            }
            HttpContext.Current.Response.Cookies.Add( cookie );
        }

        private static DateTime getExpiration( LoginTime loginTime ) {
            if (loginTime != LoginTime.Never) {
                if (loginTime == LoginTime.Forever) {
                    return DateTime.Now.AddYears( 100 );
                }
                if (loginTime == LoginTime.OneYear) {
                    return DateTime.Now.AddYears( 1 );
                }
                if (loginTime == LoginTime.OneMonth) {
                    return DateTime.Now.AddMonths( 1 );
                }
                if (loginTime == LoginTime.OneWeek) {
                    return DateTime.Now.AddDays( 7.0 );
                }
            }
            return DateTime.Now;
        }

    }

}
