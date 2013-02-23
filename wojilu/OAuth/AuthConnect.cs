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
using wojilu.Web;
using wojilu.Net;

namespace wojilu.OAuth {


    public abstract class AuthConnect {

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public abstract string AuthorizationUrl { get; }
        public abstract string AccessTokenUrl { get; }
        public abstract string UserProfileUrl { get; }

        public abstract string CallbackUrl { get; set; }

        public virtual string Scope { get; set; }

        // 一般情况下用 POST 就可以了。少数网站，比如QQ登录要求用GET，请覆盖本属性。
        public virtual String HttpMethod_AccessToken {
            get { return HttpMethod.Post; } 
            set { }
        }


        public abstract OAuthUserProfile GetUserProfile( AccessToken accessToken );

        public virtual String GetAuthorizationFullUrl() {

            UriBuilder ub = new UriBuilder( this.AuthorizationUrl );

            QueryParams queryItems = new QueryParams();

            queryItems.Add( "client_id", this.ConsumerKey );
            queryItems.Add( "redirect_uri", this.CallbackUrl );
            queryItems.Add( "response_type", "code" );

            queryItems.Add( "scope", this.Scope );

            return ub.ToString() + "?" + queryItems.ToEncodedString();
        }

        public virtual String GetUid( AccessToken accessToken ) {
            return accessToken.Uid;
        }

    }
}
