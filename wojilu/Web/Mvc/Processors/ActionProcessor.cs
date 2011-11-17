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
using System.Reflection;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.DI;
using wojilu.Web.Mvc.Attr;
using wojilu.Caching;

namespace wojilu.Web.Mvc.Processors {

    internal class ActionProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ActionProcessor ) );

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginProcessAction( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;

            ControllerBase controller = context.getController();

            // 检查缓存
            CacheInfo ci = CacheInfo.Init( ctx );
            Object cacheContent = ci.CheckCache();
            if (cacheContent != null) {
                logger.Info( "load from actionCache=" + ci.CacheKey );
                context.setContent( cacheContent.ToString() );
                getPageMetaFromCache( ctx, ci.CacheKey );
                return;
            }

            MethodInfo actionMethod = ctx.ActionMethodInfo; // context.getActionMethod();

            // 设值模板并载入全局变量
            setControllerView( controller, actionMethod );

            // 运行并处理post值
            ActionRunner.runAction( ctx, controller, actionMethod, controller.utils.runAction );
            String actionContent = controller.utils.getActionResult();

            // 加入缓存
            if (ci.IsActionCache) {
                ci.AddContentToCache( actionContent );
                // 加入PageMeta
                addPageMetaToCache( ctx, ci.CacheKey );
            }

            actionContent = PostValueProcessor.ProcessPostValue( actionContent, ctx );

            if (ctx.utils.isAjax) {
                context.showEnd( actionContent );
            }
            else if (ctx.utils.isFrame()) {

                int intNoLayout = ctx.utils.getNoLayout();

                if (intNoLayout == 0) {

                    String content = MvcUtil.getFrameContent( actionContent );
                    context.showEnd( content );
                }
                else {
                    context.setContent( actionContent );
                }


            }
            else if (ctx.utils.isEnd()) {
                context.showEnd( actionContent );
            }
            else {
                context.setContent( actionContent );
            }

            updateActionCache( ctx );

            MvcEventPublisher.Instance.EndProcessAction( context.ctx );

        }

        //------------------------------------------------------------------------

        private void setControllerView( ControllerBase controller, MethodInfo actionMethod ) {

            // 控制器继承
            if (actionMethod.DeclaringType != controller.GetType()) {
                String filePath = MvcUtil.getParentViewPath( actionMethod, controller.ctx.route.getRootNamespace( actionMethod.DeclaringType.FullName ) );
                controller.utils.setCurrentView( controller.utils.getTemplateByFileName( filePath ) );
            }
            else {
                controller.view( controller.ctx.route.action );
            }
        }

        //--------------------------------------------------------------------------

        private static void addPageMetaToCache( MvcContext ctx, String cacheKey ) {
            CacheManager.GetApplicationCache().Put( cacheKey + "_pageMeta", ctx.GetPageMeta() );
        }

        private static void getPageMetaFromCache( MvcContext ctx, String cacheKey ) {

            PageMeta p = CacheManager.GetApplicationCache().Get( cacheKey + "_pageMeta" ) as PageMeta;
            if (p != null) {
                ctx.utils.setPageMeta( p );
            }
        }

        //------------------------------------------------------------------------

        private void updateActionCache( MvcContext ctx ) {

            if (ctx.HttpMethod.Equals( "GET" )) return;

            List<String> pages = new List<String>();

            List<IActionCache> relatedCaches = ControllerMeta.GetActionCacheByUpdate( ctx.controller.GetType(), ctx.route.action );
            if (relatedCaches == null) return;

            List<IPageCache> pageCaches = new List<IPageCache>();
            foreach (IActionCache ac in relatedCaches) {
                logger.Info( "update IActionCache=" + ac.GetType().FullName );
                ac.UpdateCache( ctx );
                addPageCache( ac, pageCaches );
            }

            foreach (IPageCache pc in pageCaches) {
                logger.Info( "update IPageCache=" + pc.GetType().FullName );
                pc.UpdateCache( ctx );
            }
        }

        private void addPageCache( IActionCache ac, List<IPageCache> pageCaches ) {

            List<IPageCache> relatedCaches = ControllerMeta.GetPageCacheByUpdate( ac.GetType() );
            if (relatedCaches == null) return;

            foreach (IPageCache pc in relatedCaches) {
                if (pageCaches.Contains( pc ) == false) pageCaches.Add( pc );
            }

        }



    }

}
