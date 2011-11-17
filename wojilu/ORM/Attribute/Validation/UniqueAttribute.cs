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
using wojilu.Data;
using System.Data;
using wojilu.Log;
namespace wojilu.ORM {

    /// <summary>
    /// 唯一性验证批注，验证对象的属性值是否在所有对象中是唯一的
    /// </summary>
    [Serializable]
    public class UniqueAttribute : ValidationAttribute {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UniqueAttribute ) );

        public UniqueAttribute() {
        }

        public UniqueAttribute( String msg ) {
            base.Message = msg;
        }



        public override void Validate( String action, IEntity target, EntityPropertyInfo info, Result result ) {
            Object obj = target.get( info.Name );

            EntityInfo ei = Entity.GetInfo( target );
            int count = getCount( action, target, ei, info, obj );

            if (count > 0) {
                if (strUtil.HasText( this.Message )) {
                    result.Add( this.Message );
                }

                else {
                    String str = "[" + ei.FullName + "] : property \"" + info.Name + "\" ";
                    result.Add( str + " should be unique, but it has been in database" );
                }
            }
        }

        private static int getCount( String action, IEntity target, EntityInfo entityInfo, EntityPropertyInfo info, Object obj ) {

            if (obj == null) return 1;

            String usql;
            IDatabaseDialect dialect = entityInfo.Dialect;
            if (action.Equals( "update" )) {
                usql = String.Format( "select count(Id) from {0} where Id<>{3} and {1}={2}", entityInfo.TableName, info.ColumnName, dialect.GetParameter( info.Name ), target.Id );
            }
            else {
                usql = String.Format( "select count(Id) from {0} where {1}={2}", entityInfo.TableName, info.ColumnName, dialect.GetParameter( info.Name ) );
            }

            logger.Info( LoggerUtil.SqlPrefix + " validate unique sql : " + usql );
            IDbCommand cmd = DataFactory.GetCommand( usql, DbContext.getConnection( entityInfo ) );
            DataFactory.SetParameter( cmd, info.ColumnName, obj );
            return cvt.ToInt( cmd.ExecuteScalar() );

        }

    }
}

