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
using System.Data;

using wojilu.Data;
using wojilu.Log;
using wojilu.ORM.Operation;

namespace wojilu.ORM {

    /// <summary>
    /// 数据访问工具
    /// </summary>
    internal class ObjectDB {


        private static readonly ILog logger = LogManager.GetLogger( typeof( ObjectDB ) );

        public static IEntity FindById( int id, ObjectInfo state ) {
            return FindByIdOperation.FindById( id, state );
        }

        public static IList FindAll( ObjectInfo state ) {
            return FindAllOperation.FindAll( state );
        }

        public static Query Find( ObjectInfo obj, String condition ) {
            return FindByOperation.Find( condition, obj );
        }

        public static IList FindPage( ObjectInfo state, String condition ) {
            return FindPageOperation.FindPage( state, condition );
        }

        public static Result Insert( IEntity obj ) {
            return InsertOperation.Insert( obj );
        }

        public static Result InsertWithoutParent( IEntity obj ) {
            return InsertOperation.InsertSelf( obj );
        }

        public static Result Update( IEntity obj ) {
            return UpdateOperation.Update( obj );
        }

        public static void Update( IEntity obj, String[] arrPropertyName ) {
            UpdateOperation.Update( obj, arrPropertyName );
        }

        public static void Update( IEntity obj, String propertyName ) {
            UpdateOperation.Update( obj, new String[] { propertyName } );
        }

        public static void UpdateBatch( IEntity obj, String action, String condition ) {
            UpdateOperation.UpdateBatch( obj, action, condition );
        }

        public static int Delete( IEntity obj ) {
            return DeleteOperation.Delete( obj );
        }

        public static int Delete( Type t, int id ) {
            return DeleteOperation.Delete( t, id );
        }

        public static int DeleteBatch( Type t, String condition ) {
            return DeleteOperation.DeleteBatch( condition, Entity.GetInfo( t ) );
        }

        public static int Count( Type t ) {
            return Count( t, "" );
        }

        public static int Count( Type t, String condition ) {
            return CountOperation.Count( t, condition );
        }

        public static IList FindBySql( String sql, Type type ) {

            IDatabaseDialect dialect = Entity.GetInfo( type ).Dialect;
            sql = dialect.GetLimit( sql );

            logger.Info( String.Format( "{0}[FindBySql]{1}", LoggerUtil.SqlPrefix, sql ) );

            ObjectInfo state = new ObjectInfo( type );// EntityFactory.New( type.FullName ).state;
            state.Includer.IncludeAll();
            return EntityPropertyUtil.FindList( state, sql );
        }

    }
}

