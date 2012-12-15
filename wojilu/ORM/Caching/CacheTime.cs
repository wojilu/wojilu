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
using wojilu.ORM.Caching;

namespace wojilu.ORM.Caching {

    /// <summary>
    /// 对象的缓存时间管理工具
    /// </summary>
    public class CacheTime {

        private static readonly ILog logger = LogManager.GetLogger( typeof( CacheTime ) );
        private static IApplicationCache appCache = CacheManager.GetApplicationCache();

        public const int CacheForever = -999;

        private static Hashtable map = new Hashtable();

        public static void updateTable( Type t ) {
            logger.Debug( "updateTable at=>" + DateTime.Now + "_" + t.FullName );
            map[TimestampKey.getTable( t.FullName )] = DateTime.Now;
        }

        private static Object getTableUpdated( string typeFullName ) {
            return map[TimestampKey.getTable( typeFullName )];
        }

        private static void updatePrivate( String key, DateTime t) {
            //appCache.Put( key, t );
            map[key] = t;
        }

        private static object getUpdatedPrivate( String key ) {
            //return appCache.Get( key );
            return map[key];
        }


        //--------------------------------------------------------------------------------------------

        public static void updateObject( IEntity obj ) {
            updatePrivate( TimestampKey.getObject( obj ), DateTime.Now );
        }

        private static Object getObjectUpdated( IEntity obj ) {
            return getUpdatedPrivate( TimestampKey.getObject( obj ) );
        }

        public static Boolean isObjectUpdated( IEntity obj ) {

            Object tblUpdated = getTableUpdated( obj.GetType().FullName );
            if (tblUpdated == null) return false;

            Object objUpdated = getObjectUpdated( obj );
            if (objUpdated == null) return true;

            return (DateTime)tblUpdated >= (DateTime)objUpdated;
        }

        //--------------------------------------------------------------------------------------------

        public static void updateList( String cacheKey ) {
            logger.Debug( "updateList at=>" + DateTime.Now + "_" + cacheKey );
            updatePrivate( TimestampKey.getList( cacheKey ), DateTime.Now );
        }

        private static Object getListUpdaed( String cacheKey ) {
            return getUpdatedPrivate( TimestampKey.getList( cacheKey ) );
        }

        public static bool isListUpdate( string cacheKey, Type t ) {

            Object tblUpdated = getTableUpdated( t.FullName );
            if (tblUpdated == null) return false;
            logger.Debug( "tblUpdated=" + tblUpdated );

            Object listUpdated = getListUpdaed( cacheKey );
            if (listUpdated == null) return true;
            logger.Debug( "listUpdated=" + listUpdated );

            return (DateTime)tblUpdated >= (DateTime)listUpdated;
        }


        //--------------------------------------------------------------------------------------------

        public static void updateCount( Type t ) {
            map[TimestampKey.getCount( t )] = DateTime.Now;
        }

        public static void updateCount( Type t, String condition ) {
            map[TimestampKey.getCount( t, condition )] = DateTime.Now;
        }

        private static Object getCountUpdated( Type t ) {
            return map[TimestampKey.getCount( t )];
        }

        private static Object getCountUpdated( Type t, String condition ) {
            return map[TimestampKey.getCount( t, condition )];
        }

        public static Boolean isCountUpdate( Type t ) {

            Object tblUpdated = getTableUpdated( t.FullName );
            if (tblUpdated == null) return false;

            Object countUpdated = getCountUpdated( t );
            if (countUpdated == null) return true;

            return (DateTime)tblUpdated >= (DateTime)countUpdated;
        }

        public static Boolean isCountUpdate( Type t, String condition ) {

            Object tblUpdated = getTableUpdated( t.FullName );
            if (tblUpdated == null) return false;

            Object countUpdated = getCountUpdated( t, condition );
            if (countUpdated == null) return true;

            return (DateTime)tblUpdated >= (DateTime)countUpdated;
        }

        public static void Clear() {
            map.Clear();
        }

    }

}
