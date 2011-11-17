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

    internal class TimestampKey {

        public static String getTable( string typeFullName ) {
            return "timestamp_table_" + typeFullName;
        }

        public static string getObject( IEntity obj ) {
            return "timestamp_object_" + obj.GetType().FullName + "_" + obj.Id;
        }

        public static string getList( string sqlCacheKey ) {
            return "timestamp_list_" + sqlCacheKey;
        }

        public static string getCount( Type t ) {
            return "timestamp_count_" + t.FullName;
        }

        public static string getCount( Type t, String condition ) {
            return "timestamp_count_" + t.FullName + "_" + condition;
        }

    }

}
