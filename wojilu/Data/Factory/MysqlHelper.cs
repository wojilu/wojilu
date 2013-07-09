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
using wojilu.Reflection;

namespace wojilu.Data {


    public interface IMysqlHelper {

        Type GetMySqlCommandType();
        Type GetMySqlConnectionType();

        IDbConnection GetMySqlConnection( String connectionString );
        IDbCommand GetMySqlCommand();
        DbDataAdapter GetMySqlDataAdapter( Object cmd );
        IDbDataParameter GetMySqlParameter( String parameterName, Object parameterValue );

    }

    public class MysqlHelperLoader {

        private static IMysqlHelper _mysqlHelper = loadMysqlHelper();

        private static IMysqlHelper loadMysqlHelper() {

            String code = makeCode();

            Dictionary<String, Assembly> asmList = getMysqlAsmList();
            Assembly asm = CodeDomHelper.CompileCode( code, asmList, null );

            return asm.CreateInstance( "wojilu.Data.MysqlHelper" ) as IMysqlHelper;
        }

        public static IMysqlHelper GetHelper() {
            return _mysqlHelper;
        }

        private static Dictionary<String, Assembly> getMysqlAsmList() {

            Assembly mysqlAssembly = null;
            try {
                mysqlAssembly = Assembly.Load( "mysql.data" );
            }
            catch  {                    
                throw new Exception( "缺少 mysql.data.dll 文件。请从 lib 目录复制到 bin 中" );
            }

            Dictionary<String, Assembly> ret = new Dictionary<String, Assembly>();
            foreach (KeyValuePair<String, Assembly> kv in ObjectContext.Instance.AssemblyList) {
                ret.Add( kv.Key, kv.Value );
            }

            ret.Add( "mysql.data", mysqlAssembly );

            return ret;
        }

        private static string makeCode() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "using System;" );
            sb.AppendLine( "using System.Text;" );
            sb.AppendLine( "using System.Data;" );
            sb.AppendLine( "using System.Data.Common;" );
            sb.AppendLine( "using MySql.Data.MySqlClient;" );

            sb.AppendLine( "namespace wojilu.Data {" );
            sb.Append( @"public class MysqlHelper : IMysqlHelper {

        public Type GetMySqlCommandType() {
            return typeof( MySql.Data.MySqlClient.MySqlCommand );
        }

        public Type GetMySqlConnectionType() {
            return typeof( MySql.Data.MySqlClient.MySqlConnection );
        }

        public IDbCommand GetMySqlCommand() {
            return new MySql.Data.MySqlClient.MySqlCommand();
        }

        public IDbConnection GetMySqlConnection( String connectionString ) {
            return new MySql.Data.MySqlClient.MySqlConnection( connectionString );
        }

        public DbDataAdapter GetMySqlDataAdapter( Object cmd ) {
            if (cmd == null) throw new ArgumentNullException( ""cmd"" );
            return new MySql.Data.MySqlClient.MySqlDataAdapter( cmd as MySqlCommand );
        }

        public IDbDataParameter GetMySqlParameter( String parameterName, Object parameterValue ) {
            return new MySql.Data.MySqlClient.MySqlParameter( parameterName, parameterValue );
        }

    }" );

            sb.AppendLine( "}" );

            return sb.ToString();
        }

    }


}
