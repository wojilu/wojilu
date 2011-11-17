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

namespace wojilu.Web.Jobs {

    /// <summary>
    /// 计划任务项的接口
    /// </summary>
    public interface IWebJobItem {

        /// <summary>
        /// 任务需要执行的方法
        /// </summary>
        void Execute();

        /// <summary>
        /// 任务结束时需要执行的方法
        /// </summary>
        void End();
    }

}
