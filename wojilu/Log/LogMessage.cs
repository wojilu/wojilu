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

namespace wojilu.Log {

    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogMessage : ILogMsg {

        private String _level;
        private DateTime _logTime;
        private String _message;
        private String _typeName;

        public String LogLevel {
            get { return _level; }
            set { _level = value; }
        }

        public DateTime LogTime {
            get { return _logTime; }
            set { _logTime = value; }
        }

        public String Message {
            get { return _message; }
            set { _message = value; }
        }

        public String TypeName {
            get { return _typeName; }
            set { _typeName = value; }
        }

    }
}

