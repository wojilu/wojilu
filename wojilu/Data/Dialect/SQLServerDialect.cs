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
    /// sqlserver 特殊语法处理器
    /// </summary>
    public class SQLServerDialect : IDatabaseDialect {
        public String GetConnectionItem( String connectionString, ConnectionItemType connectionItem ) {
            String str = connectionItem.ToString().ToLower().Replace( "userid", "uid" ).Replace( "password", "pwd" );
            String[] strArray = connectionString.ToLower().Split( new char[] { ';' } );
            foreach (String item in strArray) {
                if (item.Trim().ToLower().StartsWith( str )) {
                    return item.Replace( str, "" ).Replace( "=", "" ).Replace( " ", "" );
                }
            }
            return null;
        }

        public String GetTimeQuote() {
            return "'";
        }

        public String GetLimit( String sql, int limit ) {
            return sql.ToLower().Replace( "select ", "select top " + limit + " " );
        }

        public String GetLimit( String sql ) {
            return sql;
        }

        public String GetParameter( String parameterName ) {
            return ("@" + parameterName);
        }

        public String GetParameterAdder( String parameterName ) {
            return ("@" + parameterName);
        }

        public String Top {
            get { return "top"; }
        }

        public string GetLeftQuote() {
            return "[";
        }

        public string GetRightQuote() {
            return "]";
        }

    }
}

