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
using System.Collections;

using wojilu.Data;
using wojilu.Caching;

namespace wojilu.Caching {


    /// <summary>
    /// 应用程序范围的缓存(ORM的二级缓存)
    /// </summary>
    public class ApplicationCache : IApplicationCache {

        /// <summary>
        /// 从二级缓存中获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Get( String key ) {
            return SysCache.Get( key );
        }

        /// <summary>
        /// 将对象放入二级缓存，如果缓存中已有此项，则替换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put( String key, Object val ) {
            SysCache.Put( key, val );
        }

        /// <summary>
        /// 将对象放入缓存，最后一次访问之后的 minutes 分钟内，如果还没有访问，则会过期（弹性过期）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        public void Put( String key, Object val, int minutes ) {
            SysCache.PutSliding( key, val, minutes * 60 );
        }

        /// <summary>
        /// 从缓存中移除某项
        /// </summary>
        /// <param name="key"></param>
        public void Remove( String key ) {
            SysCache.Remove( key );
        }

        /// <summary>
        /// 从缓存中移除所有项
        /// </summary>
        public void Clear() {
            SysCache.Clear();
        }

        public IDictionaryEnumerator GetEnumerator() {
            return SysCache.GetEnumerator();
        }

        public int Count {
            get { return SysCache.Count; }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return SysCache.GetEnumerator();
        }

    }
}

