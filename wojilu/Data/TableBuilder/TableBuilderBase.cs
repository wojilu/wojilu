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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.Log;

namespace wojilu.Data {

    internal class TableBuilderBase {


        private static readonly ILog logger = LogManager.GetLogger( typeof( TableBuilderBase ) );

        public List<String> CheckMappingTableIsExist( IDbCommand cmd, String db, List<String> existTables, MappingClass mapping ) {
            foreach (DictionaryEntry entry in mapping.ClassList) {
                EntityInfo entity = entry.Value as EntityInfo;
                if (entity.Database.Equals( db ) == false) continue;

                if (!isTableCreated( existTables, entity )) {
                    existTables = createTable( entity, cmd, existTables, mapping.ClassList );
                }
            }
            return existTables;
        }

        private List<String> createTable( EntityInfo entity, IDbCommand cmd, List<String> existTables, IDictionary clsList ) {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "Create Table {0} (", getFullName( entity.TableName, entity ) );
            addColumn_PrimaryKey( entity, sb, clsList );

            addColumns( entity, sb );
            String str = sb.ToString().Trim().TrimEnd( new char[] { ',' } ) + " )";

            cmd.CommandText = str;
            logger.Info( "create table:" + str );
            if (cmd.Connection == null) throw new Exception( "connection is null" );

            if (cmd.Connection.State == ConnectionState.Closed) {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();

            existTables.Add( entity.TableName );
            logger.Info( LoggerUtil.SqlPrefix + String.Format( "create table {0} ({1})", entity.TableName, entity.FullName ) );

            return existTables;
        }

        private void addColumns( EntityInfo entity, StringBuilder sb ) {
            for (int i = 0; i < entity.SavedPropertyList.Count; i++) {
                EntityPropertyInfo ep = entity.SavedPropertyList[i];
                String columnName = getFullName( ep.ColumnName, entity );
                if ((ep.SaveToDB && !ep.IsList) && !(ep.Name == "Id")) {
                    addColumnSingle( entity, sb, ep, columnName );
                }
            }
        }

        private void addColumnSingle( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            if (ep.Type == typeof( int )) {
                addColumn_Int( sb, ep, columnName );
            }
            else if (ep.Type == typeof( long )) {
                addColumn_Long( sb, columnName );
            }
            else if (ep.Type == typeof( DateTime )) {
                addColumn_Time( sb, columnName );
            }
            else if (ep.Type == typeof( decimal )) {
                addColumn_Decimal( entity, sb, ep, columnName );
            }
            else if (ep.Type == typeof( double )) {
                addColumn_Double( entity, sb, columnName );
            }
            else if (ep.Type == typeof( float )) {
                addColumn_Single( entity, sb, columnName );
            }
            else if (ep.Type == typeof( String )) {
                addColumn_String( entity, sb, ep, columnName );
            }
            else if (ep.IsEntity) {
                addColumn_entity( sb, columnName );
            }
        }



        //------------------------------------------------------------------------------------------------------------------------

        protected virtual void addColumn_PrimaryKey( EntityInfo entity, StringBuilder sb, IDictionary clsList ) {
            // 不是自动编号
            if (!DbConfig.Instance.IsAutoId || isAddIdentityKey( entity.Type ) == false) {
                sb.Append( " Id int primary key default 0, " );
            }
            else {
                sb.Append( " Id int identity(1,1) primary key, " );
            }
        }

        private Boolean isAddIdentityKey( Type t ) {
            if (OrmHelper.IsEntityBase( t.BaseType )) return true;
            if (t.BaseType.IsAbstract) return true;
            return false;
        }


        protected virtual void addColumn_Int( StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            sb.Append( columnName );
            if (ep.Property.IsDefined( typeof( TinyIntAttribute ), false )) {
                sb.Append( " tinyint default 0, " );
            }
            else {
                sb.Append( " int default 0, " );
            }
        }

        protected virtual void addColumn_Long( StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " bigint, " );
        }

        protected virtual void addColumn_Time( StringBuilder sb, String columnName ) {
            sb.Append( columnName );
            sb.Append( " DateTime, " );
        }

        protected virtual void addColumn_Decimal( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            if (ep.MoneyAttribute != null) {
                sb.Append( columnName );
                sb.Append( " money default 0, " );
            }
            else {

                DecimalAttribute da = ep.DecimalAttribute;
                if (da == null) throw new Exception( "DecimalAttribute not found=" + entity.FullName + "_" + ep.Name );

                sb.Append( columnName );
                sb.Append( " decimal(" + da.Precision + "," + da.Scale + ") default 0, " );
            }
        }

        protected virtual void addColumn_Double( EntityInfo entity, StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " float default 0, " );
        }


        protected virtual void addColumn_Single( EntityInfo entity, StringBuilder sb, string columnName ) {
            sb.Append( columnName );
            sb.Append( " real default 0, " );
        }


        protected virtual void addColumn_String( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            if (ep.LongTextAttribute != null) {
                addColumn_LongText( entity, sb, columnName );
            }
            else if (ep.SaveAttribute != null) {
                addColumn_ByColumnAttribute( entity, sb, ep, columnName );
            }
            else {
                addColumn_ShortText( sb, columnName, 250 );
            }
        }


        protected virtual void addColumn_LongText( EntityInfo entity, StringBuilder sb, String columnName ) {
            sb.Append( columnName );
            sb.Append( " ntext, " );

        }

        protected virtual void addColumn_ShortText( StringBuilder sb, String columnName, int length ) {
            sb.Append( columnName );
            sb.Append( " nvarchar(" );
            sb.Append( length );
            sb.Append( "), " );
        }


        protected virtual void addColumn_ByColumnAttribute( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {
            if (ep.SaveAttribute.Length < 255) {
                addColumn_ShortText( sb, columnName, ep.SaveAttribute.Length );
            }
            else if ((ep.SaveAttribute.Length > 255) && (ep.SaveAttribute.Length < 4000)) {
                addColumn_MiddleText( entity, sb, ep, columnName );
            }
            else {
                addColumn_LongText( entity, sb, columnName );
            }
        }

        protected virtual void addColumn_MiddleText( EntityInfo entity, StringBuilder sb, EntityPropertyInfo ep, String columnName ) {

            addColumn_ShortText( sb, columnName, ep.SaveAttribute.Length );
        }


        protected virtual void addColumn_entity( StringBuilder sb, String columnName ) {
            sb.Append( columnName );
            sb.Append( " int default 0, " );
        }


        //------------------------------------------------------------------------------------------------------------------------

        private String getFullName( String name, EntityInfo entity ) {
            if (DbConst.SqlKeyWords.Contains( name.ToLower() )) {
                String message = String.Format( "'{0}'  is reserved word. Entity:{1}, Table:{2}", name, entity.FullName, entity.TableName );
                logger.Info( message );
                throw new Exception( message );
            }
            return name;
        }

        private Boolean isTableCreated( IList existTables, EntityInfo entity ) {
            for (int i = 0; i < existTables.Count; i++) {
                if (string.Compare( existTables[i].ToString(), entity.TableName.Replace( "[", "" ).Replace( "]", "" ), true ) == 0) {
                    logger.Info( "table map : " + entity.FullName + " => " + existTables[i] );
                    return true;
                }
            }
            return false;
        }

    }

}
