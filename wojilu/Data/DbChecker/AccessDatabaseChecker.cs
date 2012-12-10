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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

using wojilu.ORM;

namespace wojilu.Data {

    internal class AccessDatabaseChecker : IDatabaseChecker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AccessDatabaseChecker ) );

        private String _connectionString;
        private List<String> existTables = new List<String>();

        public String ConnectionString {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public DatabaseType DatabaseType {
            get { return DatabaseType.Access; }
            set { }
        }

        public void CheckDatabase() {
            logger.Info( "begin check database" );
            if (strUtil.IsNullOrEmpty( _connectionString )) {
                logger.Info( "connection String is empty. begin to create access database and set connection string" );
                createDatabaseAndSaveConnectionString();
            }
            else {
                String connectionItem = DataFactory.GetDialect( DatabaseType.Access ).GetConnectionItem( _connectionString, ConnectionItemType.Database );
                if (strUtil.IsNullOrEmpty( connectionItem )) {
                    logger.Info( "connection String is found, but database is empty. begin to create access database and set connection string" );
                    createDatabaseAndSaveConnectionString();
                }
                else if (!File.Exists( connectionItem )) {
                    logger.Info( "ConnectionString:" + _connectionString );
                    logger.Info( "the database [" + connectionItem + "] is not found. begin to create access database and set connection string" );
                    DatabaseBuilder.BuildAccessDb4o( connectionItem );
                }
            }
        }

        public void CheckTable( MappingClass mapping, String db ) {
            logger.Info( "[access] begin check table" );

            OleDbConnection connection = DataFactory.GetConnection( _connectionString, this.DatabaseType ) as OleDbConnection;
            connection.Open();
            IDbCommand cmd = new OleDbCommand();
            cmd.Connection = connection;
            object[] restrictions = new object[4];
            restrictions[3] = "TABLE";
            DataTable oleDbSchemaTable = connection.GetOleDbSchemaTable( OleDbSchemaGuid.Tables, restrictions );
            foreach (DataRow row in oleDbSchemaTable.Rows) {
                existTables.Add( row["TABLE_NAME"].ToString() );
                logger.Info( "table found：" + row["TABLE_NAME"].ToString() );
            }
            existTables = new AccessTableBuilder().CheckMappingTableIsExist( cmd, db, existTables, mapping );
            connection.Close();

        }

        private void createDatabaseAndSaveConnectionString() {
            _connectionString = DatabaseBuilder.ConnectionStringPrefix + DatabaseBuilder.BuildAccessDb4o();
            logger.Info( "connection String : " + _connectionString );
            DbConfig.SaveConnectionString( _connectionString );
            logger.Info( "the connection String is resetted" );
        }



        public List<String> GetTables() {
            return existTables;
        }

    }
}

