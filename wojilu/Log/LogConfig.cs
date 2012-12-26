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
using System.Collections.Generic;
using System.Text;
using wojilu.IO;
using System.Web;
using wojilu.Web;

namespace wojilu.Log {

    /// <summary>
    /// 日志配置文件，默认配置文件在 /framework/config/log.config，日志文件在 /framework/log/log.txt 中
    /// </summary>
    public class LogConfig {

        private static LogConfig _instance = new LogConfig();

        /// <summary>
        /// 日志配置信息(全局缓存)
        /// <remarks>
        /// logLevel 的值(不区分大小写)：none, debug, info, warn, error, fatal, all；
        /// logFile 和 logProvider 通常不用填写
        /// </remarks>
        /// <example>
        /// 配置文件的格式(一行一条配置，键值之间用冒号分开)。
        /// <code>
        /// logLevel : info
        /// logFile : log/log.txt
        /// logProvider : wojilu.Log.FileLogger
        /// </code>
        /// </example>
        /// </summary>
        public static LogConfig Instance {
            get { return _instance; }
        }

        private LogConfig() {

            String absPath = getConfigAbsPath();

            if (strUtil.IsNullOrEmpty( absPath )) {
                loadDefault();
                return;
            }

            Dictionary<String, String> dic = cfgHelper.Read( absPath );

            this.FilePath = getFilePath( dic );
            this.Level = getLevel( dic );
            this.LoggerImpl = getLoggerImpl( dic );

        }

        public static void Reset() {
            _instance = new LogConfig();
        }

        //--------------------------------------------------------------------------------------

        private LogLevel getLevel( Dictionary<String, String> dic ) {

            String level;
            dic.TryGetValue( "logLevel", out level );

            if (strUtil.IsNullOrEmpty( level )) return getDefaultLevel();

            try {
                return (LogLevel)Enum.Parse( typeof( LogLevel ), level, true );
            }
            catch { return getDefaultLevel(); }
        }

        private static LogLevel getDefaultLevel() {
            return LogLevel.None;
        }

        //--------------------------------------------------------------------------------------


        private void loadDefault() {
            this.Level = LogLevel.Debug;
            this.FilePath = getAbsoluteLogPath( getDefaultLogPath() );
        }

        //----------------------------- 配置的路径 ----------------------------------------------------------------

        private static String getConfigAbsPath() {
            String absolutePath = PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "log.config" ) );
            if (!File.Exists( absolutePath )) {
                absolutePath = null;
            }
            return absolutePath;
        }


        //------------------------------ 日志的路径 ---------------------------------------------------------------

        private static String getFilePath( Dictionary<String, String> dic ) {
            String filePath;
            dic.TryGetValue( "logFile", out filePath );
            if( strUtil.IsNullOrEmpty( filePath) ) filePath = getDefaultLogPath();
            return getAbsoluteLogPath( filePath);
        }

        private static String getDefaultLogPath() {
            return cfgHelper.FrameworkRoot + "log/log.txt";
        }

        private static String getAbsoluteLogPath( String path ) {

            if (path.StartsWith( cfgHelper.FrameworkRoot ) == false)
                path = strUtil.Join( cfgHelper.FrameworkRoot, path );

            return PathHelper.Map( path );
        }

        //---------------------------------------------------------------------------------------------

        private static String getLoggerImpl( Dictionary<String, String> dic ) {
            String logProvider;
            dic.TryGetValue( "logProvider", out logProvider );
            return logProvider;
        }

        //---------------------------------------------------------------------------------------------

        /// <summary>
        /// 记录的层次，不区分大小写，有 none, debug, info, warn, error, fatal, all 这几种可选
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// 日志文件存储的路径
        /// </summary>
        public String FilePath { get; set; }

        /// <summary>
        /// 日志记录工具，默认是 FileLogger
        /// </summary>
        public String LoggerImpl { get; set; }

    }

}
