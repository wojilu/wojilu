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
using System.Data;
using wojilu;
using wojilu.Data;
using wojilu.Log;
using System.Collections.Generic;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Operation {

    internal class DeleteOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( DeleteOperation ) );

        public static int Delete( IEntity obj ) {
            if (obj == null) throw new ArgumentNullException();
            return Delete( obj.Id, obj, Entity.GetInfo( obj ) );
        }

        public static int Delete( Type t, int id ) {
            IEntity obj = FindByIdOperation.FindById( id, new ObjectInfo( t ) );
            if (obj != null) {
                return Delete( obj );
            }
            return 0;
        }


        public static int Delete( int id, IEntity obj, EntityInfo entityInfo ) {

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforDelete( obj );
            }

            int rowAffected = 0;
            rowAffected += deleteSingle( id, entityInfo );
            if (entityInfo.ChildEntityList.Count > 0) {
                foreach (EntityInfo info in entityInfo.ChildEntityList) {
                    rowAffected += deleteSingle( id, MappingClass.Instance.ClassList[info.Type.FullName] as EntityInfo );
                }
            }
            if (entityInfo.Parent != null) {
                IEntity objP = Entity.New( entityInfo.Parent.Type.FullName );
                rowAffected += deleteSingle( id, Entity.GetInfo( objP ) );
            }

            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterDelete( obj );
            }

            CacheUtil.CheckCountCache( "delete", obj, entityInfo );

            return rowAffected;
        }

        public static int DeleteBatch( String condition, EntityInfo entityInfo ) {

            if (strUtil.IsNullOrEmpty( condition )) {
                return 0;
            }

            String deleteSql = new SqlBuilder( entityInfo ).GetDeleteSql( condition );
            logger.Info(LoggerUtil.SqlPrefix+ "delete sql : " + deleteSql );

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforDeleteBatch( entityInfo.Type, condition );
            }

            IDbCommand cmd = DataFactory.GetCommand( deleteSql, DbContext.getConnection( entityInfo ) );
            int rowAffected = cmd.ExecuteNonQuery();
            logger.Info( "delete : " + rowAffected + " records affected" );

            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterDeleteBatch( entityInfo.Type, condition );
            }

            // update cache  timestamp
            CacheTime.updateTable( entityInfo.Type );

            return rowAffected;
        }


        private static int Delete( Type t, String deleteCondition ) {
            return DeleteBatch( deleteCondition, MappingClass.Instance.ClassList[t.FullName] as EntityInfo );
        }

        private static int deleteSingle( int id, EntityInfo entityInfo ) {

            IDbCommand command = DataFactory.GetCommand( getDeleteSql( id, entityInfo ), DbContext.getConnection( entityInfo ) );
            return command.ExecuteNonQuery();
        }

        private static String getDeleteSql( int id, EntityInfo entityInfo ) {
            String str = String.Format( "delete from {0} where Id={1}", entityInfo.TableName, id );
            logger.Info( "Delete(int id): " + str );
            return str;
        }
    }
}

