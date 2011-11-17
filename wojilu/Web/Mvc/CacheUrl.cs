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

namespace wojilu.Web.Mvc {

    /// <summary>
    /// Name属性存储url的路径(不包括域名和虚拟目录，以斜杠开头；后缀名根据实际有没有确定)
    /// </summary>
    public class CacheUrl : CacheObject {

        /// <summary>
        /// 0或者小于0表示不缓存
        /// </summary>
        public int CacheSeconds { get; set; }

    }

}
