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
using wojilu.Web;
using System.Net;
using System.IO;
using wojilu.Net;

namespace wojilu.OAuth {



    public class OAuthHelper : IOAuthHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( OAuthHelper ) );

        public virtual AccessToken GetAccessToken( AuthConnect connect, String code ) {
            return GetAccessToken( connect, code, HttpMethod.Post );
        }

        public virtual AccessToken GetAccessToken( AuthConnect connect, String code, String httpMethod ) {

            StringBuilder sb = new StringBuilder();
            sb.Append( connect.AccessTokenUrl );
            sb.AppendFormat( "?client_id={0}", connect.ConsumerKey );
            sb.AppendFormat( "&client_secret={0}", connect.ConsumerSecret );
            sb.AppendFormat( "&code={0}", code );
            sb.AppendFormat( "&redirect_uri={0}", connect.CallbackUrl );
            sb.Append( "&grant_type=authorization_code" );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( sb.ToString() );
            request.Method = httpMethod;

            try {
                logger.Info( "begin request access token: " + sb.ToString() );
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    using (Stream responseStream = response.GetResponseStream()) {
                        using (StreamReader reader = new StreamReader( responseStream )) {
                            return getAccessTokenByResponse( reader.ReadToEnd() );
                        }
                    }
                }
            }
            catch (WebException wex) {
                HttpClientExceptionHelper.throwWebException( sb.ToString(), wex );
            }
            catch (Exception ex) {
                HttpClientExceptionHelper.throwOtherException( sb.ToString(), ex );
            }
            return null;
        }

        private static AccessToken getAccessTokenByResponse( String response ) {

            if (strUtil.IsNullOrEmpty( response )) throw new NullReferenceException( "无法获取 access token，response为空" );

            logger.Info( "parse access token: " + response );

            response = response.Trim();

            try {

                AccessToken accessToken;

                if (response.StartsWith( "{" ) == false && response.IndexOf( "&" ) > 0 && response.IndexOf( "=" ) > 0) {
                    accessToken = parseAccessTokenQueryString( response );
                }
                else {
                    // {"access_token":"XXX","expires_in":157679999,"uid":"XXX"} 
                    accessToken = new AccessToken( response );
                }

                if (strUtil.IsNullOrEmpty( accessToken.Token )) {
                    throw new HttpClientException( "解析 access token，结果为空:" + response );
                }

                return accessToken;
            }
            catch (Exception ex) {
                throw new HttpClientException( "解析 access token 失败" + Environment.NewLine + ex.Message );
            }
        }


        // access_token=xxx&expires_in=888888&refresh_token=xxx&openid=xxx&name=xxx&nick=xxx&state=
        private static AccessToken parseAccessTokenQueryString( string response ) {

            String[] arr = response.Split( '&' );
            AccessToken x = new AccessToken();
            foreach (String item in arr) {
                String[] arrPair = item.Split( '=' );
                if (arrPair.Length != 2) continue;

                if (arrPair[0] == "openid") {
                    x.Uid = arrPair[1];
                }
                else if (arrPair[0] == "name") {
                    continue; // 有openId的场合
                }
                else {
                    x.SetValue( arrPair[0], arrPair[1] );
                }
            }
            return x;
        }

    }
}
