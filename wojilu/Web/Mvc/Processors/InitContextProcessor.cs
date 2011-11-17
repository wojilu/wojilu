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

using wojilu.DI;
using wojilu.Web.Jobs;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Context;
using wojilu.Common.Onlines;

namespace wojilu.Web.Mvc.Processors {


    internal class InitContextProcessor : ProcessorBase {

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginInitContext( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;

            ContextInitBase initor = getContextInit();

            initor.InitViewer( ctx );       // 初始化当前登录用户(访问者) 
            initor.InitOwner( ctx );       // 初始化当前被访问对象(site或group或user)
            initor.InitController( ctx );  // 初始化控制器
            initor.InitPermission( ctx ); // 初始化权限检查
            initor.InitApp( ctx );                 // 初始化当前app
        }

        private ContextInitBase getContextInit() {
            ContextInitBase initor = ObjectContext.GetByName( "contextInit" ) as ContextInitBase;
            if (initor == null) return new ContextInitDefault();
            return initor;
        }

    }



}
