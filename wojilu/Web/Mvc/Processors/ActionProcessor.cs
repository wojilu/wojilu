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

        private List<ActionObserver> obList = new List<ActionObserver>();

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginProcessAction( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;

            ControllerBase controller = ctx.controller;


            // 1) 检查 action 缓存
            ActionCacheChecker xChecker = ActionCacheChecker.InitAction( ctx );
            Object cacheContent = xChecker.GetCache();
            if (cacheContent != null) {
                logger.Info( "load from actionCache=" + xChecker.CacheKey );
                context.setContent( cacheContent.ToString() );
                setPageMeta_FromCache( ctx, xChecker.CacheKey );
                return;
            }

            // 2) 运行 before action (获取所有的 ActionObserver)
            List<ActionObserver> actionObservers = ControllerMeta.GetActionObservers( controller.GetType(), ctx.route.action );
            if (actionObservers != null) {
                foreach (ActionObserver x in actionObservers) {
                    ActionObserver ob = (ActionObserver)ObjectContext.CreateObject( x.GetType() );
                    obList.Add( ob );
                    Boolean isContinue = ob.BeforeAction( ctx );
                    if (!isContinue) return;
                }
            }

            // 3) 运行 action
            MethodInfo actionMethod = ctx.ActionMethodInfo; // context.getActionMethod();

            // 设值模板并载入全局变量
            setControllerView( controller, actionMethod );

            // 运行并处理post值
            ActionRunner.runAction( ctx, controller, actionMethod, controller.utils.runAction );
            if (ctx.utils.isEnd()) {
                afterAction( ctx );
                return;
            }

            String actionContent = controller.utils.getActionResult();

            // 4) 后续缓存处理
            if (xChecker.IsActionCache) {
                xChecker.AddCache( actionContent );
                addPageMeta_ToCache( ctx, xChecker.CacheKey );
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
            else {
                context.setContent( actionContent );
            }

            afterAction( ctx );
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

        private void afterAction( MvcContext ctx ) {

            runAfterAction( ctx );

            MvcEventPublisher.Instance.EndProcessAction( ctx );
        }

        private void runAfterAction( MvcContext ctx ) {

            List<String> pages = new List<String>();

            List<IPageCache> observedPages = new List<IPageCache>();
            foreach (ActionObserver ob in obList) {

                logger.Info( "run after ActionObserver=>" + ob.GetType() );
                ob.AfterAction( ctx );

                if (ob.GetType().IsSubclassOf( typeof( ActionCache ) )) {
                    loadObservedPage( (ActionCache)ob, observedPages );
                }
            }

            foreach (IPageCache pc in observedPages) {
                logger.Info( "update IPageCache=" + pc.GetType().FullName );
                pc.UpdateCache( ctx );
            }
        }

        private void loadObservedPage( ActionCache actionCache, List<IPageCache> observedPages ) {

            List<IPageCache> relatedPages = ControllerMeta.GetRelatedPageCache( actionCache.GetType() );
            if (relatedPages == null) return;

            foreach (IPageCache pc in relatedPages) {
                if (observedPages.Contains( pc ) == false) observedPages.Add( pc );
            }

        }

        private static void addPageMeta_ToCache( MvcContext ctx, String cacheKey ) {
            CacheManager.GetApplicationCache().Put( cacheKey + "_pageMeta", ctx.Page );
        }

        private static void setPageMeta_FromCache( MvcContext ctx, String cacheKey ) {

            PageMeta p = CacheManager.GetApplicationCache().Get( cacheKey + "_pageMeta" ) as PageMeta;
            if (p != null) {
                ctx.utils.setPageMeta( p );
            }
        }

    }

}
