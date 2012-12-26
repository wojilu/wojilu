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
using wojilu;

namespace wojilu.Log {

    /// <summary>
    /// 使用 null 模式的日志工具
    /// </summary>
    internal class NullLogger : ILog {

        public void Debug( String message ) {
        }

        public void Info( String message ) {
        }

        public void Warn( String message ) {
        }

        public void Error( String message ) {
        }

        public void Fatal( String message ) {
        }


        public String TypeName {
            set { }
        }

    }
}

