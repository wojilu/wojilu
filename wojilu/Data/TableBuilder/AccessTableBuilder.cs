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
using System.Text;
using System.Collections;
using wojilu.ORM;

namespace wojilu.Data {

    internal class AccessTableBuilder : TableBuilderBase {
        
        private Boolean isAddIdentityKey( Type t ) {
            if (OrmHelper.IsEntityBase( t.BaseType )) return true;
            if (t.BaseType.IsAbstract) return true;
            return false;
        }

        protected override void addColumn_Long( StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " long, " );
        }

        protected override void addColumn_Decimal( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            if (ep.MoneyAttribute != null) {
                sb.Append( columnName );
                sb.Append( " currency default 0, " );
            }
            else {

                DecimalAttribute da = ep.DecimalAttribute;
                if (da == null) throw new Exception( "DecimalAttribute not found=" + entity.FullName + "_" + ep.Name );

                sb.Append( columnName );
                sb.Append( " decimal(" + da.Precision + "," + da.Scale + ") default 0, " );
            }
        }
        
        protected override void addColumn_MiddleText( EntityInfo entity, StringBuilder sb, EntityPropertyInfo temP, string columnName ) {
            addColumn_LongText( entity, sb, columnName );
        }

    }

}
