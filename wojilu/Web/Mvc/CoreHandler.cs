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
using System.Web.SessionState;
using System.Text;
using System.Collections.Generic;

using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// wojilu mvc 的核心处理器：处理客户端请求，将结果返回
    /// </summary>
    public class CoreHandler : IHttpHandler, IRequiresSessionState {        


        public virtual void ProcessRequest( HttpContext context ) {

            if (MvcConfig.Instance.IsPageCache) {

                String key = MvcConfig.Instance.CheckDomainMap() ? context.Request.Url.ToString() : context.Request.Url.PathAndQuery;

                String pageContent = CacheManager.GetApplicationCache().Get( key ) as String;
                if (pageContent != null) {
                    context.Response.Write( pageContent );
                    return;
                }

            }

            MvcContext ctx = new MvcContext( new WebContext( context ) );
            ProcessContext.Begin( ctx );
        }

        public virtual void ProcessRequest( IWebContext context ) {
            ProcessContext.Begin( new MvcContext( context ) );
        }

        public virtual void ProcessRequest( MvcContext context ) {
            ProcessContext.Begin( context );
        }

        public Boolean IsReusable {
            get { return true; }
        }



    }
}

