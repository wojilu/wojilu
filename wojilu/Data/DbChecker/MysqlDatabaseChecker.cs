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
using System.Collections;
using System.Collections.Generic;

using wojilu.ORM;

namespace wojilu.Data {


    internal class MysqlDatabaseChecker : IDatabaseChecker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MysqlDatabaseChecker ) );

        private List<String> existTables = new List<String>();

        private String _connectionString;

        public String ConnectionString {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public DatabaseType DatabaseType {
            get { return DatabaseType.MySql; }
            set { }
        }

        public void CheckDatabase() {

            if (strUtil.IsNullOrEmpty( this._connectionString )) {
                throw new Exception( "connection string can not be empty" );
            }
            IDatabaseDialect dialect = DataFactory.GetDialect( DatabaseType.MySql );
            if (strUtil.IsNullOrEmpty( dialect.GetConnectionItem( this._connectionString, ConnectionItemType.Server ) )) {
                throw new Exception( "[mysql] server address is empty" );
            }
            if (strUtil.IsNullOrEmpty( dialect.GetConnectionItem( _connectionString, ConnectionItemType.Database ) )) {
                throw new Exception( "[mysql] database is empty" );
            }
        }

        public void CheckTable( MappingClass mapping, String db ) {

            logger.Info( "[mysql] begin check table" );
            IDbConnection connection = DataFactory.GetConnection( _connectionString, this.DatabaseType );
            connection.Open();

            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "show tables";

            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                existTables.Add( reader[0].ToString() );
                logger.Info( "table found：" + reader[0].ToString() );
            }

            reader.Close();
            existTables = new MySqlTableBuilder().CheckMappingTableIsExist( cmd, db, existTables, mapping );

            connection.Close();
        }


        public List<String> GetTables() {
            return existTables;
        }

    }
}

