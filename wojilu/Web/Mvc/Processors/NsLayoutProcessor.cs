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
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;

namespace wojilu.Web.Mvc.Processors {

    internal class NsLayoutProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( NsLayoutProcessor ) );

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginAddNsLayout( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;

            String content = context.getContent();

            int intNoLayout = ctx.utils.getNoLayout();
            if (intNoLayout < 0) intNoLayout = 0;

            IList paths = ctx.utils.getLayoutPath();
            int pathCount = paths.Count - intNoLayout;
            for (int i = 0; i < pathCount; i++) {

                Boolean isLastLayout = i == pathCount - 1;

                content = addLayoutPrivate( paths[i].ToString(), content, ctx, isLastLayout );

                if (ctx.utils.isEnd()) return;
            }


            if (intNoLayout > 0) {
                if (ctx.utils.isFrame()) {
                    content = MvcUtil.getFrameContent( content );
                }
                else {
                    content = MvcUtil.getNoLayoutContent( content );
                }
            }

            context.setContent( content );
        }

        // 0) path => Blog.Admin
        // 1) path => Blog
        // 2) path => ""
        private static String addLayoutPrivate( String path, String actionContent, MvcContext ctx, Boolean isLastLayout ) {

            ControllerBase controller = ControllerFactory.FindLayoutController( path, ctx );
            if (controller == null) return actionContent;

            ctx.controller.utils.addHidedLayouts( controller ); // 将controller中提到需要隐藏的控制器隐藏
            if (ctx.controller.utils.isHided( controller.GetType() )) return actionContent;

            // 检查缓存
            ActionCacheChecker ci = ActionCacheChecker.InitLayout( ctx, controller );
            Object cacheContent = ci.GetCache();
            if (cacheContent != null) {
                logger.Info( "load from nsLayoutCache=" + ci.CacheKey );
                return HtmlCombiner.combinePage( cacheContent.ToString(), actionContent );
            }

            controller.utils.switchViewToLayout();

            ActionRunner.runLayoutAction( ctx, controller, controller.Layout );

            if (ctx.utils.isEnd()) {
                return ctx.utils.getCurrentOutputString();
            }

            String actionResult = controller.utils.getActionResult();
            if (ci.IsActionCache) {
                ci.AddCache( actionResult );
            }

            return HtmlCombiner.combinePage( actionResult, actionContent );

        }

    }

}
