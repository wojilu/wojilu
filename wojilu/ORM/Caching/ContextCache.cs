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
using System.Web;
using wojilu.Web;
using wojilu.Data;

namespace wojilu.ORM.Caching {

    /// <summary>
    /// 一级缓存(上下文缓存)
    /// </summary>
    public class ContextCache {

        public static Object Get( String objectKey ) {
            if (objectKey == null) {
                throw new Exception( "get context cache : objectKey is empty" );
            }
            return Map[objectKey];
        }

        public static void Put( String objectKey, Object objectValue ) {
            if (strUtil.IsNullOrEmpty( objectKey )) {
                throw new Exception( "get context cache : objectKey is empty" );
            }
            Map[objectKey] = objectValue;
        }

        public static void Remove( String objectKey ) {
            if (!strUtil.IsNullOrEmpty( objectKey )) {
                Map.Remove( objectKey );
            }
        }

        public static void Remove( String typeFullName, int id ) {
            String objectKey = CacheKey.getObject( typeFullName, id );
            Remove( objectKey );
        }

        public static void Clear() {
            Map.Clear();
        }

        private static IDictionary Map {
            get { return DbContext.getContextCache(); }
        }



    }
}

