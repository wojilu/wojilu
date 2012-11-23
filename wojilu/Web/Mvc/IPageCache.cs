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
using wojilu.Web.Context;
using wojilu.Caching;

namespace wojilu.Web.Mvc {


    /// <summary>
    /// 页面缓存的接口
    /// </summary>
    public interface IPageCache {

        /// <summary>
        /// 是否缓存。页面可能运行在不同环境下(比如不同的owner)，本方法决定哪些环境需要缓存，哪些不需要缓存。
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        Boolean IsCache( MvcContext ctx );

        /// <summary>
        /// 获取所有相关的布局页面缓存。主要供框架调用
        /// </summary>
        /// <returns></returns>
        List<Type> GetRelatedActions();

        /// <summary>
        /// 一旦其他局部页面的缓存发生变化，则 UpdateCache 方法被执行。本方法让缓存及时更新或失效
        /// </summary>
        /// <param name="ctx"></param>
        void UpdateCache( MvcContext ctx );

        /// <summary>
        /// 网页内容被添加进缓存之后的后续动作
        /// </summary>
        /// <param name="ctx"></param>
        void AfterCachePage( MvcContext ctx );

    }


}
