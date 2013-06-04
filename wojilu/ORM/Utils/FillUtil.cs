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
using wojilu.ORM.Caching;

namespace wojilu.ORM {


    internal class FillUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FillUtil ) );

        public static Object Populate( IDataRecord rd, ObjectInfo state ) {

            IEntity obj = Entity.New( state.EntityInfo.Type.FullName );
            // state
            //obj.state.Order = state.Order;

            for (int i = 0; i < rd.FieldCount; i++) {

                Object fdvalue = rd[i];

                if (fdvalue == null || fdvalue == DBNull.Value) continue;

                EntityPropertyInfo ep = state.EntityInfo.GetPropertyByColumn( rd.GetName( i ) );
                if (ep == null) continue;

                try {
                    if (ep.IsEntity || ep.IsAbstractEntity) {
                        setEntityPropertyValueById( obj, state, ep, rd.GetInt32( i ) );
                    }
                    else {
                        ep.SetValue( obj, getReaderValue( rd, i, fdvalue, ep.Type ) );
                    }

                }
                catch (Exception ex) {
                    logger.Error( ex.Message + "=" + ep.Name + "_" + ep.Type );
                    logger.Error( ex.StackTrace );
                    throw ex;
                }

            }

            return obj;
        }

        private static void setEntityPropertyValueById( IEntity obj, ObjectInfo state, EntityPropertyInfo property, int pid ) {

            if (!property.IsAbstractEntity) {
                IEntity objValue = Entity.New( property.Type.FullName );
                objValue.Id = pid;
                // state
                //objValue.state = new ObjectInfo( property.Type ).Copy( state );
                IEntity objCache = ObjectPool.FindOne( property.Type, objValue.Id );
                if (objCache != null) {
                    objValue = objCache;
                }
                obj.set( property.Name, objValue );
            }
        }

        private static Object getReaderValue( IDataRecord rd, int i, Object rdValue, Type ptype ) {

            if (ptype == typeof( decimal ))
                return Convert.ToDecimal( rdValue );

            if (ptype == typeof( int ))
                return Convert.ToInt32( rdValue );

            if (ptype == typeof( long ))
                return Convert.ToInt64( rdValue );

            return rdValue;
        }



    }
}

