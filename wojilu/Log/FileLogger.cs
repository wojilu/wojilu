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
using System.Diagnostics;

using wojilu;

namespace wojilu.Log {

    /// <summary>
    /// 文件日志工具，所有日志会被写入磁盘
    /// </summary>
    internal class FileLogger : ILog {

        private LogLevel _levelSetting;
        private LogMessage _msg;

        public FileLogger() {
            _levelSetting = LogConfig.Instance.Level;
            _msg = new LogMessage();
        }

        public void Debug( String message ) {
            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "debug";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Debug) {
                LoggerUtil.WriteFile( _msg );
            }
        }

        public void Info( String message ) {

            Boolean isSql = false;
            if (message.StartsWith( LoggerUtil.SqlPrefix )) {
                isSql = true;
                message = strUtil.TrimStart( message, LoggerUtil.SqlPrefix );
            }

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "info";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Info) {
                LoggerUtil.WriteFile( _msg );
            }

            if (isSql) LoggerUtil.LogSqlCount();
        }


        public void Warn( String message ) {
            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "warn";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Warn) {
                LoggerUtil.WriteFile( _msg );
            }
        }

        public void Error( String message ) {
            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "error";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Error) {
                LoggerUtil.WriteFile( _msg );
            }
        }

        public void Fatal( String message ) {
            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "fatal";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Fatal) {
                LoggerUtil.WriteFile( _msg );
            }
        }

        public String TypeName {
            set {
                _msg.TypeName = value;
            }
        }


    }
}

