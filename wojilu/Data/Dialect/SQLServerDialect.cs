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

namespace wojilu.Data {

    /// <summary>
    /// sqlserver 特殊语法处理器
    /// </summary>
    public class SQLServerDialect : IDatabaseDialect {

        public String GetConnectionItem( String connectionString, ConnectionItemType connectionItem ) {

            if (connectionString == null) throw new NullReferenceException( "connectionString" );

            String[] arrItems = connectionString.Split( ';' );
            Dictionary<String,String> dic = new Dictionary<String,String>();
            foreach (String item in arrItems) {
                if (strUtil.IsNullOrEmpty( item )) continue;

                String[] arr = item.Trim().Split( '=' );
                if (arr.Length != 2) continue;

                String akey = getKey( arr[0].Trim() );
                if (akey == null) continue;

                dic[akey] = arr[1].Trim();
            }

            String result = null;
            dic.TryGetValue( getEnumKey( connectionItem ), out result );

            return result;
        }

        private String getKey( string key ) {

            key = key.ToLower();

            if (key == "server") return getEnumKey( ConnectionItemType.Server );
            if (key == "data source") return getEnumKey( ConnectionItemType.Server );
            if (key == "addr") return getEnumKey( ConnectionItemType.Server );
            if (key == "address") return getEnumKey( ConnectionItemType.Server );
            if (key == "network address") return getEnumKey( ConnectionItemType.Server );

            if (key == "uid") return getEnumKey( ConnectionItemType.UserId );
            if (key == "user id") return getEnumKey( ConnectionItemType.UserId );
            if (key == "user") return getEnumKey( ConnectionItemType.UserId );

            if (key == "pwd") return getEnumKey( ConnectionItemType.Password );
            if (key == "password") return getEnumKey( ConnectionItemType.Password );

            if (key == "database") return getEnumKey( ConnectionItemType.Database );
            if (key == "initial catalog") return getEnumKey( ConnectionItemType.Database );

            if (key == "trusted_connection") return getEnumKey( ConnectionItemType.IsTrusted );
            if (key == "integrated security") return getEnumKey( ConnectionItemType.IsTrusted );

            return null;
        }

        private String getEnumKey( ConnectionItemType connectionItem ) {
            if (connectionItem == ConnectionItemType.Server) return "server";
            if (connectionItem == ConnectionItemType.UserId) return "userid";
            if (connectionItem == ConnectionItemType.Password) return "password";
            if (connectionItem == ConnectionItemType.Database) return "database";
            if (connectionItem == ConnectionItemType.IsTrusted) return "istrusted";
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

