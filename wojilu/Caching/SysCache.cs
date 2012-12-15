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
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace wojilu.Caching {

    /// <summary>
    /// .net 自带的 InMemory 缓存
    /// </summary>
    public class SysCache {

        /// <summary>
        /// 从缓存中获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Object Get( String key ) {
            return HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 将对象放入缓存，如果缓存中已有此项，则替换。a)永不过期，b)优先级为 Normal，c)没有缓存依赖项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Put( String key, Object val ) {
            HttpRuntime.Cache[key] = val;
        }

        /// <summary>
        /// 将对象放入缓存，在参数 seconds 指定的秒数之后过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="seconds"></param>
        public static void Put( String key, Object val, int seconds ) {
            HttpRuntime.Cache.Insert( key, val, null, DateTime.UtcNow.AddSeconds( (double)seconds ), Cache.NoSlidingExpiration );
        }

        /// <summary>
        /// 将对象放入缓存，在最后一次访问之后的 seconds 秒数之后过期（弹性过期）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="seconds"></param>
        public static void PutSliding( String key, Object val, int seconds ) {
            HttpRuntime.Cache.Insert( key, val, null, Cache.NoAbsoluteExpiration, new TimeSpan( 0, 0, seconds ) );
        }

        /// <summary>
        /// 从缓存中移除某项
        /// </summary>
        /// <param name="key"></param>
        public static void Remove( String key ) {
            if (strUtil.HasText( key )) {
                HttpRuntime.Cache.Remove( key );
            }
        }

        /// <summary>
        /// 从缓存中移除所有项
        /// </summary>
        public static void Clear() {
            foreach (DictionaryEntry entry in HttpRuntime.Cache) {
                HttpRuntime.Cache.Remove( entry.Key.ToString() );
            }
        }

        public static IDictionaryEnumerator GetEnumerator() {
            return HttpRuntime.Cache.GetEnumerator();
        }

        public static int Count {
            get {
                return HttpRuntime.Cache.Count;
            }
        }

    }
}
