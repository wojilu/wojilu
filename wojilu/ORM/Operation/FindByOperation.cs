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

using wojilu;
using wojilu.Data;
using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using wojilu.ORM.Caching;

namespace wojilu.ORM.Operation {


    internal class FindByOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FindByOperation ) );

        public static Query Find( String condition, ObjectInfo state ) {
            Includer includer = state.Includer;
            if (strUtil.HasText( includer.SelectedProperty ) && (includer.SelectedProperty != "*")) {
                Query query = new Query( condition, state );
                return query.select( includer.SelectedProperty );
            }
            return new Query( condition, state );
        }

        internal static IList Find( ConditionInfo condition ) {

            IList results = ObjectPool.FindByQuery( condition );
            if (results != null) return results;

            if ("*".Equals( condition.SelectedItem )) {
                condition.State.includeAll();
            }
            else {
                condition.State.include( SqlBuilder.GetIncludeProperty( condition.SelectedItem ) );
            }

            IList includeEntityPropertyList = condition.State.Includer.EntityPropertyList;
            IDbCommand cmd = DataFactory.GetCommand( condition.Sql, DbContext.getConnection( condition.State.EntityInfo ) );
            foreach (String key in condition.Parameters.Keys) {
                DataFactory.SetParameter( cmd, key, condition.Parameters[key] );
            }

            Hashtable hashtable = new Hashtable();
            IDataReader record = null;
            results = new ArrayList();
            try {
                record = cmd.ExecuteReader();
                while (record.Read()) {
                    EntityPropertyUtil.Fill_EntityProperty_Ids( record, includeEntityPropertyList, ref hashtable );
                    results.Add( FillUtil.Populate( record, condition.State ) );
                }
            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                throw ex;
            }
            finally {
                OrmHelper.CloseDataReader( record );
            }

            if (results.Count == 0) return results;

            return EntityPropertyUtil.setEntityProperty( condition.State, results, hashtable );
        }
    }
}

