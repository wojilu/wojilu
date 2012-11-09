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
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc.Processors {

    internal class RouteProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RouteProcessor ) );

        public override void Process( ProcessContext context ) {

            MvcContext ctx = context.ctx;

            MvcEventPublisher.Instance.BeginParseRoute( ctx );
            if (ctx.utils.isSkipCurrentProcessor()) return;

            try {
                Route r = RouteTool.Recognize( ctx );

                if (r == null) {
                    throw new MvcException( HttpStatus.NotFound_404, "can't find matching route : Url=" + ctx.url.ToString() );
                }

                ctx.utils.setRoute( r );
                IsSiteClosed( ctx );
            }
            catch (MvcException ex) {
                ctx.utils.endMsg( ex.Message, HttpStatus.NotFound_404 );
            }

        }

        private void IsSiteClosed( MvcContext ctx ) {

            Boolean isAdmin = isSiteAdmin( ctx.route );

            if (!config.Instance.Site.IsClose || isAdmin != false) return;

            ctx.utils.end();
            ctx.utils.skipRender();
            ctx.web.ResponseWrite( config.Instance.Site.CloseReason );
            ctx.web.CompleteRequest();
        }

        private Boolean isSiteAdmin( Route route ) {
            String ns = route.ns;
            if (strUtil.IsNullOrEmpty( ns )) return false;
            if (ns.StartsWith( "Admin." ) || ns.Equals( "Admin" )) return true;
            return false;
        }

    }


}
