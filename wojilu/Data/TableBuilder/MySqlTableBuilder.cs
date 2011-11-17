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
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;

namespace wojilu.Data {

    internal class MySqlTableBuilder : TableBuilderBase {

        protected override void addColumn_PrimaryKey( EntityInfo entity, StringBuilder sb, IDictionary clsList ) {

            sb.Append( " Id int unsigned not null auto_increment primary key, " );

        }

        protected override void addColumn_Int( StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            sb.Append( columnName );
            if (ep.Property.IsDefined( typeof( TinyIntAttribute ), false )) {
                sb.Append( " tinyint unsigned default 0, " );
            }
            else {
                sb.Append( " int unsigned default 0, " );
            }
        }

        protected override void addColumn_LongText( EntityInfo entity, StringBuilder sb, String columnName ) {
            sb.Append( columnName );
            sb.Append( " text, " );
        }

        protected override void addColumn_ShortText( StringBuilder sb, String columnName, int length ) {
            sb.Append( columnName );
            sb.Append( " varchar(" );
            sb.Append( length );
            sb.Append( "), " );
        }

        protected override void addColumn_Decimal( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {

            if (ep.MoneyAttribute != null) {
                sb.Append( columnName );
                sb.Append( " decimal(19, 4) default 0, " );
            }
            else {

                DecimalAttribute da = ep.DecimalAttribute;
                if (da == null) throw new Exception( "DecimalAttribute not found=" + entity.FullName + "_" + ep.Name );

                sb.Append( columnName );
                sb.Append( " decimal(" + da.Precision + "," + da.Scale + ") default 0, " );

            }

        }


        protected override void addColumn_Double( EntityInfo entity, StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " double default 0, " );
        }


        protected override void addColumn_Single( EntityInfo entity, StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " float default 0, " );
        }




    }

}
