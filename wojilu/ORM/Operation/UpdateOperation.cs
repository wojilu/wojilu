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
using System.Data;
using System.Text;

using wojilu.Data;
using wojilu.Log;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Operation {

    internal class UpdateOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UpdateOperation ) );

        public static Result Update( IEntity obj ) {
            if (obj == null) throw new ArgumentNullException();
            EntityInfo ei = Entity.GetInfo( obj );
            return Update( obj, ei );
        }

        private static Result Update( IEntity obj, EntityInfo entityInfo ) {
            Result result = Validator.Validate( obj, "update" );
            if (result.IsValid == false) return result;

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforUpdate( obj );
            }

            updateSingle( obj, entityInfo );
            if (entityInfo.Parent != null) {
                IEntity objParent = Entity.New( entityInfo.Parent.Type.FullName );
                setParentValueFromChild( objParent, obj );
                updateSingle( objParent, Entity.GetInfo( objParent ) );
            }
            CacheUtil.CheckCountCache( "insert", obj, entityInfo );


            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterUpdate( obj );
            }
            result.Info = obj;

            // update cache  timestamp
            CacheTime.updateTable( entityInfo.Type );

            return result;
        }

        public static void Update( IEntity obj, String[] arrPropertyName ) {
            if (obj == null) throw new ArgumentNullException();
            Update( obj, arrPropertyName, Entity.GetInfo( obj ) );
        }

        public static void Update( IEntity obj, String[] arrPropertyName, EntityInfo entityInfo ) {
            if (obj == null) throw new ArgumentNullException();
            StringBuilder builder = new StringBuilder( String.Empty );
            builder.Append( "update " );
            builder.Append( entityInfo.TableName );
            builder.Append( " set " );

            for (int i = 0; i < arrPropertyName.Length; i++) {
                String columnName = entityInfo.GetColumnName( arrPropertyName[i] );
                builder.Append( columnName );
                builder.Append( "=" );
                builder.Append( entityInfo.Dialect.GetParameter( columnName ) );
                builder.Append( "," );
            }
            builder.Append( " where Id=" );
            builder.Append( entityInfo.Dialect.GetParameter( "Id" ) );

            String commandText = builder.ToString().Replace( ", where", " where" );
            logger.Info( LoggerUtil.SqlPrefix+entityInfo.Name + "[" + entityInfo.TableName + "] Update Sql：" + commandText );

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforUpdate( obj );
            }

            IDbCommand cmd = DataFactory.GetCommand( commandText, DbContext.getConnection( entityInfo ) );
            for (int i = 0; i < arrPropertyName.Length; i++) {
                Object parameterValue = obj.get( arrPropertyName[i] );
                String columnName = entityInfo.GetColumnName( arrPropertyName[i] );
                DataFactory.SetParameter( cmd, columnName, parameterValue );
            }
            int id = obj.Id;
            DataFactory.SetParameter( cmd, "Id", id );
            cmd.ExecuteNonQuery();

            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterUpdate( obj );
            }

            // update cache  timestamp
            CacheTime.updateTable( entityInfo.Type );


            CacheUtil.CheckCountCache( "insert", obj, entityInfo );
        }

        public static void UpdateBatch( IEntity obj, String action, String condition ) {
            if (obj == null) throw new ArgumentNullException();
            UpdateBatch( obj, action, condition, Entity.GetInfo( obj ) );
        }

        private static int UpdateBatch( IEntity obj, String action, String condition, EntityInfo entityInfo ) {
            if (obj == null) throw new ArgumentNullException();
            if (strUtil.IsNullOrEmpty( action )) {
                logger.Info( "no sql is executed : action String is empty" );
                return 0;
            }

            //action = action.Trim().ToLower();
            //if (!action.StartsWith( "set" )) {
            //    action = "set " + action;
            //}

            // http://www.wojilu.com/rubywu
            // http://www.wojilu.com/Forum1/Topic/1706
            if (!action.Trim().ToLower().StartsWith( "set" )) {
                action = "set " + action;
            }

            condition = condition.Trim().ToLower();
            if (!condition.StartsWith( "where" )) {
                condition = "where " + condition;
            }

            String sql = String.Format( "update {0} {1} {2}", entityInfo.TableName, action, condition );
            logger.Info(LoggerUtil.SqlPrefix+ "update sql : " + sql );

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforUpdateBatch( obj.GetType(), action, condition );
            }

            IDbCommand cmd = DataFactory.GetCommand( sql, DbContext.getConnection( entityInfo ) );
            int rowAffected = cmd.ExecuteNonQuery();
            logger.Info( "update : " + rowAffected + " records affected" );

            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterUpdateBatch( obj.GetType(), action, condition );
            }

            // update cache  timestamp
            CacheTime.updateTable( entityInfo.Type );

            return rowAffected;
        }

        private static void updateSingle( IEntity obj, EntityInfo entityInfo ) {

            IDbCommand cmd = DataFactory.GetCommand( getUpdateSql( entityInfo ), DbContext.getConnection( entityInfo ) );
            OrmHelper.SetParameters( cmd, "update", obj, entityInfo );
            DataFactory.SetParameter( cmd, "Id", obj.Id );
            cmd.ExecuteNonQuery();
        }

        private static String getUpdateSql( EntityInfo entityInfo ) {
            StringBuilder builder = new StringBuilder( "update " );
            builder.Append( entityInfo.TableName );
            builder.Append( " set " );
            for (int i = 0; i < entityInfo.SavedPropertyList.Count; i++) {
                EntityPropertyInfo info = entityInfo.SavedPropertyList[i];
                if (((info.Name.ToLower() != "id") && info.SaveToDB) && !info.IsList) {
                    builder.Append( info.ColumnName );
                    builder.Append( "=" );
                    builder.Append( entityInfo.Dialect.GetParameter( info.ColumnName ) );
                    builder.Append( ", " );
                }
            }
            builder.Remove( builder.Length - 2, 2 );
            builder.Append( " where Id=" );
            builder.Append( entityInfo.Dialect.GetParameter( "Id" ) );
            logger.Info( LoggerUtil.SqlPrefix+" [update sql] " + builder.ToString() );
            return builder.ToString();
        }

        private static void setParentValueFromChild( IEntity objParent, IEntity objChild ) {
            List<EntityPropertyInfo> eplist = Entity.GetInfo( objParent ).SavedPropertyList;
            foreach (EntityPropertyInfo info in eplist) {
                objParent.set( info.Name, objChild.get( info.Name ) );
            }
        }

    }
}

