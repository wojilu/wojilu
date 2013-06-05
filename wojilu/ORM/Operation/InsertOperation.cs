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

using wojilu.Data;
using wojilu.Log;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Operation {

    internal class InsertOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( InsertOperation ) );

        private static Object objLock = new Object();

        public static Result Insert( IEntity obj ) {
            if (obj == null) throw new ArgumentNullException();
            Result result = Insert( obj, Entity.GetInfo( obj ), true );
            return result;
        }

        private static Result Insert( IEntity obj, EntityInfo entityInfo, Boolean isInsertParent ) {
            Result result = Validator.Validate( obj, "insert" );
            if (result.HasErrors) return result;

            if (isInsertParent && entityInfo.Parent != null) {
                IEntity objP = Entity.New( entityInfo.Type.BaseType.FullName );
                List<EntityPropertyInfo> eplist = Entity.GetInfo( objP ).SavedPropertyList;
                foreach (EntityPropertyInfo info in eplist) {
                    if (info.IsList) continue;
                    objP.set( info.Name, obj.get( info.Name ) );
                }
                ObjectDB.Insert( objP );

                obj.Id = objP.Id;
            }

            List<IInterceptor> ilist = MappingClass.Instance.InterceptorList;
            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].BeforInsert( obj );
            }

            IDbCommand cmd = DataFactory.GetCommand( getInsertSql( entityInfo ), DbContext.getConnection( entityInfo ) );
            OrmHelper.SetParameters( cmd, "insert", obj, entityInfo );

            lock (objLock) {
                executeCmd( cmd, entityInfo, ref obj );
            }

            for (int i = 0; i < ilist.Count; i++) {
                ilist[i].AfterInsert( obj );
            }
            CacheUtil.CheckCountCache( "insert", obj, entityInfo );
            result.Info = obj;

            CacheTime.updateTable( obj.GetType() );

            return result;
        }

        private static int executeCmd( IDbCommand cmd, EntityInfo entityInfo, ref IEntity obj ) {

            // 读取操作结果
            int retval = cmd.ExecuteNonQuery();

            // 是否采用自增编号
            if (DbConfig.Instance.IsAutoId) {
                String sqlId = String.Format( "select id from {0} order by id desc", entityInfo.TableName );
                sqlId = entityInfo.Dialect.GetLimit( sqlId, 1 );
                cmd.CommandText = sqlId;
                obj.Id = cvt.ToInt( cmd.ExecuteScalar() );
            }

            return retval;
        }

        public static Result InsertSelf( IEntity obj ) {
            return Insert( obj, Entity.GetInfo( obj ), false );
        }

        private static String getInsertSql( EntityInfo entityInfo ) {
            String str = "insert into " + entityInfo.TableName + " (";
            String fStr = "";
            String vStr = ") values(";
            for (int i = 0; i < entityInfo.SavedPropertyList.Count; i++) {
                EntityPropertyInfo info = entityInfo.SavedPropertyList[i];

                if ((
                    ( /**/!DbConfig.Instance.IsAutoId || !(info.Name.ToLower() == "id") || (entityInfo.Parent != null))
                    && info.SaveToDB)
                    && (!info.IsList && !info.IsList)
                    ) {
                    String col = info.ColumnName ?? "";
                    fStr = fStr + col + ", ";
                    vStr = vStr + entityInfo.Dialect.GetParameter( info.ColumnName ) + ", ";
                }
            }
            fStr = fStr.Trim().TrimEnd( ',' );
            vStr = vStr.Trim().TrimEnd( ',' );
            str = str + fStr + vStr + ")";
            logger.Info( LoggerUtil.SqlPrefix + entityInfo.Name + " InsertSql：" + str );
            return str;
        }

    }
}

