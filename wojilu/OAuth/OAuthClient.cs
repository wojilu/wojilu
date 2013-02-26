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
using System.IO;
using System.Net;
using System.Text;
using wojilu.Web;
using wojilu.Net;
using wojilu.DI;

namespace wojilu.OAuth {

    public class OAuthClient : HttpClient {

        public static new OAuthClient New() {
            return ObjectContext.Create<OAuthClient>();
        }

        public static OAuthClient Init( String apiUrl, String accessToken, String httpMethod ) {

            if (strUtil.IsNullOrEmpty( apiUrl )) throw new ArgumentNullException( "api url" );
            if (strUtil.IsNullOrEmpty( accessToken )) throw new ArgumentNullException( "accessToken" );

            OAuthClient x = New();
            x.SetUrl( apiUrl );
            x.SetMethod( httpMethod );
            x.SetAccessToken( accessToken );
            x.SetUserAgent( string.Concat( 'w', 'o', 'j', 'i', 'l', 'u', ' ', 'o', 'a', 'u', 't', 'h', ' ', 'c', 'l', 'i', 'e', 'n', 't' ) );
            return x;
        }

        public IOAuthHelper oauthHelper { get; set; }

        private String _accessToken;

        public OAuthClient() {
            oauthHelper = new OAuthHelper();
        }

        public virtual OAuthClient SetAccessToken( String accessToken ) {
            _accessToken = accessToken;
            _parameters.Add( "access_token", accessToken );
            _headers.Add( "Authorization", "OAuth2 " + accessToken );
            return this;
        }

        public virtual AccessToken GetAccessToken( AuthConnect connect, String code ) {
            return oauthHelper.GetAccessToken( connect, code );
        }

        public virtual AccessToken GetAccessToken( AuthConnect connect, String code, String httpMethod ) {
            return oauthHelper.GetAccessToken( connect, code, httpMethod );
        }
    }

}
