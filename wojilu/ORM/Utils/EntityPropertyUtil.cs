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
using wojilu;
using wojilu.Data;
using System.Collections.Generic;
using wojilu.Members.Interface;
using wojilu.Web;
using wojilu.ORM.Utils;
using wojilu.ORM.Caching;

namespace wojilu.ORM {


    internal class EntityPropertyUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( EntityPropertyUtil ) );

        public static void Fill_EntityProperty_Ids( IDataRecord record, IList _includeEntityPropertyList, ref Hashtable _entityProperty_ids ) {

            if (_includeEntityPropertyList == null) return;

            foreach (EntityPropertyInfo info in _includeEntityPropertyList) {


                String columnName = info.ColumnName;

                if (record[columnName] == DBNull.Value) continue;

                if (_entityProperty_ids[columnName] == null) {
                    _entityProperty_ids[columnName] = record[columnName] + ",";
                }
                else {
                    String ids = _entityProperty_ids[columnName].ToString();
                    String tempId = "," + record[columnName] + ",";
                    if (("," + ids).IndexOf( tempId ) < 0) {
                        _entityProperty_ids[columnName] = ids + record[columnName] + ",";
                    }
                }
            }
        }

        public static IList FindList( ObjectInfo state, String sql ) {
            IList results = new ArrayList();
            IDataReader rd = null;
            Hashtable hashtable = new Hashtable();
            IDbCommand command = DataFactory.GetCommand( sql, DbContext.getConnection( state.EntityInfo ) );
            try {
                rd = command.ExecuteReader();
                while (rd.Read()) {
                    IDataRecord record = rd;
                    Fill_EntityProperty_Ids( record, state.Includer.EntityPropertyList, ref hashtable );

                    int id = Convert.ToInt32( record["Id"] );
                    Object cache = ObjectPool.FindOne( state.EntityInfo.Type, id );

                    if (cache != null) {
                        results.Add( cache );
                    }
                    else {
                        results.Add( FillUtil.Populate( record, state ) );
                    }
                }
            }
            catch (Exception ex) {
                logger.Error( "sql=>" + sql );
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                throw ex;
            }
            finally {
                OrmHelper.CloseDataReader( rd );
            }


            if (results.Count == 0) {
                return results;
            }
            return setEntityProperty( state, results, hashtable );
        }




        public static IList setEntityProperty( ObjectInfo state, IList results, Hashtable _entityProperty_ids ) {
            Hashtable selectColumn = null;
            if (strUtil.HasText( state.Includer.SelectedProperty ) && (state.Includer.SelectedProperty != "*")) {
                selectColumn = SqlBuilder.GetIncludeSelectColumn( state.Includer.SelectedProperty, state.EntityInfo.EntityPropertyList );
            }
            else {
                selectColumn = Includer.getSelectColumnFromInclude( state.Includer.EntityPropertyList );
            }
            Hashtable propertyList = getEntityPropertyList( state.Includer.EntityPropertyList, _entityProperty_ids, selectColumn, state.Order );
            fillPropertyObjectToResultList( ref results, state.EntityInfo.Type, state.Includer.EntityPropertyList, propertyList );
            return results;
        }


        public static Hashtable getEntityPropertyList( IList _includeEntityPropertyList, Hashtable _ep_ids, Hashtable selectColumn, String order ) {
            Hashtable hashtable = new Hashtable();

            if (_includeEntityPropertyList == null) return hashtable;

            foreach (EntityPropertyInfo info in _includeEntityPropertyList) {
                String epIds = "";
                String[] arrIds = new String[] { };
                if (_ep_ids[info.ColumnName] != null)
                    arrIds = _ep_ids[info.ColumnName].ToString().Trim().TrimEnd( ',' ).Split( ',' );

                foreach (String strId in arrIds) {
                    IEntity cacheObj = ObjectPool.FindOne( info.EntityInfo.Type, cvt.ToInt( strId ) );
                    if (cacheObj != null) {
                        hashtable[getPropertyObjectKey( info.Name, cacheObj.Id )] = cacheObj;
                    }
                    else {
                        epIds = epIds + strId + ",";
                    }
                }
                epIds = epIds.TrimEnd( ',' );

                if (strUtil.IsNullOrEmpty( epIds )) continue;

                String col = selectColumn[info.Name].ToString().Trim().TrimEnd( ',' );
                if (string.Compare( col, "Id", true ) != 0) {
                    if (!col.Equals( "*" ) && (("," + col + ",").ToLower().IndexOf( ",id," ) < 0)) {
                        col = "Id," + col;
                    }

                    ObjectInfo cstate = new ObjectInfo( info.EntityInfo );
                    cstate.IsFindChild = true;
                    IList list = ObjectDB.Find( cstate, String.Format( "Id in ({0})", epIds ) ).list();

                    foreach (IEntity objItem in list) {
                        hashtable[getPropertyObjectKey( info.Name, objItem.Id )] = objItem;
                        ObjectPool.Add( objItem );
                    }

                }
            }

            return hashtable;
        }

        public static void fillPropertyObjectToResultList( ref IList resultList, Type t, IList _includeEntityPropertyList, Hashtable _ep_obj_list ) {

            if (_includeEntityPropertyList == null) return;

            foreach (IEntity obj in resultList) {

                foreach (EntityPropertyInfo ep in _includeEntityPropertyList) {

                    IEntity objValue = obj.get( ep.Name ) as IEntity;
                    if (objValue == null) continue;

                    String key = getPropertyObjectKey( ep.Name, objValue.Id );
                    Object propertyValue = _ep_obj_list[key];

                    ValueSetter.setEntityByCheckNull( obj, ep, propertyValue, objValue.Id );

                }
            }

        }



        public static String getPropertyObjectKey( String propertyName, int objId ) {
            return (propertyName + "_" + objId);
        }

    }
}

