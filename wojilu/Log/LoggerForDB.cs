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
    /// 存储到数据库的日志(尚未实现，请勿使用)
    /// </summary>
    internal class LoggerForDB : ILog {

        private LogLevel _levelSetting;
        private LogMsg _msg;

        public LoggerForDB() {
            _levelSetting = LogConfig.Instance.Level;
            _msg = new LogMsg();
        }

        public void Debug( String message ) {

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "debug";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Debug) {
                _msg.Insert();
            }
        }

        public void Info( String message ) {

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "info";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Info) {
                _msg.Insert();
            }
        }

        public void Warn( String message ) {

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "warn";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Warn) {
                _msg.Insert();
            }
        }

        public void Error( String message ) {

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "error";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Error) {
                _msg.Insert();
            }
        }

        public void Fatal( String message ) {

            _msg.LogTime = DateTime.Now;
            _msg.Message = message;
            _msg.LogLevel = "fatal";
            System.Diagnostics.Debug.Write( LoggerUtil.GetFormatMsg( _msg ) );
            if (_levelSetting >= LogLevel.Fatal) {
                _msg.Insert();
            }
        }

        public String TypeName {
            set { _msg.TypeName = value; }
        }

    }
}

