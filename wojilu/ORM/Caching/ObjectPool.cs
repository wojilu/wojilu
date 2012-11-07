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
using wojilu.Data;
using wojilu.ORM.Operation;

namespace wojilu.ORM.Caching {

    internal class ObjectPool {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ObjectPool ) );

        private static List<IObjectPool> pools = getPools();

        private static List<IObjectPool> getPools() {

            List<IObjectPool> list = new List<IObjectPool>();

            if (DbConfig.Instance.ContextCache) list.Add( ContextPool.Instance );
            if (DbConfig.Instance.ApplicationCache) list.Add( ApplicationPool.Instance );

            return list;
        }

        public static void Add( IEntity obj ) {
            foreach (IObjectPool pool in pools) {
                pool.Add( obj );
            }
        }

        public static void AddAll( Type t, IList objList ) {
            foreach (IObjectPool pool in pools) {
                pool.AddAll( t, objList );
            }
        }

        public static void AddQueryList( Type t, String sql, Dictionary<String, Object> parameters, IList objList ) {
            foreach (IObjectPool pool in pools) {
                pool.AddQueryList( t, sql, parameters, objList );
            }
        }

        public static void AddPage( Type t, String condition, PageHelper pager, IList list ) {
            foreach (IObjectPool pool in pools) {
                pool.AddPage( t, condition, pager, list );
            }
        }

        public static void AddCount( Type t, int count ) {
            foreach (IObjectPool pool in pools) {
                pool.AddCount( t, count );
            }
        }

        public static void AddCount( Type t, String condition, int count ) {
            foreach (IObjectPool pool in pools) {
                pool.AddCount( t, condition, count );
            }
        }

        public static void AddSqlList( String sql, IList objList ) {
            foreach (IObjectPool pool in pools) {
                pool.AddSqlList( CacheKey.getSqlKey( sql ), objList );
            }
        }

        //-------------------------------------------------------≤È—Ø-----------------------------------------------------------


        public static IEntity FindOne( Type t, int id ) {
            foreach (IObjectPool pool in pools) {
                IEntity result = pool.FindOne( t, id );
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        public static IList FindAll( Type t ) {
            foreach (IObjectPool pool in pools) {
                IList result = pool.FindAll( t );
                if (result != null) return result;
            }
            return null;
        }

        public static IList FindByQuery( ConditionInfo condition ) {
            foreach (IObjectPool pool in pools) {
                IList result = pool.FindByQuery( condition );
                if (result != null) return result;
            }
            return null;
        }

        public static IList FindByQuery( Type type, String _queryString, Dictionary<String, Object> _namedParameters ) {
            foreach (IObjectPool pool in pools) {
                IList result = pool.FindByQuery( type, _queryString, _namedParameters );
                if (result != null) return result;
            }
            return null;
        }

        public static IPageList FindPage( Type t, String condition, PageHelper pager ) {
            foreach (IObjectPool pool in pools) {
                IPageList result = pool.FindPage( t, condition, pager );
                if (result != null) return result;
            }
            return null;
        }

        public static IList FindBySql( String sql, Type t ) {
            foreach (IObjectPool pool in pools) {
                IList result = pool.FindBySql( sql, t );
                if (result != null) return result;
            }
            return null;
        }

        public static int FindCount( Type t ) {
            foreach (IObjectPool pool in pools) {
                int result = pool.FindCount( t );
                if (result >= 0) return result;
            }
            return -1;
        }

        public static int FindCount( Type t, String condition ) {

            foreach (IObjectPool pool in pools) {
                int result = pool.FindCount( t, condition );
                if (result >= 0) return result;
            }
            return -1;
        }


        //---------------------------------------------------------------------------------------------------

        public static void Update( IEntity obj ) {
            Add( obj );
        }

        public static int Delete( IEntity obj ) {
            return Delete( obj.GetType(), obj.Id );
        }

        public static int Delete( Type t, int id ) {
            foreach (IObjectPool pool in pools) {
                pool.Delete( t, id );
            }
            return 1;
        }

    }
}
