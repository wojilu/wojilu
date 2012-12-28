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
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace wojilu.Web.Mock {

    /// <summary>
    /// request 的模拟
    /// </summary>
    public class MvcRequest {

        public MvcRequest() {
            this.QueryString = new NameValueCollection();
            init();
        }

        public MvcRequest( String url ) {

            if (url == null) throw new ArgumentNullException( "MvcRequest" );
            if (url.StartsWith( "http" ) == false) url = strUtil.Join( sys.Url.SiteUrl, url );

            this.Url = new Uri( url );
            this.QueryString = getQueryString( this.Url );
            init();
        }

        private static NameValueCollection getQueryString( Uri url ) {

            NameValueCollection nv = new NameValueCollection();

            if( url == null ) return nv;
            String query = url.Query;
            if (strUtil.IsNullOrEmpty( query )) return nv;

            String[] arr = query.TrimStart( '?' ).Split( '&' );
            foreach (String item in arr) {

                if (strUtil.IsNullOrEmpty( item )) continue;

                String[] pair = item.Split( '=' );
                if (pair.Length != 2) continue;

                nv.Add( pair[0].Trim(), pair[1].Trim() );
            }

            return nv;
        }

        private void init() {
            this.HttpMethod = "GET";
            this.UserLanguages = new String[] { };
            this.Form = new NameValueCollection();
            this.Params = merge( this.Form, this.QueryString );
            this.ServerVariables = new NameValueCollection();
            this.Cookies = new MvcCookies();

            this.UserHostAddress = "0.0.0.0";
            this.UserAgent = "mock wojilu agent";
        }


        private NameValueCollection merge( NameValueCollection postList, NameValueCollection getList ) {
            NameValueCollection list = new NameValueCollection();
            list.Add( postList );
            list.Add( getList );
            return list;
        }

        public NameValueCollection Form { get; set; }
        public NameValueCollection QueryString { get; set; }
        public NameValueCollection ServerVariables { get; set; }
        public NameValueCollection Params { get; set; }
        public HttpFileCollection Files { get; set; } //TODO
        public MvcCookies Cookies { get; set; }

        public String HttpMethod { get; set; }
        public Uri Url { get; set; }
        public String PathInfo { get; set; }
        public Uri UrlReferrer { get; set; }
        public String UserHostAddress { get; set; }
        public String UserAgent { get; set; }
        public String[] UserLanguages { get; set; }
        public String RawUrl { get; set; }

    }

}
