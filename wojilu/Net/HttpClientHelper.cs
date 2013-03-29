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
using System.Globalization;
using System.Threading;

namespace wojilu.Net {

    public class HttpClientHelper : IHttpClientHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HttpClientHelper ) );

        public String Upload( String apiUrl, Dictionary<String, String> parameters, Dictionary<String, String> headers, List<HttpFile> files, String userAgent ) {

            if (files == null || files.Count == 0) return InvokeApi( apiUrl, "POST", ConstructQueryString( parameters ), headers );

            string boundary = Guid.NewGuid().ToString();
            string header = string.Format( "--{0}", boundary );
            string footer = string.Format( "--{0}--", boundary );

            StringBuilder sbParams = new StringBuilder();
            if (parameters.Count > 0) {
                foreach (KeyValuePair<string, string> kv in parameters) {
                    sbParams.AppendLine( header );
                    sbParams.AppendLine( String.Format( "Content-Disposition: form-data; name=\"{0}\"", kv.Key ) );
                    sbParams.AppendLine();
                    sbParams.AppendLine( kv.Value );
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (HttpFile x in files) {
                sb.AppendLine( header );
                sb.AppendLine( String.Format( "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "pic", x.FileName ) );
                sb.AppendLine( String.Format( "Content-Type: {0}", x.ContentType ) );
                sb.AppendLine();
                sb.AppendLine( encoder.GetString( x.FileStream ) );
            }
            sb.AppendLine( footer );

            return InvokeApi( apiUrl, "POST", sbParams.ToString(), headers, boundary, sb.ToString(), userAgent );
        }

        public String InvokeApi( String apiUrl, String httpMethod, String strQuery, Dictionary<String, String> headers, String boundary = "", String strFiles = "", String userAgent = "", String strEncoding = "" ) {

            if (strUtil.IsNullOrEmpty( apiUrl )) throw new ArgumentNullException( "api url" );
            if (strUtil.IsNullOrEmpty( httpMethod )) throw new ArgumentNullException( "httpMethod" );
            httpMethod = httpMethod.ToUpper();

            logger.Info( "request api url=" + apiUrl );

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( apiUrl );
            request.UserAgent = userAgent;
            request.Method = httpMethod;
            foreach (KeyValuePair<String, String> kv in headers) {
                request.Headers.Add( kv.Key, kv.Value );
            }

            if (httpMethod == HttpMethod.Post) {
                setPostParams( strQuery, boundary, strFiles, request );
            }

            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    using (Stream responseStream = response.GetResponseStream()) {

                        if (strUtil.IsNullOrEmpty( strEncoding )) {

                            using (StreamReader reader = new StreamReader( responseStream )) {
                                String strRet = reader.ReadToEnd();
                                logger.Info( "server response=" + strRet );
                                return strRet;
                            }
                        }
                        else {

                            using (StreamReader reader = new StreamReader( responseStream, getEncoding( strEncoding, response ) )) {
                                String strRet = reader.ReadToEnd();
                                logger.Info( "server response=" + strRet );
                                return strRet;
                            }
                        }
                    }
                }
            }
            catch (WebException wex) {
                HttpClientExceptionHelper.throwWebException( apiUrl, wex );
            }
            catch (Exception ex) {
                HttpClientExceptionHelper.throwOtherException( apiUrl, ex );
            }
            return null;
        }

        private Encoding getEncoding( string strEncoding, WebResponse response ) {
            if (strUtil.HasText( strEncoding )) {
                return Encoding.GetEncoding( strEncoding );
            }

            return getEncoding( response );
        }

        private static Encoding getEncoding( WebResponse response ) {

            Encoding _encoding = Encoding.Default;

            try {
                String contentType = response.ContentType;
                if (contentType == null) {
                    return _encoding;
                }
                String[] strArray = contentType.ToLower( CultureInfo.InvariantCulture ).Split( new char[] { ';', '=', ' ' } );
                Boolean isFind = false;
                foreach (String item in strArray) {
                    if (item == "charset") {
                        isFind = true;
                    }
                    else if (isFind) {
                        return Encoding.GetEncoding( item );
                    }
                }
            }
            catch (Exception exception) {
                if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException)) {
                    throw;
                }
            }
            return _encoding;
        }


        private static void setPostParams( String strQuery, String boundary, String strFiles, HttpWebRequest request ) {
            request.ContentLength = Encoding.UTF8.GetBytes( strQuery ).Length + encoder.GetBytes( strFiles ).Length;

            if (strUtil.HasText( strFiles )) {
                request.ContentType = string.Format( "multipart/form-data; boundary={0}", boundary );
            }
            else {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            using (Stream dataStream = request.GetRequestStream()) {
                dataStream.Write( Encoding.UTF8.GetBytes( strQuery ), 0, Encoding.UTF8.GetByteCount( strQuery ) );
                dataStream.Write( encoder.GetBytes( strFiles ), 0, encoder.GetByteCount( strFiles ) );
            }
        }


        public String ConstructQueryString( Dictionary<String, String> parameters ) {

            if (parameters == null || parameters.Count == 0) return "";

            String str = "";
            foreach (KeyValuePair<String, String> kv in parameters) {
                str += kv.Key + "=" + System.Web.HttpUtility.UrlEncode( kv.Value );
                str += "&";
            }

            return str.TrimEnd( '&' );
        }


        private static Encoding encoder = Encoding.GetEncoding( "iso-8859-1" );


    }

}
