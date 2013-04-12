/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using System.Web;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.SessionState;
using wojilu.Common;
using wojilu.Web.Mvc;

namespace wojilu.Web {

    /// <summary>
    /// web 原始数据和方法
    /// </summary>
    public class WebContext : IWebContext {

        private HttpContext _httpctx;

        public WebContext( HttpContext httpContext ) {
            _httpctx = httpContext;
        }

        public String PathApplication { get { return SystemInfo.ApplicationPath; } }
        public String PathHost { get { return SystemInfo.Host; } }
        public String PathRoot { get { return SystemInfo.RootPath; } }

        /*******************************************************************/

        public object Context { get { return _httpctx; } }

        private HttpContext HttpContext { get { return _httpctx; } }
        public object Session { get { return _httpctx.Session; } }
        private HttpRequest Request { get { return _httpctx.Request; } }
        private HttpResponse Response { get { return _httpctx.Response; } }
        private NameValueCollection Form { get { return this.Request.Form; } }
        private IDictionary Items { get { return _httpctx.Items; } }

        /*******************************************************************/


        public String ClientHttpMethod { get { return this.Request.HttpMethod; } }
        public Uri Url { get { return this.Request.Url; } }
        public String PathInfo { get { return this.Request.PathInfo; } }
        public String PathReferrer {
            get {
                try {
                    // this.Request.ServerVariables["Http_Referer"]
                    return this.Request.UrlReferrer == null ? null : this.Request.UrlReferrer.ToString();
                }
                catch {
                    return null;
                }
            }
        }
        public String PathReferrerHost {
            get {
                try {
                    return this.Request.UrlReferrer == null ? null : this.Request.UrlReferrer.Host;
                }
                catch {
                    return null;
                }
            }
        }

        public String ClientIp { get { return this.Request.UserHostAddress; } }
        public String ClientAgent { get { return this.Request.UserAgent; } }
        public String[] ClientLanguages { get { return this.Request.UserLanguages; } }
        public String PathAbsolute { get { return this.Request.Url.AbsolutePath; } }

        public String PathRawUrl { get { return this.Request.RawUrl; } }

        /***************************** method **************************************/

        public void ResponseStatus( String status ) {
            this.Response.Status = status;
        }

        public void ResponseContentType( String contentType ) {
            this.Response.ContentType = contentType;
        }

        public void ResponseEnd() {
            this.Response.End();
        }

        public void ResponseFlush() {
            this.Response.Flush();
        }

        public Boolean UserIsLogin { get { return UserId() > 0; } }
        public void UserLogout() { FormsAuthentication.SignOut(); }

        public void UserLogout( String cookieName ) {

            HttpContext current = HttpContext.Current;

            if (current.Request.Browser.Cookies) {
                string str = string.Empty;
                if (current.Request.Browser["supportsEmptyStringInCookieValue"] == "false") {
                    str = "NoCookie";
                }
                HttpCookie cookie = new HttpCookie( cookieName, str );
                cookie.HttpOnly = true;
                cookie.Path = FormsAuthentication.FormsCookiePath;
                cookie.Expires = new DateTime( 1999, 10, 12 );
                cookie.Secure = FormsAuthentication.RequireSSL;
                if (FormsAuthentication.CookieDomain != null) {
                    cookie.Domain = FormsAuthentication.CookieDomain;
                }
                current.Response.Cookies.Remove( cookieName );
                current.Response.Cookies.Add( cookie );
            }
        }

        public int UserId() {
            return UserId( FormsAuthentication.FormsCookieName );
        }

        public int UserId( String cookieName ) {
            FormsAuthenticationTicket ticket = getTicket( cookieName );
            if (ticket == null) return -1;
            return cvt.ToInt( ticket.UserData );
        }

        public void UserLogin( int userId, String userName, DateTime expiration ) {
            UserLogin( FormsAuthentication.FormsCookieName, userId, userName, expiration );
        }
        public void UserLogin( int userId, String userName, LoginTime expiration ) {
            UserLogin( FormsAuthentication.FormsCookieName, userId, userName, expiration );
        }

        public void UserLogin( String cookieName, int userId, String userName, LoginTime expiration ) {
            UserLogin( cookieName, userId, userName, getExpiration( expiration ) );
        }

        public void UserLogin( String cookieName, int userId, String userName, DateTime expiration ) {

            Boolean isPersistent = (expiration.Subtract( DateTime.Now ).Days >= 1) ? true : false;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                2,
                userName,
                DateTime.Now,
                expiration,
                isPersistent,
                userId.ToString(),
                FormsAuthentication.FormsCookiePath
            );

            String str = FormsAuthentication.Encrypt( ticket );
            HttpCookie cookie = new HttpCookie( cookieName, str );
            if (isPersistent) {
                cookie.Expires = ticket.Expiration;
            }

            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            this.Response.Cookies.Add( cookie );
        }

        private FormsAuthenticationTicket getTicket( String cookieName ) {
            FormsAuthenticationTicket ticket;
            String encryptedTicket = getAuthCookieValue( cookieName );
            if (encryptedTicket == null) return null;
            if (encryptedTicket.Length == 0) return null;

            try {
                ticket = FormsAuthentication.Decrypt( encryptedTicket );
            }
            catch {
                return null;
            }
            return ticket;
        }

        private String getAuthCookieValue( String cookieName ) {

            if ("Shockwave Flash" == HttpContext.Current.Request.UserAgent) {
                return HttpContext.Current.Request.Form[cookieName];
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null) {
                return cookie.Value;
            }
            return null;
        }


