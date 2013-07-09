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
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace wojilu.Data {

    /// <summary>
    /// mysql 数据工厂，获取 Connection, Command, DataAdapter
    /// </summary>
    public class MysqlFactory : DbFactoryBase {

        private static IMysqlHelper GetMysqlHelper() {
            return MysqlHelperLoader.GetHelper();
        }

        public static Type mySqlCommandType {
            get { return GetMysqlHelper().GetMySqlCommandType(); }
        }

        public static Type mySqlConnectionType {
            get { return GetMysqlHelper().GetMySqlConnectionType(); }
        }

        public override IDbConnection GetConnection( String connectionString ) {
            return GetMysqlHelper().GetMySqlConnection( connectionString );
        }

        public override IDbCommand GetCommand( String CommandText ) {
            base.checkOpen();
            IDbCommand cmd = GetMysqlHelper().GetMySqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = CommandText;
            base.setTransaction( cmd );
            return cmd;
        }

        internal override IDatabaseChecker GetDatabaseChecker() {
            return new MysqlDatabaseChecker();
        }

        public override IDatabaseDialect GetDialect() {
            return new MysqlDialect();
        }

        public override Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue ) {

            if (parameterValue == null) return parameterValue;

            parameterValue = base.processValue( parameterValue );
            parameterName = new MysqlDialect().GetParameterAdder( parameterName );

            IDbDataParameter parameter = GetMysqlHelper().GetMySqlParameter( parameterName, parameterValue );
            cmd.Parameters.Add( parameter );

            return parameterValue;

        }

        public override DbDataAdapter GetAdapter() {
            return GetMysqlHelper().GetMySqlDataAdapter( cmd );
        }

        public override DbDataAdapter GetAdapter( String CommandText ) {
            return GetMysqlHelper().GetMySqlDataAdapter( GetCommand( CommandText ) );
        }



    }

}
