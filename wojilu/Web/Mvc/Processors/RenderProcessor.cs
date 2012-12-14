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

using wojilu.Data;
using wojilu.Caching;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc.Processors {

    internal class RenderProcessor : ProcessorBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( RenderProcessor ) );


        public override void Process( ProcessContext context ) {

            MvcContext ctx = context.ctx;


            // page meta
            String pageContent = context.getContent();
            pageContent = processPageMeta( pageContent, ctx );
            context.setContent( pageContent );


            // page cache
            if (MvcConfig.Instance.IsPageCache) {
                addPageCache( context, ctx );
            }

            MvcEventPublisher.Instance.BeginRender( ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            ctx.utils.clearResource();
            ctx.web.ResponseWrite( context.getContent() );

            MvcEventPublisher.Instance.EndRender( ctx );
        }

        private static void addPageCache( ProcessContext context, MvcContext ctx ) {

            if (ctx.controller == null) return;
            if (ctx.IsMock) return;

            IPageCache pageCache = ControllerMeta.GetPageCache( ctx.controller.GetType(), ctx.route.action );
            if (pageCache == null) return;
            if (pageCache.IsCache( ctx ) == false) return;

            String key = ctx.url.PathAndQuery;
            if (MvcConfig.Instance.CheckDomainMap()) key = ctx.url.ToString();

            CacheManager.GetApplicationCache().Put( key, context.getContent() );

            logger.Info( "add page cache, key=" + key );
            pageCache.AfterCachePage( ctx );
        }

        private static String processPageMeta( string pageContent, MvcContext ctx ) {

            if (pageContent == null) return "";

            String title = ctx.Page.Title;
            if (strUtil.IsNullOrEmpty( title )) title = config.Instance.Site.SiteName;
            if (ctx.utils.getIsHome()) title = config.Instance.Site.SiteName + lang.get( "homePage" );

            String result = pageContent.Replace( "#{pageTitle}", title );
            result = result.Replace( "#{pageDescription}", ctx.Page.Description );
            result = result.Replace( "#{pageKeywords}", ctx.Page.Keywords );
            result = result.Replace( "#{pageRssLink}", ctx.Page.RssLink );

            return result;
        }




    }

}
