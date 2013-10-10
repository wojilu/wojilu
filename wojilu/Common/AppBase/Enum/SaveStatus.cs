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


namespace wojilu.Common.AppBase {


    /// <summary>
    /// 存储状态(普通、草稿、删除、系统删除)
    /// </summary>
    public class SaveStatus {

        public static readonly int Normal = 0;
        public static readonly int Draft = 1;
        public static readonly int Delete = 2;
        public static readonly int SysDelete = 3;

        /// <summary>
        /// 私有数据，不公开显示
        /// </summary>
        public static readonly int Private = 4;


    }

}

