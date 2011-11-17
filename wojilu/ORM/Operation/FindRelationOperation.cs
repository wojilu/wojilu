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
using System.Text;

using wojilu.Data;

namespace wojilu.ORM.Operation {

    internal class FindRelationOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FindRelationOperation ) );

        public static IList FindDataOther( Type throughType, Type t, String order, int id ) {

            // 1029
            ObjectInfo state = new ObjectInfo( throughType );
            String relationPropertyName = state.EntityInfo.GetRelationPropertyName( t );
            EntityPropertyInfo info = state.EntityInfo.FindRelationProperty( t );
            state.Order = order;
            state.include( info.Name );
            return ObjectDB.Find( state, relationPropertyName + ".Id=" + id ).listChildren( info.Name );

        }

        public static String GetSameTypeIds( Type throughType, Type t, int id ) {

            // 1029
            ObjectInfo state = new ObjectInfo( throughType );
            String relationPropertyName = state.EntityInfo.GetRelationPropertyName( t );
            EntityPropertyInfo info = state.EntityInfo.FindRelationProperty( t );
            String ids = ObjectDB.Find(state, relationPropertyName + ".Id=" + id ).get( info.Name + ".Id" );
            EntityPropertyInfo property = state.EntityInfo.GetProperty( relationPropertyName );


            String sql = String.Format( "select distinct {0} from {1} where {2} in ({3}) and {0}<>{4}", property.ColumnName, state.EntityInfo.TableName, info.ColumnName, ids, id );

            IDbCommand command = DataFactory.GetCommand( sql, DbContext.getConnection( state.EntityInfo ) );
            IDataReader rd = null;
            StringBuilder builder = new StringBuilder();
            try {
                rd = command.ExecuteReader();
                while (rd.Read()) {
                    builder.Append( rd[0] );
                    builder.Append( "," );
                }
            }
            catch (Exception exception) {
                logger.Error( exception.Message );
                throw exception;
            }
            finally {
                OrmHelper.CloseDataReader( rd );
            }
            return builder.ToString().TrimEnd( ',' );
        }

    }
}

