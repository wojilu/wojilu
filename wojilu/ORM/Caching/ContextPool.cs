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
using wojilu.Data;
using System.Collections;
using wojilu.ORM.Operation;

namespace wojilu.ORM.Caching {

    /// <summary>
    /// 一级缓存(上下文缓存)的缓存池
    /// </summary>
    internal class ContextPool : IObjectPool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContextPool ) );

        public static ContextPool Instance = new ContextPool();

        private ContextPool() { }

        public void Add( IEntity obj ) {

            if (obj == null) return;

            String key = CacheKey.getObject( obj.GetType(), obj.Id );
            addToContext( key, obj );
            CacheTime.updateObject( obj ); // 加入缓存的时候，设置最新 timestamp
        }


        public void AddAll( Type t, IList objList ) {
            addList( CacheKey.getObjectAll( t ), objList );
        }

        public void AddQueryList( Type t, String sql, Dictionary<String, Object> parameters, IList objList ) {
            addList( CacheKey.getQuery( t, sql, parameters ), objList );
        }

        public void AddPage( Type t, String condition, PageHelper pager, IList list ) {
            String key = CacheKey.getPageList( t, condition, pager.getSize(), pager.getCurrent() );
            addList( key, list );
        }

        public void AddCount( Type t, int count ) {
            String key = CacheKey.getCountKey( t );
            addToContext( key, count );
        }

        public void AddCount( Type t, String condition, int count ) {
            String key = CacheKey.getCountKey( t, condition );
            addToContext( key, count );
        }

        public void AddSqlList( String sql, IList objList ) {
            addToContext( sql, objList );
        }

        //---------------------------------------------------

        private void addToContext( String key, Object val ) {

            ContextCache.Put( key, val );
            logger.Debug( "ctx_cache_+=>" + key );
        }

        private void addList( String key, IList objList ) {

            if (objList == null ) return;
            addToContext( key, objList );
            foreach (IEntity obj in objList) {
                ContextCache.Put( CacheKey.getObject( obj.GetType(), obj.Id ), obj );
            }
            CacheTime.updateList( key );
        }

        //-------------------------------------------------------查询-----------------------------------------------------------



        public IEntity FindOne( Type t, int id ) {
            String key = CacheKey.getObject( t, id );
            return getFromContext( key ) as IEntity;
        }

        public IList FindAll( Type t ) {
            String key = CacheKey.getObjectAll( t );
            return getListFromCache( key, t );
        }

        public IList FindByQuery( ConditionInfo condition ) {
            return getListFromCache( CacheKey.getQuery( condition.Type, condition.Sql, condition.Parameters ), condition.Type );
        }

        public IList FindByQuery( Type type, String _queryString, Dictionary<String, Object> _namedParameters ) {
            return getListFromCache( CacheKey.getQuery( type, _queryString, _namedParameters ), type );
        }

        public IPageList FindPage( Type t, String condition, PageHelper pager ) {
            return null;
        }

        public IList FindBySql( String sql, Type t ) {
            return ContextCache.Get( sql ) as IList;
        }

        public int FindCount( Type t ) {
            String key = CacheKey.getCountKey( t );
            Object obj = getFromContext( key );
            return obj == null ? -1 : (int)obj;
        }

        public int FindCount( Type t, String condition ) {
            String key = CacheKey.getCountKey( t, condition );
            Object obj = getFromContext( key );
            return obj == null ? -1 : (int)obj;
        }

        //---------------------------------------------------

        private static Object getFromContext( String key ) {
            Object obj = ContextCache.Get( key );
            if (obj != null) {
                logger.Debug( "ctx_cache_get=>" + key );
            }
            return obj;
        }


        private static IList getListFromCache( String queryKey, Type t ) {

            if (CacheTime.isListUpdate( queryKey, t )) return null;

            return getFromContext( queryKey ) as IList;
        }

        public void Delete( Type t, int id ) {

            ContextCache.Remove( t.FullName, id );
        }

    }

}
