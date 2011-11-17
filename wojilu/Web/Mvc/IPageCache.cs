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

        Boolean IsCache( MvcContext ctx );

        /// <summary>
        /// 本页面所有关联的 IActionCache。一旦关联的 IActionCache 被操作，则页面缓存会失效
        /// </summary>
        /// <returns></returns>
        List<Type> GetRelatedActions();

        /// <summary>
        /// 关联的 IActionCache 被刷新之后，需要重建本页面缓存的具体操作
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
