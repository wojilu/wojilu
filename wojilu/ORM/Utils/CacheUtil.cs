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
using System.Text;

using wojilu;
using wojilu.Data;
using wojilu.Reflection;
using wojilu.ORM.Caching;

namespace wojilu.ORM {

    internal class CacheUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( CacheUtil ) );

        public static void CheckCountCache( String action, IEntity obj, EntityInfo entityInfo ) {

            for (int i = 0; i < entityInfo.SavedPropertyList.Count; i++) {
                IEntity container = null;
                String propertyName = null;
                EntityPropertyInfo info = entityInfo.SavedPropertyList[i];
                if ((info.Name != "Id") && !(info.Name == "OID")) {
                    ICacheAttribute attribute = ReflectionUtil.GetAttribute( info.Property, typeof( CacheCountAttribute ) ) as ICacheAttribute;
                    if (attribute != null) {
                        container = ReflectionUtil.GetPropertyValue( obj, info.Name ) as IEntity;
                        propertyName = attribute.TargetPropertyName.Replace( " ", "" );
                    }
                    if (container != null) {
                        String columnName = Entity.GetInfo( container ).GetColumnName( propertyName );
                        setCountCacheBySql( container, columnName, action );
                    }
                }
            }
        }

        public static void setCountCacheBySql( IEntity container, String columName, String action ) {

            EntityInfo et = Entity.GetInfo( container );

            StringBuilder builder = new StringBuilder();
            builder.Append( "update " );
            builder.Append( et.TableName );
            builder.Append( " set " );
            builder.Append( columName );
            builder.Append( "=" );
            builder.Append( columName );
            if (action.ToLower() == "insert") {
                builder.Append( "+1" );
            }
            else {
                builder.Append( "-1" );
            }
            builder.AppendFormat( " where id={0}", container.Id );
            DataFactory.GetCommand( builder.ToString(), DbContext.getConnection( et ) ).ExecuteNonQuery();
            logger.Debug( "set counter cache : " + builder.ToString() );

            CacheTime.updateTable( et.Type );

        }

    }
}

