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
using System.Web;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.Data {

    /// <summary>
    /// access 特殊语法处理器
    /// </summary>
    [Serializable]
    public class AccessDialect : IDatabaseDialect {

        public String GetConnectionItem( String connectionString, ConnectionItemType connectionItem ) {
            String str = connectionItem.ToString().ToLower().Replace( "database", "data source" ).Replace( "userid", "user id" );
            String[] arrItem = connectionString.ToLower().Split( new char[] { ';' } );
            foreach (String item in arrItem) {
                if (item.Trim().ToLower().StartsWith( str )) {
                    return item.Replace( str, "" ).Replace( "=", "" ).Replace( " ", "" );
                }
            }
            return null;
        }

        public String GetLimit( String sql, int limit ) {
            return sql.ToLower().Replace( "select ", "select top " + limit + " " );
        }

        public String GetLimit( String sql) {
            return sql;
        }

        public String GetTimeQuote() {
            return "#";
        }

        public String GetParameter( String parameterName ) {
            return "?";
        }

        public String GetParameterAdder( String parameterName ) {
            return ("@" + parameterName);
        }

        public static String MapPath( String connectionString ) {
            if (SystemInfo.IsWeb==false) {
                return connectionString;
            }
            String connectionItem = new AccessDialect().GetConnectionItem( connectionString, ConnectionItemType.Database );
            String newValue = PathHelper.Map( connectionItem );
            return connectionString.Replace( connectionItem, newValue );
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

