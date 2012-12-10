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

namespace wojilu {

    /// <summary>
    /// 从内存数据库中查询数据
    /// </summary>
    /// <remarks>
    /// 数据持久化在 /framework/data/ 目录下，以json格式存储。加载之后常驻内存。
    /// 特点：直接从内存中检索，速度相当于 Hashtable。插入和更新较慢(相对而言)，因为插入和更新会在内存中重建索引。
    /// </remarks>
    public class cdb {

        /// <summary>
        /// 查询类型 T 的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>返回所有数据的列表</returns>
        public static List<T> findAll<T>() where T : CacheObject {
            IList list = MemoryDB.FindAll( typeof( T ) );
            return db.getResults<T>( list );
        }

        /// <summary>
        /// 根据 id 查询某条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>返回某条数据</returns>
        public static T findById<T>( int id ) where T : CacheObject {
            Object obj = MemoryDB.FindById( typeof( T ), id );
            return (T)obj;
        }

        /// <summary>
        /// 根据名称查询数据，因为已经根据名称做了索引，所以速度很快。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns>返回数据列表</returns>
        public static List<T> findByName<T>( String name ) where T : CacheObject {
            return findBy<T>( "Name", name );
        }

        /// <summary>
        /// 根据 id 获取对象的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String findNameById<T>( int id ) where T : CacheObject {
            T obj = cdb.findById<T>( id );
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
        public static List<T> findBy<T>( String propertyName, Object val ) where T : CacheObject {
            findAll<T>();
            IList list = MemoryDB.FindBy( typeof( T ), propertyName, val );
            return db.getResults<T>( list );
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
        public static DataPage<T> findPage<T>( String propertyName, Object val, int pageSize ) where T : CacheObject {
            List<T> list = findBy<T>( propertyName, val );
            DataPage<T> page = DataPage<T>.GetPage( list, pageSize );
            return page;
        }

        /// <summary>
        /// 查询分页后的数据列表。不用提供当前页信息，因为在web环境中，框架会自动获取当前页面。
        /// 分页是在内存中进行的，也就是先查询内存中所有记录，然后根据当前页和 pageSize 获取特定页面的数据。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize">每页需要显示的数据量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<T> findPage<T>( int pageSize ) where T : CacheObject {
            
            List<T> list = findAll<T>();
            DataPage<T> page = DataPage<T>.GetPage( list, pageSize );
            return page;
        }


        /// <summary>
        /// 插入数据，并对所有属性做索引，速度较慢。新插入的数据会被同步持久化到磁盘。
        /// </summary>
        public static void insert( CacheObject obj ) {
            MemoryDB.Insert( obj );
        }

        /// <summary>
        /// 插入时，只针对特定属性做索引，提高速度
        /// </summary>
        /// <param name="propertyName">需要做索引的属性</param>
        /// <param name="pValue">属性的值</param>
        public static void insertByIndex( CacheObject obj, String propertyName, Object pValue ) {
            MemoryDB.InsertByIndex( obj, propertyName, pValue );
        }

        /// <summary>
        /// 更新对象，并将对象同步持久化的磁盘，同时更新索引
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Result update( CacheObject obj ) {
            return MemoryDB.Update( obj );
        }

        /// <summary>
        ///  更新数据（不持久化，也不做索引）
        /// </summary>
        /// <returns></returns>
        public static Result updateNoIndex( CacheObject obj ) {
            return new Result();
        }

        /// <summary>
        /// 从内存中删除数据，并同步磁盘中内容。
        /// </summary>
        /// <param name="obj"></param>
        public static void delete( CacheObject obj ) {
            MemoryDB.Delete( obj );
        }

    }

}
