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
using System.Web;

namespace wojilu.Web.Handler {

    /// <summary>
    /// 网页不存在的处理器
    /// </summary>
    public class PageNotFoundHandler : IHttpHandler {

        public Boolean IsReusable {
            get { return true; }
        }

        public void ProcessRequest( HttpContext context ) {
            context.Response.Status = HttpStatus.NotFound_404;
            context.Response.Write( "<h1>sorry, page not found.</h1>" );
            context.ApplicationInstance.CompleteRequest();
        }

    }

}
