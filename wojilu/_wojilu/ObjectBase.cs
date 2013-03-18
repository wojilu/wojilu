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
using wojilu.ORM;
using wojilu.Reflection;
using wojilu.Serialization;

namespace wojilu {

    public class ExtData : Dictionary<String, String> {
        public String show {
            get { return this["show"]; }
            set { this["show"] = value; }
        }
        public String edit {
            get { return this["edit"]; }
            set { this["edit"] = value; }
        }
        public String delete {
            get { return this["delete"]; }
            set { this["delete"] = value; }
        }
    }

    /// <summary>
    /// 所有ORM中的领域模型都需要继承的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ObjectBase<T> : IEntity, IComparable where T : ObjectBase<T> {

        private int _id;

        /// <summary>
        /// 对象的 id
        /// </summary>
        public int Id {
            get { return _id; }
            set { this.setId( value ); _id = value; }
        }

        protected virtual void setId( int id ) {
        }

        private ExtData _extData;

        [NotSave]
        public ExtData data {
            get {
                if (_extData == null) _extData = new ExtData();
                return _extData;
            }
            set {
                _extData = value;
            }
        }

        //----------------------------------------------------------------------------

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public static List<T> findAll() { return db.findAll<T>(); }

        /// <summary>
        /// 根据 id 查询对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T findById( int id ) { return db.findById<T>( id ); }

        /// <summary>
        /// 统计所有的数据量
        /// </summary>
        /// <returns></returns>
        public static int count() { return db.count<T>(); }

        /// <summary>
        /// 根据条件统计数据量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int count( String condition ) { return db.count<T>( condition ); }

        /// <summary>
        /// 根据查询条件，返回一个查询对象。一般用于参数化查询。
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>返回查询对象xQuery，可以进一步参数化赋值，并得到结果</returns>
        public static xQuery<T> find( String condition ) { return db.find<T>( condition ); }

        /// <summary>
        /// 根据查询条件，返回分页数据集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataPage<T> findPage( String condition ) { return db.findPage<T>( condition ); }

        /// <summary>
        /// 根据查询条件和每页数量，返回分页数据集合
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public static DataPage<T> findPage( String condition, int pageSize ) { return db.findPage<T>( condition, pageSize ); }

        /// <summary>
        /// 直接使用 sql 语句查询，返回对象列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> findBySql( String sql ) { return db.findBySql<T>( sql ); }

        /// <summary>
        /// 将对象插入数据库
        /// </summary>
        /// <returns>返回一个结果对象 Result。如果发生错误，则 Result 中包含错误信息</returns>
        public Result insert() { return db.insert( this ); }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <returns>返回一个结果对象 Result。如果发生错误，则 Result 中包含错误信息</returns>
        public Result update() { return db.update( this ); }

        /// <summary>
        /// 只修改对象的某个特定属性
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void update( String propertyName ) { db.update( this, propertyName ); }

        /// <summary>
        /// 只修改对象的特定属性
        /// </summary>
        /// <param name="arrPropertyName">需要修改的属性的数组</param>
        public void update( String[] arrPropertyName ) { db.update( this, arrPropertyName ); }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <returns>返回受影响的行数</returns>
        public int delete() { return db.delete( this ); }

        /// <summary>
        /// 批量更新对象
        /// </summary>
        /// <param name="action">更新的操作</param>
        /// <param name="condition">更新的条件</param>
        public static void updateBatch( String action, String condition ) { db.updateBatch<T>( action, condition ); }

        /// <summary>
        /// 根据 id 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回受影响的行数</returns>
        public static int delete( int id ) { return db.delete<T>( id ); }

        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="condition">删除条件</param>
        /// <returns>返回受影响的行数</returns>
        public static int deleteBatch( String condition ) { return db.deleteBatch<T>( condition ); }

        //------------------------------------- 以下实例方法 --------------------------------------------

        /// <summary>
        /// 根据属性名称获取属性的值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public Object get( String propertyName ) {
            EntityInfo ei = getEntityInfo();
            if (propertyName.IndexOf( "." ) < 0) {
                EntityPropertyInfo ep = ei.GetProperty( propertyName );
                if (ep == null) return null;
                return ep.GetValue( this );
            }
            String[] arrItems = propertyName.Split( new char[] { '.' } );
            Object result = null;
            ObjectBase<T> obj = this;
            for (int i = 0; i < arrItems.Length; i++) {
                if (i < (arrItems.Length - 1)) {
                    obj = (ObjectBase<T>)obj.get( arrItems[i] );
                }
                else {
                    result = obj.get( arrItems[i] );
                }
            }
            return result;
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性的值</param>
        public void set( String propertyName, Object propertyValue ) {
            getEntityInfo().GetProperty( propertyName ).SetValue( this, propertyValue );
        }

        //-------------------------------------------------------------------------

        /// <summary>
        /// 获取扩展属性内部某项的值
        /// </summary>
        /// <param name="propertyName">扩展属性名称</param>
        /// <param name="key">扩展属性内部某项的 key</param>
        /// <returns></returns>
        public Object getExt( String propertyName, String key ) {
            Dictionary<String, Object> dic = this.getExtDic( propertyName );
            Object val = null;
            dic.TryGetValue( key, out val );
            return val;
        }

        /// <summary>
        /// 获取扩展属性本身的值
        /// </summary>
        /// <param name="propertyName">扩展属性名称</param>
        /// <returns></returns>
        public Dictionary<String, Object> getExtDic( String propertyName ) {
            Object pvalue = this.get( propertyName );
            Dictionary<String, Object> dic = new Dictionary<string, Object>();
            if (pvalue == null || strUtil.IsNullOrEmpty( pvalue.ToString() )) return dic;
            try {
                dic = JSON.ToDictionary( pvalue.ToString() );
            }
            catch {
            }
            return dic;
        }

        /// <summary>
        /// 给扩展属性内部某项赋值
        /// </summary>
        /// <param name="propertyName">扩展属性名称</param>
        /// <param name="key">扩展属性内部某项的 key</param>
        /// <param name="val">扩展属性内部某项的 val</param>
        public void setExt( String propertyName, String key, String val ) {

            Dictionary<String, Object> dic = this.getExtDic( propertyName );
            dic[key] = val;
            this.setExtDic( propertyName, dic );
        }

        /// <summary>
        /// 给扩展属性本身的赋值
        /// </summary>
        /// <param name="propertyName">扩展属性名称</param>
        /// <param name="dic">扩展属性的值</param>
        public void setExtDic( String propertyName, Dictionary<String, Object> dic ) {
            this.set( propertyName, JSON.DicToString( dic ) );
            this.update( propertyName );
        }

        //-------------------------------------------------------------------------

        private EntityInfo _entity;

        private EntityInfo getEntityInfo() {
            if (_entity == null) {
                _entity = Entity.GetInfo( this );
            }
            return _entity;
        }

        /// <summary>
        /// 排序方法(根据Id大小排序)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo( Object obj ) {
            return ((ObjectBase<T>)obj).Id.CompareTo( Id );
        }

    }

}
