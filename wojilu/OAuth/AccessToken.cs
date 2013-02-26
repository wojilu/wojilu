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
using wojilu.Serialization;

namespace wojilu.OAuth {

    public class AccessToken {

        public AccessToken() {
        }

        public AccessToken( String jsonString ) {

            JsonObject obj = Json.ParseJson( jsonString );

            if (obj == null || obj.Count == 0) return;

            _jsonValue = obj;

            this.Token = obj.Get( "access_token" );
            this.RefreshToken = obj.Get( "refresh_token" );
            this.ExpiresIn = obj.Get<int>( "expires_in" );
            this.Scope = obj.Get( "scope" );

            String _uid = obj.Get( "uid" );
            if (_uid == null) _uid = obj.Get( "name" );

            this.Uid = _uid;
        }

        /// <summary>
        /// 用户在第三方平台的唯一ID标识
        /// </summary>
        public String Uid { get; set; }

        /// <summary>
        /// 用户在第三方平台的名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// access token
        /// </summary>
        public String Token { get; set; }

        public int ExpiresIn { get; set; }

        public String RefreshToken { get; set; }

        public String Scope { get; set; }


        private JsonObject _jsonValue;

        public String GetString( String key ) {

            return _jsonValue.Get( key );
        }

        public void SetValue( string key, string value ) {

            if (key == "access_token") {
                this.Token = value;
                return;
            }

            if (key == "expires_in") {
                this.ExpiresIn = cvt.ToInt( value );
                return;
            }

            if (key == "refresh_token") {
                this.RefreshToken = value;
                return;
            }

            if (key == "name") {
                this.Uid = value;
                return;
            }

        }


    }

}
