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
using System.Data.Common;

namespace wojilu.Data {

    /// <summary>
    /// 数据工厂抽象基类，可以不用考虑数据库差异而获取 Connection, Command, DataAdapter
    /// </summary>
    public abstract class DbFactoryBase {

        protected void checkOpen() {
            if (this.cn.State != ConnectionState.Open) {
                this.cn.Open();
            }
        }

        public static DbFactoryBase Instance( String connectionString ) {
            DbFactoryBase result = Instance( DbTypeChecker.GetDatabaseType( connectionString ) );
            IDbConnection cn = result.GetConnection( connectionString );
            result.cn = cn;
            return result;
        }

        public static DbFactoryBase Instance( IDbConnection cn ) {
            DbFactoryBase result = Instance( DbTypeChecker.GetDatabaseType( cn ) );
            result.cn = cn;
            return result;
        }


        public static DbFactoryBase Instance( IDbCommand cmd ) {
            DbFactoryBase result = Instance( DbTypeChecker.GetDatabaseType( cmd ) );
            result.cmd = cmd;
            return result;
        }


        public static DbFactoryBase Instance( DatabaseType dbtype ) {

            if (dbtype == DatabaseType.SqlServer) return new MsSqlDbFactory();
            if (dbtype == DatabaseType.SqlServer2000) return new MsSqlDbFactory();
            if (dbtype == DatabaseType.Access) return new AccessFactory();
            if (dbtype == DatabaseType.MySql) return new MysqlFactory();
            if (dbtype == DatabaseType.Oracle) return new OracleFactory();

            throw new Exception( lang.get( "dbNotSupport" ) );
        }

        public abstract IDbConnection GetConnection( String connectionString );
        public abstract IDbCommand GetCommand( String CommandText );
        public IDbCommand GetCommand() { return this.GetCommand( null ); }
        internal abstract IDatabaseChecker GetDatabaseChecker();
        public abstract IDatabaseDialect GetDialect();
        public abstract Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue );
        public abstract DbDataAdapter GetAdapter();
        public abstract DbDataAdapter GetAdapter( String CommandText );


        protected IDbConnection cn;
        protected IDbCommand cmd;


        protected virtual void setTransaction( IDbCommand cmd ) {
            DbContext.setTransaction( cmd );
        }

        protected virtual Object processValue( Object parameterValue ) {

            if (parameterValue is DateTime) {
                DateTime time = (DateTime)parameterValue;
                if ((time < new DateTime( 1800, 1, 1 )) || (time > new DateTime( 9000, 1, 1 ))) {
                    parameterValue = DateTime.Now;
                }
            }
            else if (parameterValue is string) {
                parameterValue = parameterValue.ToString().Trim();
            }
            else if (parameterValue is int && (int)parameterValue == 0) {
                parameterValue = Convert.ToInt32( parameterValue );
            }

            return parameterValue;

        }




    }

}
