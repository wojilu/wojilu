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
using wojilu.Log;
using wojilu.ORM.Operation;
using wojilu.ORM.Caching;

namespace wojilu.ORM {

    /// <summary>
    /// 查询对象
    /// </summary>
    public class Query {

        private static readonly ILog logger = LogManager.GetLogger( typeof( Query ) );

        private Dictionary<String, Object> _namedParameters = new Dictionary<String, object>( 10 );

        private String _selectItems = "*";
        private String _whereStr;

        private ObjectInfo _state;

        public Query( String queryString, ObjectInfo state ) {
            _whereStr = queryString.Trim();
            _state = state;
        }


        public String QueryString {
            get { return _whereStr; }
        }

        /// <summary>
        /// (本方法不建议使用)只查询指定的属性，本来用于提高性能，但和缓存会起冲突。
        /// </summary>
        /// <param name="propertyString"></param>
        /// <returns></returns>
        public Query select( String propertyString ) {
            _selectItems = propertyString;
            return this;
        }

        /// <summary>
        /// 给查询条件中的参数赋值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="val">参数值</param>
        /// <returns></returns>
        public Query set( String name, Object val ) {
            _namedParameters[name] = val;
            return this;
        }

        /// <summary>
        /// 统计符合查询条件的结果数量
        /// </summary>
        /// <returns></returns>
        public int count() {
            return select( "Id" ).list().Count;
        }

        /// <summary>
        /// 返回符合查询条件的第一条结果
        /// </summary>
        /// <returns></returns>
        public IEntity first() {
            IList results = list( 1 );
            if ((results != null) && (results.Count > 0)) {
                return (results[0] as IEntity);
            }
            return null;
        }

        /// <summary>
        /// 获取所有结果的某个属性的字符串集合，比如 get( "Id" ) 返回 "2, 7, 16, 25"
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public String get( String propertyName ) {
            return getPropertyValueStringAll( select( propertyName ).list(), propertyName );
        }

        /// <summary>
        /// 返回查询的所有结果
        /// </summary>
        /// <returns></returns>
        public IList list() {
            return list( -1 );
        }

        /// <summary>
        /// 返回符合查询条件的前 n 条结果
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList list( int count ) {

            processQueryString();
            SqlBuilder builder = new SqlBuilder( _state.EntityInfo.Type );
            String sql = builder.GetFindConditionSql( _selectItems, _whereStr, _namedParameters, count );

            // cache
            IList list = ObjectPool.FindByQuery( _state.EntityInfo.Type, sql, _namedParameters );
            if (list != null) return list;

            logger.Info( LoggerUtil.SqlPrefix + "[FindBy]" + sql );

            ConditionInfo conditionInfo = new ConditionInfo();
            conditionInfo.Type = _state.EntityInfo.Type;
            conditionInfo.State = _state;
            conditionInfo.SelectedItem = _selectItems;
            conditionInfo.WhereString = _whereStr;
            conditionInfo.Parameters = _namedParameters;
            conditionInfo.Count = count;
            conditionInfo.Sql = sql;

            IList parentResults = FindByOperation.Find( conditionInfo );
            if (parentResults.Count > 0 && _state.EntityInfo.ChildEntityList.Count > 0 && _state.IsFindChild) {
                parentResults = findFromChild( parentResults );
            }

            // cache
            if ("*".Equals( _selectItems ))
                ObjectPool.AddQueryList( _state.EntityInfo.Type, sql, _namedParameters, parentResults );

            return parentResults;
        }

        /// <summary>
        /// 将所有结果对象的某个实体属性封装成集合返回
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IList listChildren( String propertyName ) {
            return listChildren( propertyName, -1 );
        }

        /// <summary>
        /// 将所有结果对象的某个实体属性封装成集合返回
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList listChildren( String propertyName, int count ) {
            IList mylist = list( count );
            IList results = new ArrayList();
            for (int i = 0; i < mylist.Count; i++) {
                results.Add( ((IEntity)mylist[i]).get( propertyName ) );
            }
            return results;
        }

