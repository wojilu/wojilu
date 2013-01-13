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
using System.Reflection;

using wojilu.Web.Context;
using wojilu.Web.Mvc.Processors;
using wojilu.Caching;

namespace wojilu.Web.Mvc {

    internal class ProcessContext {

        public MvcContext ctx { get; set; }

        public Boolean IsUserLogin() { return ctx.viewer.IsLogin; }

        public ProcessContext( MvcContext ctx ) {
            this.ctx = ctx;
        }

        public void showEnd( String content ) {
            setContent( content );
            ctx.utils.end();
        }

        public void setContent( String content ) {
            ctx.utils.setCurrentOutputString( content );
        }

        public String getContent() {
            return ctx.utils.getCurrentOutputString();
        }


        private static List<ProcessorBase> initProcessor() {
            List<ProcessorBase> list = new List<ProcessorBase>();
            list.Add( new RouteProcessor() );
            list.Add( new InitContextProcessor() );
            list.Add( new ActionMethodChecker() );
            list.Add( new ForbiddenActionChecker() );
            list.Add( new LoginActionChecker() );
            list.Add( new HttpMethodChecker() );
            list.Add( new PermissionChecker() );
            list.Add( new ActionProcessor() );
            list.Add( new LayoutProcessor() );
            list.Add( new NsLayoutProcessor() );
            return list;
        }


        public static void Begin( MvcContext ctx ) {

            WebStopwatch.Start();

            MvcEventPublisher.Instance.BeginProcessMvc( ctx );

            List<ProcessorBase> processorList = initProcessor();
            ProcessContext context = new ProcessContext( ctx );
            foreach (ProcessorBase p in processorList) {

                if (ctx.utils.isEnd()) break; // showEnd 会跳过下面所有处理器，除了 RenderProcessor

                if (ctx.utils.GetCancelMvcProcessor().Contains( p.GetType() )) continue; // cancelProcessor 会跳过指定处理器

                p.Process( context );
                ctx.utils.skipCurrentProcessor( false ); // 重置状态 // skipCurrentProcessor 会跳过当前处理器的剩余部分
            }

            // 呈现页面内容
            if (skinRender( ctx ) == false) new RenderProcessor().Process( context );

            ctx.web.CompleteRequest();

        }

        private static bool skinRender( MvcContext ctx ) {

            if (ctx.utils.IsSkinRender()) return true; // skinRender会跳过页面呈现
            if (ctx.utils.GetCancelMvcProcessor().Contains( typeof( RenderProcessor ) )) return true;

            return false;
        }


    }

}
