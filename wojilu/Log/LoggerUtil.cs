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
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;

using wojilu;
using wojilu.IO;
using wojilu.ORM;
using wojilu.Web;
using wojilu.Data;

namespace wojilu.Log {

    /// <summary>
    /// 日志处理工具
    /// </summary>
    public class LoggerUtil {

        private static Object objLock = new object();

        /// <summary>
        /// sql 日志的前缀
        /// </summary>
        public static readonly String SqlPrefix = "sql=";

        private static readonly String _contextLogItem = "currentLogList";
        private static String getSqlCountLabel() {
            return DbContext.SqlCountLabel;
        }

        /// <summary>
        /// 在 web 系统中，记录 sql 执行的次数
        /// </summary>
        public static void LogSqlCount() {

            if (CurrentRequest.getItem( getSqlCountLabel() ) == null) {
                CurrentRequest.setItem( getSqlCountLabel(), 1 );
            }
            else {
                CurrentRequest.setItem( getSqlCountLabel(), ((int)CurrentRequest.getItem( getSqlCountLabel() )) + 1 );
            }
        }

        /// <summary>
        /// 将日志写入磁盘
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteFile( ILogMsg msg ) {

            if (SystemInfo.IsWeb == false) {
                writeFilePrivate( msg );
                return;
            }

            StringBuilder sb = CurrentRequest.getItem( _contextLogItem ) as StringBuilder;
            if (sb == null) {
                sb = new StringBuilder();
                CurrentRequest.setItem( _contextLogItem, sb );
            }

            sb.AppendFormat( "{0} {1} {2} - {3} \r\n", msg.LogTime, msg.LogLevel, msg.TypeName, msg.Message );

        }

        private static void writeFilePrivate( ILogMsg msg ) {

            String formatMsg = GetFormatMsg( msg );
            writeContentToFile( formatMsg );
        }

        private static void writeContentToFile( String formatMsg ) {

            String logFilePath = LogConfig.Instance.FilePath;
            lock (objLock) {
                if (wojilu.IO.File.Exists( logFilePath )) {
                    DateTime lastAccessTime = System.IO.File.GetLastWriteTime( logFilePath );
                    DateTime now = DateTime.Now;

                    if (cvt.IsDayEqual( lastAccessTime, now )) {
                        wojilu.IO.File.Append( logFilePath, formatMsg );
                    }
                    else {
                        String destFileName = getDestFileName( logFilePath );
                        if (file.Exists( destFileName ) == false) {
                            wojilu.IO.File.Move( logFilePath, destFileName );
                        }
                        wojilu.IO.File.Write( logFilePath, formatMsg );
                    }
                }
                else {
                    if (Directory.Exists( Path.GetDirectoryName( logFilePath ) ) == false) {
                        Directory.CreateDirectory( Path.GetDirectoryName( logFilePath ) );
                    }
                    wojilu.IO.File.Write( logFilePath, formatMsg );
                }
            }
        }

        private static String getDestFileName( string logFilePath ) {
            String ext = Path.GetExtension( logFilePath );
            String pathWithoutExt = strUtil.TrimEnd( logFilePath, ext );
            return pathWithoutExt + "_" + DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ).ToString( "yyyy.MM.dd" ) + ext;
        }

        public static String GetFormatMsg( ILogMsg logMsg ) {
            return String.Format( "{0} {1} {2} - {3} \r\n", logMsg.LogTime, logMsg.LogLevel, logMsg.TypeName, logMsg.Message );
        }

        /// <summary>
        /// 将所有日志即可写入磁盘
        /// </summary>
        internal static void Flush() {

            StringBuilder sb = CurrentRequest.getItem( _contextLogItem ) as StringBuilder;

            if (sb != null) {
                writeContentToFile( sb.ToString() );
                CurrentRequest.setItem( _contextLogItem, null );
            }

        }


    }
}

