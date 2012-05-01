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
using wojilu.Data;

namespace wojilu.ORM {

    internal class OrmHelper {

        internal static void CloseDataReader( IDataReader rd ) {

            if (rd == null) return;
            if (rd.IsClosed) return;

            rd.Close();
        }

        internal static void SetParameters( IDbCommand cmd, String action, IEntity obj, EntityInfo entityInfo ) {

            for (int i = 0; i < entityInfo.SavedPropertyList.Count; i++) {

                EntityPropertyInfo info = entityInfo.SavedPropertyList[i];

                if (isContinue( action, info, entityInfo )) continue;


                Object paramVal = obj.get( info.Name );
                if (paramVal == null && info.DefaultAttribute != null) {
                    paramVal = info.DefaultAttribute.Value;
                }

                if (paramVal == null) {
                    setDefaultValue( cmd, info, entityInfo );
                }
                else if (info.Type.IsSubclassOf( typeof( IEntity ) ) || MappingClass.Instance.ClassList.Contains( info.Type.FullName )) {
                    setEntityId( cmd, info, paramVal );
                }
                else {
                    paramVal = DataFactory.SetParameter( cmd, info.ColumnName, paramVal );
                    obj.set( info.Name, paramVal );
                }                

            }
        }

        private static Boolean isContinue( String action, EntityPropertyInfo info, EntityInfo entityInfo ) {

            if (info.SaveToDB == false) return true;
            if (info.IsList) return true;

            if (info.Name.Equals( "Id" )) {

                if (action.Equals( "update" )) return true;

                // 增加实体键类型识别
                if (/**/DbConfig.Instance.IsAutoId && action.Equals("insert") 
                    && entityInfo.Parent == null
                    ) 
                    return true;
            }

            return false;
        }


        private static void setDefaultValue( IDbCommand cmd, EntityPropertyInfo info, EntityInfo entityInfo ) {

            if (MappingClass.Instance.ClassList.Contains( info.Type.FullName )) {
                DataFactory.SetParameter( cmd, info.ColumnName, -1 );
            }
            else if (info.Type == typeof( DateTime )) {
                if (entityInfo.DbType == DatabaseType.Access) {
                    DataFactory.SetParameter( cmd, info.ColumnName, DateTime.Now.ToString() );
                }
                else {
                    DataFactory.SetParameter( cmd, info.ColumnName, DateTime.Now );
                }
            }
            else {
                DataFactory.SetParameter( cmd, info.ColumnName, "" );
            }
        }

        private static void setEntityId( IDbCommand cmd, EntityPropertyInfo info, Object paramVal ) {
            int id = ((IEntity)paramVal).Id;
            DataFactory.SetParameter( cmd, info.ColumnName, id );
        }

        public static Boolean IsEntityBase( Type t ) {
            return t.FullName.IndexOf( "wojilu.ObjectBase" ) >= 0;
        }


    }

}

