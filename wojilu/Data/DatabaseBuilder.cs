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
using System.Data;
using System.IO;
using System.Web;

using wojilu.Reflection;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.Data {


    internal class DatabaseBuilder {

        public static String ConnectionStringPrefix = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
        private static readonly ILog logger = LogManager.GetLogger( typeof( DatabaseBuilder ) );

        public static String BuildAccessDb4o() {
            String dbPath = getDbPath();
            BuildAccessDb4o( dbPath );
            return dbPath;
        }

        public static void BuildAccessDb4o( String dbPath ) {
            String str = ConnectionStringPrefix + dbPath;
            logger.Info( "creating database : " + str );
            Object instanceFromProgId = ReflectionUtil.GetInstanceFromProgId( "ADOX.Catalog" );
            try {
                ReflectionUtil.CallMethod( instanceFromProgId, "Create", new object[] { str } );
            }
            catch (Exception exception) {
                logger.Info( "creating database error : " + exception.Message );
                LogManager.Flush();
                throw exception;
            }
            logger.Info( "create database ok" );
        }

        //public static void Compact( String dbPath ) {
        //    if (!File.Exists( dbPath )) {
        //        throw new Exception( "database not found" );
        //    }
        //    IDbConnection connection = DbContext.getConnection();
        //    if ((connection != null) && (connection.State == ConnectionState.Open)) {
        //        connection.Close();
        //    }
        //    String sourceFileName = dbPath + ".bak";
        //    ReflectionUtil.CallMethod( ReflectionUtil.GetInstanceFromProgId( "JRO.JetEngine" ), "CompactDatabase", new object[] { ConnectionStringPrefix + dbPath, ConnectionStringPrefix + sourceFileName } );
        //    File.Copy( sourceFileName, dbPath, true );
        //    File.Delete( sourceFileName );
        //}

        private static String getDbPath() {
            DateTime now = DateTime.Now;
            String path = "wojiluDB_" + Guid.NewGuid().ToString().Replace( "-", "" ) + ".mdb";
            path = PathHelper.Map( path );
            return path;
        }

    }
}

