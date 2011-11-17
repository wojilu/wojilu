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
using wojilu.Data;
using wojilu.DI;
using wojilu.Web;

namespace wojilu.Caching {

    /// <summary>
    /// 缓存管理器
    /// </summary>
    public class CacheManager {

        /// <summary>
        /// 获取 ApplicationCache
        /// </summary>
        /// <returns></returns>
        public static IApplicationCache GetApplicationCache() {


            String cfgCache = DbConfig.Instance.ApplicationCacheManager;

            if (strUtil.IsNullOrEmpty( cfgCache )) return defaultCache();
            if (ObjectContext.Instance.TypeList.ContainsKey( cfgCache ) == false) return defaultCache();

            return ObjectContext.GetByType( cfgCache ) as IApplicationCache;

        }

        private static IApplicationCache defaultCache() {
            return new ApplicationCache();
        }



    }

}
