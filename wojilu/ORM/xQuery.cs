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
    /// 泛型查询对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xQuery<T> {

        private Query _q;

        public xQuery( Query query ) {
            _q = query;
        }

        /// <summary>
        /// 给查询条件中的参数赋值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val">参数值</param>
        /// <returns></returns>
        public xQuery<T> set( String name, Object val ) {
            _q.set( name, val );
            return this;
        }

        /// <summary>
        /// 返回查询的所有结果
        /// </summary>
        /// <returns></returns>
        public List<T> list() {
            return this.list( -1 );
        }

        /// <summary>
        /// 返回符合查询条件的前 n 条结果
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> list( int count ) {
            IList list = _q.list( count );
            return db.getResults<T>( list );
        }

        /// <summary>
        /// 返回符合查询条件的第一条结果
        /// </summary>
        /// <returns></returns>
        public T first() {
            Object obj = _q.first();
            return (T)obj;
        }

        /// <summary>
        /// 统计符合查询条件的结果数量
        /// </summary>
        /// <returns></returns>
        public int count() {
            return _q.count();
        }

        /// <summary>
        /// (本方法不建议使用)只查询指定的属性，本来用于提高性能，但和缓存会起冲突。
        /// </summary>
        /// <param name="propertyString"></param>
        /// <returns></returns>
        public xQuery<T> select( String propertyString ) {
            _q.select( propertyString );
            return this;
        }

        /// <summary>
        /// 将所有结果对象的某个实体属性封装成集合返回
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public List<TData> listChildren<TData>( String propertyName ) {
            IList list = _q.listChildren( propertyName );
            return db.getResults<TData>( list );
        }

        /// <summary>
        /// 获取所有结果的某个属性的字符串集合，比如 get( "Id" ) 返回 "2, 7, 16, 25"
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public String get( String propertyName ) {
            return _q.get( propertyName );
        }

    }

}
