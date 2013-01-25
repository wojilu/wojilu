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

namespace wojilu.Web {

    /// <summary>
    /// web 服务器反馈的 http 状态码
    /// </summary>
    public class HttpStatus {

        public static readonly String NotFound_404 = "404 Not Found";
        public static readonly String BadRequest_400 = "400 Bad Request";
        public static readonly String Unauthorized_401 = "401 Unauthorized";
        public static readonly String Forbidden_403 = "403 Forbidden";
        public static readonly String MethodNotAllowed_405 = "405 Method Not Allowed";

        public static readonly String Continue_100 = "100 Continue";
        public static readonly String OK_200 = "200 OK";
        public static readonly String Created_201 = "201 Created";
        public static readonly String Accepted_202 = "202 Accepted";

        public static readonly String MovedPermanently_301 = "301 Moved Permanently";
        public static readonly String Found_302 = "302 Found";
        public static readonly String SeeOther_303 = "303 See Other";
        public static readonly String NotModified_304 = "304 Not Modified";
        public static readonly String TemporaryRedirect_307 = "307 Temporary Redirect";

        public static readonly String InternalServerError_500 = "500 Internal Server Error";
        public static readonly String ServiceTemporarilyUnavailable_503 = "503 Service Temporarily Unavailable";

        private static readonly Dictionary<int, string> statusMap = getStatusMap();

        public static String GetStatusString( int customStatusCode ) {

            if (customStatusCode == 200) return OK_200;
            if (customStatusCode == 404) return NotFound_404;
            if (customStatusCode == 400) return BadRequest_400;
            if (customStatusCode == 403) return Forbidden_403;
            if (customStatusCode == 405) return MethodNotAllowed_405;

            String statusString;
            statusMap.TryGetValue( customStatusCode, out statusString );
            return statusString;
        }

        private static Dictionary<int, string> getStatusMap() {

            Dictionary<int, string> map = new Dictionary<int, string>();

            map[100] = Continue_100;
            map[200] = OK_200;
            map[201] = Created_201;
            map[202] = Accepted_202;

            map[301] = MovedPermanently_301;
            map[302] = Found_302;
            map[303] = SeeOther_303;
            map[304] = NotModified_304;
            map[307] = TemporaryRedirect_307;

            map[400] = BadRequest_400;
            map[401] = Unauthorized_401;
            map[403] = Forbidden_403;
            map[404] = NotFound_404;
            map[405] = MethodNotAllowed_405;

            map[500] = InternalServerError_500;
            map[503] = ServiceTemporarilyUnavailable_503;


            return map;
        }

    }

}
