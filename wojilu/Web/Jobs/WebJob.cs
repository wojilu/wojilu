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

namespace wojilu.Web.Jobs {

    /// <summary>
    /// 计划任务对象
    /// </summary>
    public class WebJob : CacheObject {

        public WebJob() {
            this.IsRunning = true;
        }

        /// <summary>
        /// 类的完整名称，比如wojilu.Common.Jobs.RefreshServerJob
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 间隔时间。单位:ms
        /// </summary>
        public int Interval { get; set; } 

        /// <summary>
        /// 是否运行
        /// </summary>
        public Boolean IsRunning { get; set; }

    }

}
