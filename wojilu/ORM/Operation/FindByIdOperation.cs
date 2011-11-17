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
using wojilu.Members.Interface;
using wojilu.Web;
using wojilu.ORM.Utils;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Operation {

    internal class FindByIdOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FindByIdOperation ) );

        public static IEntity FindById( int id, ObjectInfo state ) {


            if (id < 0) return null;

            IEntity result = null;
            if (state.IsFindChild)
                result = findByIdFromChild( id, state.EntityInfo );
            if (result != null) return result;

            if (state.Includer.EntityPropertyList == null)
                state.Includer.IncludeAll();
            return findById_Private( id, state );
        }

        private static IEntity findById_Private( int id, ObjectInfo state ) {

            if (id < 0) return null;

            IEntity result = null;

            SqlBuilder sh = new SqlBuilder( state.EntityInfo );
            processIncluder( state.Includer );
            String sql = sh.GetFindById( id, state.Includer.SelectedProperty );
            IDbCommand cmd = DataFactory.GetCommand( sql, DbContext.getConnection( state.EntityInfo ) );
            IList list = new ArrayList();
            IDataReader rd = null;
            try {
                rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    list.Add( FillUtil.Populate( rd, state ) );
                }
            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                throw ex;
            }
            finally {
                OrmHelper.CloseDataReader( rd );
            }

            if (list.Count > 0) result = list[0] as IEntity;

            result = setEntityProperty( result, id, state );

            return result;
        }

        // TODO: performance
        private static IEntity findByIdFromChild( int id, EntityInfo entityInfo ) {
            foreach (EntityInfo ei in entityInfo.ChildEntityList) {
                IEntity result = ObjectDB.FindById( id, new ObjectInfo( ei ) );
                if (result != null) return result;

            }
            return null;
        }

        // select is prior to include
        private static void processIncluder( Includer includer ) {
            if (strUtil.IsNullOrEmpty( includer.SelectedProperty )) return;
            if (includer.SelectedProperty.Equals( "*" ))
                includer.IncludeAll();
            else
                includer.Include( SqlBuilder.GetIncludeProperty( includer.SelectedProperty ) );
        }

        private static IEntity setEntityProperty( IEntity obj, int id, ObjectInfo state ) {

            if (obj == null)
                return null;

            IList entityPropertyList = state.EntityInfo.EntityPropertyList;
            foreach (EntityPropertyInfo ep in entityPropertyList) {

                if (!isPropertyInIncluder( ep, state.Includer.EntityPropertyList )) continue;

                IEntity propertyValue = obj.get( ep.Name ) as IEntity;
                if (propertyValue == null) continue;

                int pid = propertyValue.Id;
                if (pid <= 0) continue;


                IEntity cachedValue = ObjectPool.FindOne( ep.Type, pid );
                if (cachedValue == null) {

                    propertyValue = ObjectDB.FindById( pid, new ObjectInfo( ep.Type ) );

                    ObjectPool.Add( propertyValue );
                }
                else {
                    propertyValue = cachedValue;
                }

                ValueSetter.setEntityByCheckNull( obj, ep, propertyValue, pid );


            }

            return obj;
        }

        private static Boolean isPropertyInIncluder( EntityPropertyInfo p, IList _includeEntityPropertyList ) {

            if (_includeEntityPropertyList == null) return false;
            if (_includeEntityPropertyList.Count == 0) return false;

            foreach (EntityPropertyInfo _include_ep in _includeEntityPropertyList) {
                if (_include_ep.Name.Equals( p.Name )) return true;
            }

            return false;
        }

    }
}