        private void processQueryString() {
            if (_whereStr.ToLower().StartsWith( "order " )) {
                _whereStr = " " + _whereStr;
            }
            else if (strUtil.HasText( _whereStr )) {

                // rubywu http://www.wojilu.com/Forum1/Post/9415
                if (_whereStr.ToLower().IndexOf( " order " ) >= 0) {
                    _whereStr = " and " + _whereStr;

                //if (_whereStr.ToLower().IndexOf( " order" ) >= 0) {
                //    String oldValue = _whereStr.Substring( 0, _whereStr.ToLower().IndexOf( " order" ) );
                //    _whereStr = " and (" + oldValue + ")" + _whereStr.Replace( oldValue, "" );
                }
                else {
                    _whereStr = " and (" + _whereStr + ") order by Id " + _state.Order;
                }
            }
        }

        private static String queryCachedObjects( IList parentResults, ArrayList results, IList oldParentsList, String _no_cached_Ids ) {
            foreach (IEntity obj in parentResults) {
                IEntity objCache = ObjectPool.FindOne( obj.GetType(), obj.Id );
                if (objCache != null) {
                    results.Add( objCache );
                    oldParentsList.RemoveAt( getIndexOfObject( oldParentsList, objCache ) );
                }
                else {
                    _no_cached_Ids = _no_cached_Ids + obj.Id + ",";
                }
            }
            _no_cached_Ids = _no_cached_Ids.TrimEnd( ',' );
            return _no_cached_Ids;
        }

        private void queryChildObjects( int parentResultsCount, ArrayList results, IList oldParentsList, String _no_cached_Ids ) {
            int icount = 0;
            foreach (EntityInfo info in _state.EntityInfo.ChildEntityList) {
                if (icount >= parentResultsCount) {
                    break;
                }


                ObjectInfo state = new ObjectInfo( info );
                IList list = ObjectDB.Find( state, String.Format( "Id in ({0})", _no_cached_Ids ) ).select( _selectItems ).list();

                for (int i = 0; i < list.Count; i++) {
                    IEntity objc = list[i] as IEntity;
                    // state
                    //objc.state = new ObjectInfo( objc.GetType() ).Copy( _state );
                    results.Add( objc );
                    oldParentsList.RemoveAt( getIndexOfObject( oldParentsList, objc ) );
                    ObjectPool.Add( objc );
                }

                if (list.Count > 0) {
                    icount += list.Count;
                }

            }
        }


        private IList findFromChild( IList parentResults ) {
            int count = parentResults.Count;
            ArrayList results = new ArrayList();
            IList oldParentsList = copyList( parentResults );
            String str = "";
            str = queryCachedObjects( parentResults, results, oldParentsList, str );
            if (!strUtil.IsNullOrEmpty( str )) {
                queryChildObjects( count, results, oldParentsList, str );
                if (oldParentsList.Count > 0) {
                    results.AddRange( oldParentsList );
                }
                results.Sort();
            }
            return results;
        }

        internal static int getIndexOfObject( IList results, IEntity target ) {
            for (int i = 0; i < results.Count; i++) {
                IEntity obj = results[i] as IEntity;
                if (obj.Id == target.Id) {
                    return i;
                }
            }
            return -1;
        }

        private String getPropertyValueStringAll( IList list, String propertyName ) {
            StringBuilder builder = new StringBuilder();
            foreach (IEntity obj in list) {
                builder.Append( obj.get( propertyName ).ToString() );
                builder.Append( "," );
            }
            return builder.ToString().TrimEnd( ',' );
        }

        private static IList copyList( IList oldList ) {
            IList list = new ArrayList();
            foreach (Object obj in oldList) {
                list.Add( obj );
            }
            return list;
        }

    }
}

