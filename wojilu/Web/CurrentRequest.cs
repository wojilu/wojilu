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
using System.Web;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using wojilu.Web.Mvc;
using wojilu.Web.Mock;

namespace wojilu.Web {


    /// <summary>
    /// 当前请求范围中的数据，方便静态方法调用
    /// </summary>
    public class CurrentRequest {

        public static object getItem( string key ) {
            if (SystemInfo.IsWeb) {
                return HttpContext.Current.Items[key];
            }
            else {
                return Thread.GetData( Thread.GetNamedDataSlot( key ) );
            }
        }

        public static void setItem( String key, object obj ) {
            if (SystemInfo.IsWeb) {
                HttpContext.Current.Items[key] = obj;
            }
            else {
                Thread.SetData( Thread.GetNamedDataSlot( key ), obj );
            }
        }

        //public IDictionary items;
        //private String rawUrl;
        //public String[] userLanguages;
        //public String httpMethod;
        //public NameValueCollection form;

        public static void setRequest( MvcRequest req ) {
            if (req == null) {
                setItem( "_RawUrl", null );
                setItem( "_UserLanguages", null );
                setItem( "_HttpMethod", null );
                setItem( "_Form", null );
            }
            else {
                setItem( "_RawUrl", req.RawUrl );
                setItem( "_UserLanguages", req.UserLanguages );
                setItem( "_HttpMethod", req.HttpMethod );
                setItem( "_Form", req.Form );
            }
        }

        public static String getRawUrl() {
            if (getItem( "_RawUrl" ) != null) return getItem( "_RawUrl" ).ToString();
            if (HttpContext.Current == null) return "";
            return HttpContext.Current.Request.RawUrl;
        }

        public static String[] getUserLanguages() {
            if (getItem( "_UserLanguages" ) != null) return (String[])getItem( "_UserLanguages" );
            if (HttpContext.Current == null) return new String[] { };
            return HttpContext.Current.Request.UserLanguages;
        }

        public static String getHttpMethod() {
            if (getItem( "_HttpMethod" ) != null) return getItem( "_HttpMethod" ).ToString();
            if (HttpContext.Current == null) return "";
            return HttpContext.Current.Request.HttpMethod;
        }

        public static String getForm( String key ) {
            if (getItem( "_Form" ) != null) return ((NameValueCollection)getItem( "_Form" ))[key];
            if (HttpContext.Current == null) return "";
            return HttpContext.Current.Request.Form[key];
        }

        public static String getCookieKey() {
            return "wojiluCookie";
        }

        public static String getLangCookie() {
            return CookieGet( "lang" );
        }

        private static String CookieGet( String key ) {
            if (HttpContext.Current == null) return "";
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get( getCookieKey() );
            if (cookie == null) return "";
            return cookie[key];
        }

        public static void setCurrentPage( int current ) {
            setItem( "pageNumber", current );
        }

        public static int getCurrentPage( ) {
            return cvt.ToInt( getItem( "pageNumber" ) );
        }


    }

}
