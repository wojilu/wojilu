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

using wojilu;
using wojilu.Log;

namespace wojilu.ORM.Operation {


    internal class FindAllOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FindAllOperation ) );

        public static IList FindAll( ObjectInfo state ) {


            IList parentResults = findAllPrivate( state );
            if (state.EntityInfo.ChildEntityList.Count > 0) {
                return findAllFromChild( parentResults, state );
            }
            return parentResults;
        }

        private static IList findAllPrivate( ObjectInfo state ) {

            String sql = "select * from " + state.EntityInfo.TableName;
            logger.Info( LoggerUtil.SqlPrefix + "[" + state.EntityInfo.Name + "_FindAll]" + sql );

            return EntityPropertyUtil.FindList( state, sql );
        }

        private static IList findAllFromChild( IList parents, ObjectInfo state ) {

            ArrayList results = new ArrayList();

            foreach (EntityInfo info in state.EntityInfo.ChildEntityList) {

                ObjectInfo childState = new ObjectInfo( info);
                childState.includeAll();
                IList children = ObjectDB.FindAll( childState );

                for (int i = 0; i < children.Count; i++) {
                    IEntity child = children[i] as IEntity;
                    // state
                    //child.state.Order = state.Order;
                    results.Add( child );
                    parents.RemoveAt( Query.getIndexOfObject( parents, child ) );
                }

            }

            if (parents.Count > 0) results.AddRange( parents );
            results.Sort();

            return results;
        }

    }
}

