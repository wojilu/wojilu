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
using System.Collections;
using wojilu.Caching;
using wojilu.Data;
using wojilu.ORM.Operation;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Caching {

    /// <summary>
    /// 二级缓存(application级)的缓存池
    /// </summary>
    public class ApplicationPool : IObjectPool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ApplicationPool ) );
        private static IApplicationCache appCache = CacheManager.GetApplicationCache();

        public static readonly ApplicationPool Instance = new ApplicationPool();

        private ApplicationPool() { }

        // 只是单纯加入缓存，比如在findList的时候，顺便加入缓存
        // 插入数据的时候只更新表的timestamp，并不加入缓存
        public void Add( IEntity obj ) {

            if (obj == null) return;

            String key = CacheKey.getObject( obj.GetType(), obj.Id );
            addToApplication( key, obj );
            CacheTime.updateObject( obj ); // 加入缓存的时候，设置最新 timestamp
        }

        public void AddAll( Type t, IList objList ) {
            String key = CacheKey.getObjectAll( t );
            addList( key, objList );
        }

        public void AddQueryList( Type t, String sql, Dictionary<String, Object> parameters, IList objList ) {
            String key = CacheKey.getQuery( t, sql, parameters );
            addList( key, objList );
        }

        public void AddPage( Type t, String condition, PageHelper pager, IList list ) {
            String key = CacheKey.getPageList( t, condition, pager.getSize(), pager.getCurrent() );
            addList( key, list );
            addToApplication( CacheKey.getPagerInfoKey( key ), pager ); // 添加分页统计数据
        }

        public void AddSqlList( String sql, IList objList ) {
            if (objList == null) return;
            addList( sql, objList );
        }

        //---------------------------------------------------------------

        public void AddCount( Type t, int count ) {
            String key = CacheKey.getCountKey( t );
            addToApplication( key, count );
            CacheTime.updateCount( t );
        }

        public void AddCount( Type t, String condition, int count ) {
            String key = CacheKey.getCountKey( t, condition );
            addToApplication( key, count );
            CacheTime.updateCount( t, condition );
        }


        //-----------------------------------------------------------------


        private static void addToApplication( String key, Object val ) {

            int minutes = DbConfig.Instance.ApplicationCacheMinutes;

            if (minutes == CacheTime.CacheForever) {
                appCache.Put( key, val );
                logger.Debug( "APP_cache_+=>" + key );
            }
            else if (minutes > 0) {
                appCache.Put( key, val, minutes );
                logger.Debug( "APP_cache_+=>" + key );
            }
        }

        private static void addList( String key, IList objList ) {

            if (objList == null) return;

            foreach (IEntity obj in objList) {
                addToApplication( CacheKey.getObject( obj.GetType(), obj.Id ), obj );
                CacheTime.updateObject( obj ); // 加入缓存的时候，设置最新 timestamp
            }

            List<int> ids = new List<int>();
            foreach (IEntity obj in objList) ids.Add( obj.Id );
            addToApplication( key, ids );
            CacheTime.updateList( key );
        }

        //-------------------------------------------------------查询-----------------------------------------------------------

        // 内部调用，需要查询一级缓存
        private IEntity findOnePrivate( Type t, int id ) {

            String key = CacheKey.getObject( t, id );
            IEntity result = ContextPool.Instance.FindOne( t, id );
            if( result != null ) return result;

            result = this.FindOne( t, id );
            ContextPool.Instance.Add( result );

            return result;
        }


        public IEntity FindOne( Type t, int id ) {

            String key = CacheKey.getObject( t, id );

            IEntity result = getFromApplication( key ) as IEntity;
            if (result != null) {
                if (CacheTime.isObjectUpdated( result )) return null; // 检查是否更新
                logger.Debug( "APP_cache_get=>" + key );

                return result;
            }

            return null;
        }

        public IList FindAll( Type t ) {

            return getListFromCache( CacheKey.getObjectAll( t ), t );
        }

        public IList FindByQuery( ConditionInfo condition ) {
            return getListFromCache( CacheKey.getQuery( condition.Type, condition.Sql, condition.Parameters ), condition.Type );
        }

        public IList FindByQuery( Type type, String _queryString, Dictionary<String, Object> _namedParameters ) {
            return getListFromCache( CacheKey.getQuery( type, _queryString, _namedParameters ), type );
        }

        public IPageList FindPage( Type t, String condition, PageHelper pager ) {
            String key = CacheKey.getPageList( t, condition, pager.getSize(), pager.getCurrent() );
            logger.Debug( "FindPage=>" + t.FullName );
            IList list = getListFromCache( key, t );
            if (list == null) return null;

            IPageList result = new DataPageInfo();

            PageHelper pageInfo = getPagerInfo( key );
            if (pageInfo == null) return null;

            result.Results = list;
            result.PageCount = pageInfo.PageCount;
            result.RecordCount = pageInfo.RecordCount;
            result.Size = pageInfo.getSize();
            result.PageBar = pageInfo.PageBar;
            result.Current = pageInfo.getCurrent();

            return result;
        }

        private static PageHelper getPagerInfo( String pageKey ) {
            return getFromApplication( CacheKey.getPagerInfoKey( pageKey ) ) as PageHelper;
        }

        public IList FindBySql( String sql, Type t ) {
            return getListFromCache( sql, t );
        }

        public int FindCount( Type t ) {
            if (CacheTime.isCountUpdate( t )) return -1;
            String key = CacheKey.getCountKey( t );
            Object obj = getFromApplication( key );
            return obj == null ? -1 : (int)obj;
        }

        public int FindCount( Type t, String condition ) {
            if (CacheTime.isCountUpdate( t, condition )) return -1;
            String key = CacheKey.getCountKey( t, condition );
            Object obj = getFromApplication( key );
            return obj == null ? -1 : (int)obj;
        }


        //-----------------------------------------------------------

        internal IList getListFromCache( String queryKey, Type t ) {

            if (CacheTime.isListUpdate( queryKey, t )) return null;

            List<int> ids = appCache.Get( queryKey ) as List<int>;
            if (ids == null) return null;

            IList result = new ArrayList();
            foreach (int id in ids) {

                IEntity obj = this.findOnePrivate( t, id );

                if (obj == null) return null;

                if (CacheTime.isObjectUpdated( obj )) return null; // 如果有任何一个对象更新过，则缓存失效

                // 检查实体属性
                renewEntityPropertyValue( obj );

                result.Add( obj );
            }


            logger.Debug( "APP_cache_get=>" + queryKey );
            return result;
        }

        private void renewEntityPropertyValue( IEntity obj ) {
            EntityInfo ei = Entity.GetInfo( obj );
            foreach (EntityPropertyInfo ep in ei.EntityPropertyList) {
                IEntity objP = obj.get( ep.Name ) as IEntity;
                if (objP == null) continue;

                IEntity val = this.findOnePrivate( objP.GetType(), objP.Id );
                if (val == null) continue;
                
                ep.SetValue( obj, val );
            }
        }

        internal static Object getFromApplication( String key ) {
            return appCache.Get( key );
        }

        public void Delete( Type t, int id ) {
            appCache.Remove( CacheKey.getObject( t.FullName, id ) );
            logger.Debug( "Delete=>" + t.FullName + id );
            CacheTime.updateTable( t );

        }

        public void Clear() {
            appCache.Clear();
            CacheTime.Clear();
        }

    }

}
