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
using System.IO;
using wojilu.Common;
using wojilu.Web.Mvc;
using wojilu.Web.Mock;

namespace wojilu.Web {

    /// <summary>
    /// web 原始数据和方法的模拟，可用于桌面环境
    /// </summary>
    public class MockWebContext : IWebContext, IMockContext {

        private MvcHttpContext _httpctx;

        public static MockWebContext New( String url, StringWriter sw ) {
            return New( 0, null, url, sw );
        }

        public static MockWebContext New( int userId, String url, StringWriter sw ) {
            return New( userId, null, url, sw );
        }

        public static MockWebContext New( int userId, String httpMethod, String url, StringWriter sw ) {


            // 构造request/response/httpContext
            MvcRequest req = new MvcRequest( url );
            req.HttpMethod = strUtil.IsNullOrEmpty( httpMethod ) ? "GET" : httpMethod;

            MvcResponse res = new MvcResponse();
            res.Writer = sw;

            MvcHttpContext ctx = new MvcHttpContext();
            ctx.Request = req;
            ctx.Response = res;

            // 同时构造静态context
            CurrentRequest.setRequest( req );

            MockWebContext mctx = new MockWebContext( ctx );
            mctx.setUserId( userId );

            return mctx;
        }

        public MockWebContext( MvcHttpContext httpContext ) {
            _httpctx = httpContext;
        }

        public String PathApplication { get { return SystemInfo.ApplicationPath; } }
        public String PathHost { get { return SystemInfo.Host; } }
        public String PathRoot { get { return SystemInfo.RootPath; } }

        /*******************************************************************/

        public object Context { get { return _httpctx; } }

        private MvcHttpContext HttpContext { get { return _httpctx; } }
        public object Session { get { return _httpctx.Session; } }
        private MvcRequest Request { get { return _httpctx.Request; } }
        private MvcResponse Response { get { return _httpctx.Response; } }
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

        public void ResponseContentType( String status ) {

        }

        public void ResponseEnd() {
            this.Response.End();
        }

        public void ResponseFlush() {
            this.Response.Flush();
        }

        public Boolean UserIsLogin { get { return UserId() > 0; } }
        public void UserLogout() { }
        public void UserLogout( String cookieName ) { }

        public int UserId() {
            return _userId;
        }

        public int UserId( String cookieName ) { return -1; }

        public void UserLogin( int userId, String userName, LoginTime expiration ) { }
        public void UserLogin( String cookieName, int userId, String userName, LoginTime expiration ) { }
        public void UserLogin( int userId, String userName, DateTime expiration ) { }
        public void UserLogin( String cookieName, int userId, String userName, DateTime expiration ) { }

        public void setUserId( int userId ) {
            _userId = userId;
        }

        private int _userId;

        /*******************************************************************/


        public void CompleteRequest() {
            //_httpctx.ApplicationInstance.CompleteRequest();
        }

        public String UrlEncode( String path ) {
            return HttpUtility.UrlEncode( path );
        }

        public String UrlDecode( String path ) {
            return HttpUtility.UrlDecode( path );
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
            this.Response.ContentType = "text/json";
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
            get { return ""; }
        }


        /****************************** cookie *************************************/

        private UrlInfo getUrl() {
            return new UrlInfo( this.Url, this.PathApplication, this.PathInfo );
        }

        private String COOKIEKEY() {
            return strUtil.TrimStart( getUrl().SiteAndAppPath.ToLower(), "http://" ).Replace( ".", "" ).Replace( "/", "" ) + "Main";
        }

        public void CookieClear() {
            HttpCookie cookie = this.Request.Cookies.Get( COOKIEKEY() );
            if (cookie != null) {
                cookie.Expires = DateTime.Now.AddDays( -1.0 );
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
            return "{}";
        }

        public String GetAuthJson( String cookieName ) {
            return "{}";
        }

        public String GetAuthJson( String[] arrCookieName ) {
            return "{}";
        }

    }

}
