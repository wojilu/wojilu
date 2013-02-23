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
using System.Net;
using System.IO;

namespace wojilu.Net {

    internal class HttpClientExceptionHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HttpClientExceptionHelper ) );

        internal static void throwWebException( String apiUrl, WebException wex ) {
            WebResponse response = wex.Response;
            using (Stream responseStream = response.GetResponseStream()) {
                using (StreamReader reader = new StreamReader( responseStream )) {

                    String error = reader.ReadToEnd();

                    String msg = "调用 API 失败: " + apiUrl + Environment.NewLine +
                        "错误原因：" + error;

                    logger.Error( msg );

                    HttpClientException ex = new HttpClientException( msg );
                    ex.ErrorInfo = error;
                    throw ex;
                }
            }
        }

        internal static void throwOtherException( String apiUrl, Exception ex ) {
            String msg = "调用 API 失败: " + apiUrl + Environment.NewLine + ex.Message;
            throw new HttpClientException( msg );
        }
    }
}
