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

namespace wojilu.Data {

    /// <summary>
    /// mysql 特殊语法处理器
    /// </summary>
    public class MysqlDialect : IDatabaseDialect {

        public String GetConnectionItem( String connectionString, ConnectionItemType connectionItem ) {
            return getConnectionItem( connectionString, connectionItem );
        }


        public static String getConnectionItem( String connStr, ConnectionItemType connectionItemType ) {

            if (strUtil.IsNullOrEmpty( connStr )) throw new Exception( "mysql connection string is empty" );

            String[] arrItems = connStr.ToLower().Split( ';' );

            foreach (String item in arrItems) {

                if (strUtil.IsNullOrEmpty( item )) continue;
                String[] arrPair = item.Split( '=' );
                if (arrPair.Length != 2) continue;

                String key = arrPair[0].Trim();
                String val = arrPair[1].Trim();

                if (keyEqual( key, connectionItemType )) return val;

            }

            return "";
        }

        private static Boolean keyEqual( String key, ConnectionItemType connectionItemType ) {

            if (connectionItemType == ConnectionItemType.Server) return key == "server";
            if (connectionItemType == ConnectionItemType.Database) return key == "database";
            if (connectionItemType == ConnectionItemType.UserId) return key == "user" || key == "uid" || key == "user id";
            if (connectionItemType == ConnectionItemType.Password) return key == "password" || key == "pwd";

            return false;
        }

        public String GetLimit( String sql, int limit ) {
            return (sql + " limit " + limit);
        }

        public String GetLimit( String rSql ) {
            if (rSql == null) return null;

            String sql = rSql.ToLower();
            if (sql.ToLower().IndexOf( " top " ) > 0) {

                // selec top 10 * from
                // 10 * from
                String rMainSql = sql.Split( new String[] { " top " }, StringSplitOptions.None )[1].Trim();

                String[] arrItem = rMainSql.Split( ' ' );
                int limit = cvt.ToInt( arrItem[0] );

                String mainSql = strUtil.TrimStart( rMainSql, arrItem[0] );


                return this.GetLimit( "select " + mainSql, limit );
            }

            return sql;
        }

        public String GetTimeQuote() {
            return "'";
        }

        public String GetParameter( String parameterName ) {
            return ("@" + parameterName);
        }

        public String GetParameterAdder( String parameterName ) {
            return ("@" + parameterName);
        }

        public String Top {
            get { return "limit"; }
        }

        public string GetLeftQuote() {
            return "`";
        }

        public string GetRightQuote() {
            return "`";
        }

    }
}

