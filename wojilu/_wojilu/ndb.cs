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
using System.Text;
using wojilu.ORM;
using wojilu.ORM.Caching;

namespace wojilu {

    /// <summary>
    /// 集中了对象的常用 CRUD (读取/插入/更新/删除) 操作方法，非泛型实现。主要用于某些不能使用泛型的场合，不太常用。
    /// </summary>
    public class ndb {

        /// <summary>
        /// 根据 id 查询对象
        /// </summary>
        /// <param name="t">对象的类型</param>
        /// <param name="id">对象的 id</param>
        /// <returns></returns>
        public static IEntity findById( Type t, int id ) {

            if (id < 0) return null;

            IEntity objCache = ObjectPool.FindOne( t, id );
            if (objCache == null) {
                ObjectInfo state = new ObjectInfo( t );
                objCache = ObjectDB.FindById( id, state );
                ObjectPool.Add( objCache );
            }
            return objCache;
        }

        /// <summary>
        /// 查询 t 类型对象的所有数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IList findAll( Type t ) {
            ObjectInfo state = new ObjectInfo( t );
            state.includeAll();
            IList objList = ObjectPool.FindAll( t );
            if (objList == null) {
                objList = ObjectDB.FindAll( state );
                ObjectPool.AddAll( t, objList );
            }
            return objList;
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">查询对象</param>
        /// <returns>返回查询对象Query，可以进一步参数化赋值，并得到结果</returns>
        public static Query find( Type t, String condition ) {
            ObjectInfo state = new ObjectInfo( t );
            return ObjectDB.Find( state, condition );
        }

        /// <summary>
        /// 根据查询条件，返回分页数据集合
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">查询条件</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static IPageList findPage( Type t, String condition ) {
            return findPage( t, condition, -1 );
        }

        /// <summary>
        /// 根据查询条件，返回分页数据集合
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static IPageList findPage( Type t, String condition, int pageSize ) {

            ObjectInfo state = new ObjectInfo( t );
            state.includeAll();
            if (pageSize > 0) state.Pager.setSize( pageSize );

            IList list = ObjectDB.FindPage( state, condition );
            IPageList result = new DataPageInfo();
            result.Results = list;
            result.PageCount = state.Pager.PageCount;
            result.RecordCount = state.Pager.RecordCount;
            result.Size = pageSize>0 ? pageSize: state.Pager.getSize();
            result.PageBar = state.Pager.PageBar;
            result.Current = state.Pager.getCurrent();
            return result;
        }

        /// <summary>
        /// 根据 sql 语句，查询对象
        /// </summary>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <returns>返回对象列表</returns>
        public static Object findBySql( Type t, String sql ) {

            IList objList = ObjectPool.FindBySql( sql, t );
            if (objList == null) {
                objList = ObjectDB.FindBySql( sql, t );
                ObjectPool.AddSqlList( sql, objList );
            }
            return objList;
        }

        /// <summary>
        /// 统计 t 类型对象的所有数据量
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int count( Type t ) {
            return ObjectDB.Count( t );
        }

        /// <summary>
        /// 根据条件统计数据量
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        public static int count( Type t, String condition ) {
            return ObjectDB.Count( t, condition );
        }

        /// <summary>
        /// 根据 id 删除对象
        /// </summary>
        /// <param name="t"></param>
        /// <param name="objId">对象 id</param>
        /// <returns>返回受影响的行数</returns>
        public static int delete( Type t, int objId ) {
            int num = ObjectDB.Delete( t, objId );
            ObjectPool.Delete( t, objId );
            return num;
        }
    }

}
