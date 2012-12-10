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


        public override IDbConnection GetConnection( String connectionString ) {
            return getMySqlConnection( connectionString );
        }

        public override IDbCommand GetCommand( String CommandText ) {
            base.checkOpen();
            IDbCommand cmd = getMySqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = CommandText;
            setTransaction( cmd );
            return cmd;
        }

        internal override IDatabaseChecker GetDatabaseChecker() {
            return new MysqlDatabaseChecker();
        }

        public override IDatabaseDialect GetDialect() {
            return new MysqlDialect();
        }

        public override Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue ) {

            parameterValue = base.processValue( parameterValue );
            parameterName = new MysqlDialect().GetParameterAdder( parameterName );

            IDbDataParameter parameter = getMySqlParameter( parameterName, parameterValue );
            cmd.Parameters.Add( parameter );

            return parameterValue;
        }

        public override DbDataAdapter GetAdapter() {
            return getMySqlDataAdapter( cmd );
        }

        public override DbDataAdapter GetAdapter( String CommandText ) {
            return getMySqlDataAdapter( GetCommand( CommandText ) );
        }

        //-----------------------------------

        private static IDbCommand getMySqlCommand() {
            return (rft.GetInstance( mySqlCommandType ) as IDbCommand);
        }

        private static IDbConnection getMySqlConnection( String connectionString ) {
            return (rft.GetInstance( mySqlConnectionType, new object[] { connectionString } ) as IDbConnection);
        }

        private static DbDataAdapter getMySqlDataAdapter( Object cmd ) {
            return (rft.GetInstance( mySqlDataAssembly.GetType( "MySql.Data.MySqlClient.MySqlDataAdapter" ), new object[] { cmd } ) as DbDataAdapter);
        }

        private static IDbDataParameter getMySqlParameter( String parameterName, Object parameterValue ) {
            return (rft.GetInstance( mySqlDataAssembly.GetType( "MySql.Data.MySqlClient.MySqlParameter" ), new object[] { parameterName, parameterValue } ) as IDbDataParameter);
        }

        public static Type mySqlCommandType {
            get { return mySqlDataAssembly.GetType( "MySql.Data.MySqlClient.MySqlCommand" ); }
        }

        public static Type mySqlConnectionType {
            get { return mySqlDataAssembly.GetType( "MySql.Data.MySqlClient.MySqlConnection" ); }
        }

        public static Assembly mySqlDataAssembly {
            get { return Assembly.Load( "MySql.Data" ); }
        }

    }

}
