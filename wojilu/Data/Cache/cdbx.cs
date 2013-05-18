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

using wojilu.Data;
using wojilu.ORM;
using wojilu.Web;

namespace wojilu.Data {

    /// <summary>
    /// 从内存数据库中查询数据
    /// </summary>
    /// <remarks>
    /// 数据持久化在 /framework/data/ 目录下，以json格式存储。加载之后常驻内存。
    /// 特点：直接从内存中检索，速度相当于 Hashtable。插入和更新较慢(相对而言)，因为插入和更新会在内存中重建索引。
    /// </remarks>
    public class cdbx {

        /// <summary>
        /// 查询类型 T 的所有数据
        /// </summary>
        public static List<CacheObject> findAll( Type t ) {
            IList list = MemoryDB.FindAll( t );
            return db.getResults<CacheObject>( list );
        }

        /// <summary>
        /// 根据 id 查询某条数据
        /// </summary>
        public static CacheObject findById( Type t, int id ) {
            return MemoryDB.FindById( t, id );
        }

        /// <summary>
        /// 根据名称查询数据，因为已经根据名称做了索引，所以速度很快。
        /// </summary>
        public static List<CacheObject> findByName( Type t, String name ) {
            return findBy( t, "Name", name );
        }

        /// <summary>
        /// 根据 id 获取对象的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String findNameById( Type t, int id ) {
            CacheObject obj = findById( t, id );
            if (obj == null) return "";
            return obj.Name;
        }

        /// <summary>
        /// 根据属性查询数据。框架已经给对象的所有属性做了索引。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="val">属性的值</param>
        /// <returns>返回数据列表</returns>
        public static List<CacheObject> findBy( Type t, String propertyName, Object val ) {
            findAll( t );
            IList list = MemoryDB.FindBy( t, propertyName, val );
            return db.getResults<CacheObject>( list );
        }

        /// <summary>
        /// 查询分页后的数据列表。不用提供当前页信息，因为在web环境中，框架会自动获取当前页面。
        /// 分页是在内存中进行的，也就是先查询内存中所有记录，然后根据当前页和 pageSize 获取特定页面的数据。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="val">属性的值</param>
        /// <param name="pageSize">每页需要显示的数据量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<CacheObject> findPage( Type t, String propertyName, Object val, int pageSize ) {
            IList xlist = findBy( t, propertyName, val );
            List<CacheObject> list = db.getResults<CacheObject>( xlist );
            DataPage<CacheObject> page = DataPage<CacheObject>.GetPage( list, pageSize );
            return page;
        }

        /// <summary>
        /// 查询分页后的数据列表。不用提供当前页信息，因为在web环境中，框架会自动获取当前页面。
        /// 分页是在内存中进行的，也就是先查询内存中所有记录，然后根据当前页和 pageSize 获取特定页面的数据。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页需要显示的数据量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<CacheObject> findPage( Type t, int pageSize ) {

            List<CacheObject> list = findAll( t );
            DataPage<CacheObject> page = DataPage<CacheObject>.GetPage( list, pageSize );
            return page;
        }



    }

}
