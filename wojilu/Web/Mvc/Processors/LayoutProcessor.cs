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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;

namespace wojilu.Web.Mvc.Processors {

    internal class LayoutProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( LayoutProcessor ) );

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginAddLayout( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;
            ControllerBase controller = ctx.controller;

            if (controller.utils.isHided( controller.GetType() )) {
                return;
            }

            int intNoLayout = ctx.utils.getNoLayout();
            IList paths = ctx.utils.getLayoutPath();
            if (intNoLayout >= paths.Count + 1) return;

            String actionContent = context.getContent();


            //检查缓存
            ActionCacheChecker ci = ActionCacheChecker.InitLayout( ctx );
            Object cacheContent = ci.GetCache();
            if (cacheContent != null) {
                logger.Info( "load from layoutCache=" + ci.CacheKey );
                context.setContent( HtmlCombiner.combinePage( cacheContent.ToString(), actionContent ) );
                return;
            }

            String layoutContent = ControllerRunner.RunLayout( ctx );

            // 加入缓存
            if (ci.IsActionCache) {
                ci.AddCache( layoutContent );
            }

            if (ctx.utils.isEnd()) return;

            context.setContent( HtmlCombiner.combinePage( layoutContent, actionContent ) );
        }


    }

}