        private DateTime getExpiration( LoginTime loginTime ) {

            if (loginTime == LoginTime.Forever) return DateTime.Now.AddYears( 50 );
            if (loginTime == LoginTime.OneYear) return DateTime.Now.AddYears( 1 );
            if (loginTime == LoginTime.OneMonth) return DateTime.Now.AddMonths( 1 );
            if (loginTime == LoginTime.OneWeek) return DateTime.Now.AddDays( 7 );

            // 20分钟内如果不活动即失效
            return DateTime.Now.AddMinutes( 20 );
        }


        /*******************************************************************/


        public void CompleteRequest() {
            _httpctx.ApplicationInstance.CompleteRequest();
        }

        public String UrlEncode( String path ) {
            return _httpctx.Server.UrlEncode( path );
        }

        public String UrlDecode( String path ) {
            return _httpctx.Server.UrlDecode( path );
        }

        public void RenderXml( String content ) {
            this.Response.Clear();
            this.Response.ContentType = "text/xml";
            this.Response.Charset = "UTF-8";
            this.Response.Write( content );
            this.Response.End();
        }

        public void RenderJson( String content ) {
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            this.Response.Write( content );
            this.Response.End();
        }

        public HttpFileCollection getUploadFiles() {
            return this.Request.Files;
        }

        public String param( String key ) {
            return this.Request.Params[key];
        }

        public String get( String key ) {
            return this.Request.QueryString[key];
        }

        public String ClientVar( String key ) {
            return this.Request.ServerVariables[key];
        }

        public Boolean getHas( String key ) {
            for (int i = 0; i < this.Request.QueryString.Keys.Count; i++) {
                if (this.Request.QueryString.Keys[i].Equals( key )) return true;
            }
            return false;
        }

        public NameValueCollection postValueAll() {
            return this.Form;
        }

        public String post( String key ) {
            return this.Form[key];
        }

        public Boolean postHas( String key ) {
            for (int i = 0; i < this.Form.Keys.Count; i++) {
                if (this.Form.Keys[i].Equals( key )) return true;
            }
            return false;
        }

        public String[] postValuesByKey( String name ) {
            return this.Form.GetValues( name );
        }

        public void Redirect( String url ) {
            this.Response.Redirect( url );
        }

        public void Redirect( String url, Boolean endResponse ) {
            this.Response.Redirect( url, endResponse );
        }

        public void ResponseWrite( String content ) {
            this.Response.Write( content );
        }

        public void SessionSet( String key, Object val ) {
            _httpctx.Session[key] = val;
        }

        public Object SessionGet( String key ) {
            return _httpctx.Session[key];
        }

        public String SessionId {
            get { return _httpctx.Session.SessionID; }
        }

        /****************************** cookie *************************************/

        private String COOKIEKEY() {
            return CurrentRequest.getCookieKey();
        }

        public void CookieClear() {
            HttpCookie cookie = this.Request.Cookies.Get( COOKIEKEY() );
            if (cookie != null) {
                cookie.Expires = DateTime.Now.AddDays( -1.0 );
                if (FormsAuthentication.CookieDomain != null) {
                    cookie.Domain = FormsAuthentication.CookieDomain;
                }
                cookie.Values.Clear();
                this.Response.Cookies.Add( cookie );
            }
        }

        public String CookieGet( String key ) {
            HttpCookie cookie = this.Request.Cookies.Get( COOKIEKEY() );
            if (cookie == null) {
                return String.Empty;
            }
            return cookie[key];
        }

        public void CookieRemove( String key ) {
            HttpCookie cookie = this.Request.Cookies.Get( COOKIEKEY() );
            if (cookie != null) {

                if (FormsAuthentication.CookieDomain != null) {
                    cookie.Domain = FormsAuthentication.CookieDomain;
                }
                cookie.Values.Remove( key );
                this.Response.Cookies.Add( cookie );
            }
        }

        public void CookieSet( String key, String val ) {
            HttpCookie cookie = this.Request.Cookies.Get( COOKIEKEY() );
            if (cookie == null) {
                cookie = new HttpCookie( COOKIEKEY() );
            }
            cookie[key] = val;

            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            this.Response.Cookies.Add( cookie );
        }

        /**********************************************/

        public String CookieAuthName() {
            return FormsAuthentication.FormsCookieName;
        }

        public String CookieAuthValue() {
            return CookieAuthValue( this.CookieAuthName() );
        }

        public String CookieAuthValue( String cookieName ) {
            HttpCookie c = this.Request.Cookies[cookieName];
            return c != null ? c.Value : null;
        }

        public void CookieSetLang( String val ) {
            this.CookieSet( "lang", val );
        }

        public String GetAuthJson() {
            return GetAuthJson( new String[] { } );
        }

        public String GetAuthJson( String arrCookieName ) {
            return GetAuthJson( new String[] { arrCookieName } );
        }

        /// <summary>
        /// 获取安全验证的cookie字符串(json格式)
        /// </summary>
        /// <param name="arrCookieName">默认cookie名称之外的其他cookie名称，在ctx.web.UserLogin() 第一个参数中指定</param>
        /// <returns></returns>
        public String GetAuthJson( String[] arrCookieName ) {

            String str = "{ \"" + this.CookieAuthName() + "\":\"" + this.CookieAuthValue() + "\", \"" + SystemInfo.clientSessionID + "\":\"" + this.SessionId + "\" ";

            if (arrCookieName == null || arrCookieName.Length == 0) {
                return str + "}";
            }
            else {
                str += ", ";
            }

            foreach (String cookieName in arrCookieName) {
                if (strUtil.IsNullOrEmpty( cookieName )) continue;
                str += "\"" + cookieName + "\":\"" + this.CookieAuthValue( cookieName ) + "\", ";
            }

            return str.Trim().TrimEnd( ',' ) + "}";
        }



    }

}
