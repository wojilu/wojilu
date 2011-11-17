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

namespace wojilu.ORM.Caching {


    internal class CacheKey {

        public static String getObject( Type t, int id ) {
            return getObject( t.FullName, id );
        }

        public static String getObject( String typeFullName, int id ) {
            return "__object_" + typeFullName + "_" + id.ToString();
        }

        public static String getObjectAll( Type t ) {
            return "__objectAll_" + t.FullName + "_all";
        }

        public static String getQuery( Type t, String sql, Dictionary<String, Object> parameters ) {
            String str = sql + " paramValue : ";
            foreach (KeyValuePair<String, Object> pair in parameters) {
                str = str + " \"" + cvt.ToNotNull( pair.Value ) + "\", ";
            }

            return "__objectQueryList_" + t.FullName + "_" + str.Trim().TrimEnd( ',' );
        }

        public static string getPageList( Type t, string condition, int pageSize, int current ) {
            return "__objectPageList_" + t.FullName + "_ids_" + condition + "_pagesize" + pageSize + "_" + current;
        }


        public static String getPagerInfoKey( String key ) {
            return "__objectPagebar_" + key;
        }

        public static string getCountKey( Type t ) {
            return "__objectCount_" + t.FullName;
        }


        public static string getCountKey( Type t, string condition ) {
            return "__objectCount_" + t.FullName + "_" + condition;
        }

        public static String getSqlKey( String sql ) {
            return "__objectSql_"+ sql;
        }



    }

}
