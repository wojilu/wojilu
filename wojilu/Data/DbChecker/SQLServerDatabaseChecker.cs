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
using System.Data.SqlClient;

using wojilu.ORM;

namespace wojilu.Data {


    internal class SQLServerDatabaseChecker : IDatabaseChecker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SQLServerDatabaseChecker ) );

        private String _connectionString;
        private DatabaseType _databaseType;

        public String ConnectionString {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public DatabaseType DatabaseType {
            get { return _databaseType; }
            set { _databaseType = value; }
        }

        private List<String> existTables = new List<String>();

        public void CheckDatabase() {
            if (strUtil.IsNullOrEmpty( _connectionString )) {
                throw new Exception( "[sqlserver] connection String is not found" );
            }
            IDatabaseDialect dialect = DataFactory.GetDialect( DatabaseType.SqlServer );
            if (strUtil.IsNullOrEmpty( dialect.GetConnectionItem( _connectionString, ConnectionItemType.Server ) )) {
                throw new Exception( "[sqlserver] address is empty" );
            }
            if (strUtil.IsNullOrEmpty( dialect.GetConnectionItem( _connectionString, ConnectionItemType.Database ) )) {
                throw new Exception( "[sqlserver] database is empty" );
            }
        }

        public void CheckTable( MappingClass mapping, String db ) {
            logger.Info( "[sqlserver] begin check table" );
            SqlConnection connection = new SqlConnection( _connectionString );
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT OBJECT_NAME(id) as name FROM sysobjects WHERE xtype='U' AND OBJECTPROPERTY(id, 'IsMSShipped') = 0";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                existTables.Add( reader["name"].ToString() );
                logger.Info( "table found：" + reader["name"].ToString() );
            }
            reader.Close();
            existTables = new SqlServerTableBuilder().CheckMappingTableIsExist( cmd, db, existTables, mapping );

            connection.Close();
        }

        public List<String> GetTables() {
            return existTables;
        }

    }
}

