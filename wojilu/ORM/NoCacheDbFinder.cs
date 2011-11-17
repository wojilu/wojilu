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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace wojilu.ORM {

    /// <summary>
    /// 绕过缓存，直接访问数据库
    /// </summary>
    public class NoCacheDbFinder {

        public static List<T> findAll<T>() {
            ObjectInfo state = new ObjectInfo( typeof( T ) );
            state.includeAll();
            IList list = ObjectDB.FindAll( state );
            return db.getResults<T>( list );
        }

        public T findById<T>( int id ) {
            if (id < 0) return default( T );
            Object obj = ObjectDB.FindById( id, new ObjectInfo(typeof(T))  );
            return (T)obj;
        }


    }

}
