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
using System.Web.Security;

using wojilu.Web.Mvc;

namespace wojilu.Web.Context {

    /// <summary>
    /// 上下文初始化器，顺序是：InitViewer -> InitOwner -> InitController -> InitPermission -> InitApp
    /// </summary>
    public abstract class ContextInitBase {

        /// <summary>
        /// 初始化当前的 app
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InitApp( MvcContext ctx ) {
        }

        /// <summary>
        /// 初始化当前访问者
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InitViewer( MvcContext ctx ) {
        }

        /// <summary>
        /// 初始化当前被访问者
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InitOwner( MvcContext ctx ) {
        }

        /// <summary>
        /// 初始化当前 controller
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InitController( MvcContext ctx ) {
            ControllerBase controller = ControllerFactory.InitController( ctx );
            if (controller == null) {
                String typeName = ctx.route.getControllerNameWithoutRootNamespace();
                String msg = lang.get( "exControllerNotExist" ) + ": " + typeName;
                throw ctx.ex( HttpStatus.NotFound_404, msg );
            }

            ctx.utils.setController( controller );
        }

        /// <summary>
        /// 初始化权限检查
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void InitPermission( MvcContext ctx ) {
        }

    }
}

