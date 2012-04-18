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

namespace wojilu.Data {

    /// <summary>
    /// 实体键值类型
    /// </summary>
    public static class IdType
    {
        /// <summary>
        /// 自动（默认类型）
        /// </summary>
        public static readonly string Auto="auto";

        /// <summary>
        /// 整型
        /// </summary>
        public static readonly string Int = "int";

        /// <summary>
        /// 长整型
        /// </summary>
        public static readonly string Long = "long";

        /// <summary>
        /// Globally Unique Identifier（全球唯一标识符） 也称作 UUID(Universally Unique IDentifier) 。
        /// </summary>
        public static readonly string Guid = "guid";
        
        /// <summary>
        /// 字符型
        /// </summary>
        public static readonly string String = "string";
    }
}
