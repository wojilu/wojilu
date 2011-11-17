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

using wojilu.Data;
using wojilu.Log;
using wojilu.ORM.Utils;
using wojilu.ORM.Operation;

namespace wojilu.ORM {

    [Serializable]
    internal class SqlBuilder {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SqlBuilder ) );

        //-------------------------初始化-------------------------

        private IList _propertyList;
        private EntityInfo _entityInfo;

        public SqlBuilder( Type type ) {
            _entityInfo = Entity.GetInfo( type );
            _propertyList = _entityInfo.SavedPropertyList;
        }

        public SqlBuilder( EntityInfo entityInfo ) {
            _entityInfo = entityInfo;
            _propertyList = _entityInfo.SavedPropertyList;
        }

        //-------------------------Sql语句-------------------------

        public String GetFindConditionSql( String propertyString, String condition, Dictionary<String, Object> paramMap, int maxResults ) {

            String[] arrConditonResult = getConditionResult( propertyString, condition, paramMap );
            String sql = "";
            if (maxResults > 0) {
                sql = String.Format( "select {0} from {1}{2}", arrConditonResult[0], arrConditonResult[1], arrConditonResult[2] );
                sql = _entityInfo.Dialect.GetLimit( sql, maxResults );
            }
            else {
                sql = String.Format( "select {0} from {1}{2}", arrConditonResult[0], arrConditonResult[1], arrConditonResult[2] );
            }

            return sql;
        }

        public String GetFindById( int id, String propertyString ) {
            String columnString = getColumnsByProperty( propertyString );
            String sql = String.Format( "select {0} from {1} where Id={2}", columnString, _entityInfo.TableName, id );
            logger.Info( String.Format( "{0}[{1}_FindById] {2}", LoggerUtil.SqlPrefix, _entityInfo.Name, sql ) );
            return sql;
        }

        // TODO
        public String GetFindById( String pkeyValue ) {
            String pkeyName = ""; //_entityInfo.getPrimaryKeyName();
            String sql = String.Format( "select * from {0} where {1}={2}", _entityInfo.TableName, pkeyName, pkeyValue );
            logger.Info( String.Format( "{0}[{1}_FindById] {2}", LoggerUtil.SqlPrefix, _entityInfo.Name, sql ) );
            return sql;
        }

        public String GetDeleteSql( String condition ) {

            String where = condition.Trim().ToLower();
            where = where.StartsWith( "where " ) ? where : "where "+ condition;
            return string.Format( "delete from {0} {1}", _entityInfo.TableName, where );
        }

        public String GetCountSql( String condition ) {
            String[] arrConditonResult = getConditionResult( "", condition, new Dictionary<String, Object>() );
            return String.Format( "select count(*) from {0}{1}", arrConditonResult[1], arrConditonResult[2] );
        }

        public String GetPropertyList( String tbl, String selectColumn, String idlist ) {
            String sql;
            if (idlist.IndexOf( "," ) < 0) {
                sql = "select " + selectColumn + " from " + tbl + " where Id=" + idlist;
            }
            else {
                sql = "select " + selectColumn + " from " + tbl + " where Id in (" + idlist + ")";
            }
            logger.Info( LoggerUtil.SqlPrefix + "[" + _entityInfo.Name + "_FindPropertyList]" + sql );
            return sql;
        }

        public String GetPageSql( PageCondition pc ) {

            String columnString = getColumnsByProperty( pc.Property );

            //------------------------ condition ------------------------------
            String joinTable = "";
            pc.ConditionStr = processCondition( pc.ConditionStr, new Dictionary<String, Object>(), ref joinTable );

            Boolean isJoin = false;
            if (strUtil.HasText( joinTable )) {
                isJoin = true;
                joinTable = _entityInfo.TableName + " t0, " + joinTable;

                // columnString中的每个columnName都要加上t0前缀
                columnString = addJoinTablePrefix( columnString, ',' ); 
            }
            else {
                joinTable = _entityInfo.TableName;
            }
            joinTable = joinTable.Trim().TrimEnd( ',' );


            if (strUtil.HasText( pc.ConditionStr )) {
                if (pc.ConditionStr.ToLower().Trim().StartsWith( "and " )) {

                    //"and id=7" 剔除and开头
                    pc.ConditionStr = " where " + pc.ConditionStr.Trim().Substring( 3 ); 
                }
                //"order by id" 加上一个空格
                else if (pc.ConditionStr.Trim().ToLower().StartsWith( "order" )) { 
                    pc.ConditionStr = " " + pc.ConditionStr;
                }
                else {
                    //加上 where
                    pc.ConditionStr = " where " + pc.ConditionStr; 
                }
            }

            //------------------------ order ------------------------------
            Boolean isCustomerOrder = false;
            if (pc.ConditionStr.ToLower().IndexOf( " order " ) >= 0) isCustomerOrder = true;

            String orderString = "";
            pc.OrderStr = pc.OrderStr.ToLower();

            // 默认排序方式：逆序
            if (isCustomerOrder == false) {

                if (pc.OrderStr == "asc")
                    orderString = " order by Id asc";
                else
                    orderString = " order by Id desc";

                if (isJoin) orderString = orderString.Replace( "Id", "t0.Id" );

            }
            else {
                // 自定义排序方式
                String newOrderString = "";

                String tempString = pc.ConditionStr;
                //此处conditionString已经不带order
                pc.ConditionStr = pc.ConditionStr.Substring( 0, pc.ConditionStr.ToLower().IndexOf( "order " ) ); 

                orderString = tempString.Replace( pc.ConditionStr, "" );

                String[] arrOrder = orderString.Split( new char[] { ' ', ',' } );
                foreach (String s in arrOrder) {
                    if (s.ToLower() == "asc" || s.ToLower() == "desc") {
                        newOrderString += s + ", ";
                    }
                    else if (s.ToLower() != "order" && s.ToLower() != "by" && s != null && s != "") {
                        if (isJoin)
                            newOrderString += "t0." + s + " ";
                        else
                            newOrderString += s + " ";
                    }
                    else {
                        newOrderString += s + " ";
                    }
                }

                orderString = newOrderString.Trim().TrimEnd( ',' );
            }

            String _condition_nowhere = pc.ConditionStr.Trim();
            if (strUtil.HasText( _condition_nowhere ) && _condition_nowhere.ToLower().StartsWith( "where" )) {
                _condition_nowhere = pc.ConditionStr.Trim().Substring( 5 );
                _condition_nowhere = "and " + _condition_nowhere;
            }

            String sql = "";

            String pageCountSql = "select count(*) from " + joinTable + pc.ConditionStr;

            //callback执行beginCount方法
            pc.CurrentPage = pc.beginCount( pageCountSql, pc.Pager, _entityInfo );

            // TODO：最后一页优化

            if (pc.CurrentPage == 1) {
                sql = "select " + columnString + " from " + joinTable + pc.ConditionStr + orderString;

                sql = _entityInfo.Dialect.GetLimit( sql, pc.Size );
            }
            else {

                StringBuilder sb = new StringBuilder();
                getPageSql( pc, columnString, joinTable, isJoin, isCustomerOrder, orderString, _condition_nowhere, sb );

                sql = sb.ToString();

            }

            logger.Info( LoggerUtil.SqlPrefix + "[Page Sql] " + sql );
            return sql;
        }

        private void getPageSql( PageCondition pc, String columnString, String joinTable, Boolean isJoin, Boolean isCustomerOrder, String orderString, String _condition_nowhere, StringBuilder sb ) {

            if (_entityInfo.DbType == DatabaseType.MySql) {

                int startRow = pc.Size * (pc.CurrentPage - 1);
                
                sb.Append( "select " );
                sb.Append( columnString );
                sb.Append( " from " );
                sb.Append( joinTable );
                sb.Append( " " );
                sb.Append( pc.ConditionStr );
                sb.Append( " " );
                sb.Append( orderString );
                sb.Append( " limit " );
                sb.Append( startRow );
                sb.Append( ", " );
                sb.Append( pc.Size );

                return;
            }

            if (isCustomerOrder == false) {

                sb.Append( "select top " );
                sb.Append( pc.Size );
                sb.Append( " " );
                sb.Append( columnString );
                sb.Append( " from " );
                sb.Append( joinTable );

                if (pc.OrderStr == "asc") {
                    if (isJoin)
                        sb.Append( " where t0.Id>(select max(Id) from (select top " );
                    else
                        sb.Append( " where Id>(select max(Id) from (select top " );
                }
                else {
                    if (isJoin)
                        sb.Append( " where t0.Id<(select min(Id) from (select top " );
                    else
                        sb.Append( " where Id<(select min(Id) from (select top " );
                }

                sb.Append( pc.Size * (pc.CurrentPage - 1) );

                if (isJoin)
                    sb.Append( " t0.Id from " );
                else
                    sb.Append( " Id from " );

                sb.Append( joinTable );
                sb.Append( pc.ConditionStr );
                sb.Append( orderString );
                sb.Append( ") as t) " );
                //sb.Append( "and " );
                sb.Append( _condition_nowhere );
                sb.Append( orderString );

            }

            else if (_entityInfo.DbType == DatabaseType.Access || _entityInfo.DbType == DatabaseType.SqlServer2000) {

                sb.Append( "select top " );
                sb.Append( pc.Size );
                sb.Append( " " );
                sb.Append( columnString );
                sb.Append( " from " );
                sb.Append( joinTable );

                if (strUtil.IsNullOrEmpty( pc.ConditionStr )) {
                    if (isJoin)
                        sb.Append( " where  t0.Id not in ( select top " );
                    else
                        sb.Append( " where  Id not in ( select top " );

                }
                else {

                    sb.Append( pc.ConditionStr );

                    if (isJoin)
                        sb.Append( " and  t0.Id not in ( select top " );
                    else
                        sb.Append( " and  Id not in ( select top " );

                }

                sb.Append( pc.Size * (pc.CurrentPage - 1) );

                if (isJoin)
                    sb.Append( " t0.Id from " );
                else
                    sb.Append( " Id from " );

                sb.Append( joinTable );
                sb.Append( pc.ConditionStr );
                sb.Append( orderString );
                sb.Append( " ) " );
                //sb.Append( "and " );
                sb.Append( _condition_nowhere );
                sb.Append( " " );
                sb.Append( orderString );

            }

            else {

                int startRow = pc.Size * (pc.CurrentPage - 1);
                int endRow = startRow + pc.Size;

                sb.AppendFormat( "select {0} from ", columnString );
                sb.Append( "(" );
                sb.AppendFormat( "select ROW_NUMBER() over ( {0} ) as rowNumber, {1} from {2} {3}", orderString, columnString, joinTable, pc.ConditionStr );
                sb.Append( ") as tmpTable " );
                sb.AppendFormat( "where (rowNumber between {0} and {1}) {2}", (startRow + 1), endRow, _condition_nowhere );

            }
        }

        //-------------------------帮助方法-------------------------

        private String[] getConditionResult( String propertyString, String condition, Dictionary<String, Object> paramMap ) {

            String columnString = getColumnsByProperty( propertyString );

            String joinTable = "";

            String conditionString = condition;
            if (strUtil.HasText( conditionString )) {
                conditionString = processCondition( conditionString, paramMap, ref joinTable ).Trim();
            }

            if (strUtil.HasText( joinTable )) {
                joinTable = _entityInfo.TableName + " t0, " + joinTable;
                // columnString中的每个columnName都要加上t0前缀
                columnString = addJoinTablePrefix( columnString, ',' ); 
            }
            else {
                joinTable = _entityInfo.TableName;
            }
            joinTable = joinTable.Trim().TrimEnd( ',' );

            if (strUtil.HasText( conditionString )) {
                if (conditionString.ToLower().StartsWith( "and" )) {
                    //"and id=7" 剔除and开头
                    conditionString = " where " + conditionString.Substring( 3 ).Trim(); 
                }
                //"order by id" 加上一个空格
                else if (conditionString.ToLower().StartsWith( "order" )) { 
                    conditionString = " " + conditionString.Trim();
                }
                else {
                    //加上 where
                    conditionString = " where " + conditionString.Trim(); 
                }
            }
            return new String[] { columnString, joinTable, conditionString };
        }

        // 示例：Id,Title,Member.Id,Cat.Name
        public String getColumnsByProperty( String propertyString ) {

            if (!strUtil.HasText( propertyString ) || propertyString.Trim().Equals( "*" )) return "*";

            //将 propertyString 与 当前实体的所有属性比较，得到 columnList
            String[] arrProperty = propertyString.Split( ',' );
            String result = "";
            foreach (EntityPropertyInfo ep in _propertyList) {
                foreach (String pString in arrProperty) {

                    if (isAddColumn( pString.Trim(), ep )) {
                        //实体属性的数据，这里不联表查询。而是获取相应id，通过id in(idlist) 得到数据再填充
                        result += ep.ColumnName + ","; 
                        break;
                    }

                }
            }
            return result.Trim().TrimEnd( ',' );
        }

        // 是否在sql中选择当前属性的column
        private Boolean isAddColumn( String selectedProperty, EntityPropertyInfo ep ) {

            //选择字符串和属性完全同名
            if (selectedProperty.Equals( ep.Name )) return true;

            //属性Member,selectProperty为Member.Name，只要前缀相同，即可选取属性的columnName
            if (ep.IsEntity && selectedProperty.StartsWith( ep.Name + "." )) return true;

            return false;
        }

        // 示例：Member.Id=6 and Cat.Id>7
        // art.Find("Author=:author and Title=:title and Id>:id order by Id desc")
        public String processCondition( String conditionString, Dictionary<String, Object> paramMap, ref String joinTable ) {

            //logger.Info( "conditionString=>" + conditionString );


            // 根据不同方言，替换参数，比如“Author=:author and Title=:title”需要替换成“Author=? and Title=?”
            foreach (String key in paramMap.Keys) {
                conditionString = conditionString.Replace( ":" + key, _entityInfo.Dialect.GetParameter( key ) );
            }

            //如果是实体属性在设置条件，还要负责把 Member.Id=:mid 转换成 TMemberId=?：不联表查询
            // 1、当前实体具有的column ： _entityInfo.ColumnList
            // 2、找到参数查询所对应的column
            if (isConditionHasEntity( conditionString )) {

                //是否联表查询
                Boolean isJoinedQuery = false;
                String _join_condition = "";
                int _joined_Property_Count = 0;

                //  and (Member.Id=? and Cat.Name=?) order by Id desc
                List<String[]> items = SqlUtil.getEntityProperties( conditionString );
                foreach (String[] pair in items) {

                    String strItem = pair[0] + "." + pair[1];

                    if (!pair[1].ToLower().Equals( "id" )) isJoinedQuery = true;

                    if (!isJoinedQuery) {
                        conditionString = conditionString.Replace( strItem, _entityInfo.GetColumnName( pair[0] ) );
                    }
                    else {

                        _joined_Property_Count += 1;

                        EntityPropertyInfo _e_p = _entityInfo.GetProperty( pair[0] );

                        EntityPropertyInfo _sub_p = _e_p.EntityInfo.GetProperty( pair[1] );
                        String subColumn = (_sub_p == null ? pair[1] : _sub_p.ColumnName);
                        String _sub_column_name = "t" + _joined_Property_Count + "." + subColumn;

                        // joinTable
                        joinTable += _e_p.EntityInfo.TableName + " t" + _joined_Property_Count + ",";

                        // replace entity property
                        conditionString = conditionString.Replace( strItem, _sub_column_name );
                        _join_condition += "and t0." + _e_p.ColumnName + "=t" + _joined_Property_Count + ".Id ";

                    }
                }

                // add prefix "0" to all current entity property's column, especially in "order by id desc" etc.(=>order by t0.id desc)
                if (isJoinedQuery) conditionString = addJoinTablePrefix( conditionString, ' ' );

                //conditionString = _join_condition + conditionString;
                conditionString = conditionString.Trim();
                _join_condition = _join_condition.Trim();
                if (conditionString.StartsWith( "and " ) == false && _join_condition.EndsWith( " and" ) == false) {
                    conditionString =_join_condition + " and " + conditionString;
                }
                else {
                    conditionString = _join_condition + " " + conditionString;
                }
            }

            return conditionString;
        }

        // " and  ( ControlPath  =  '~/Module/BlogMain.ascx' )  order by Id desc"
        // " and Id>6 and Member.Name = 'zhang' "
        // " and UnitPrice = 32.80 "
        private Boolean isConditionHasEntity( String conditionString ) {


            foreach (EntityPropertyInfo ep in this._entityInfo.EntityPropertyList) {

                if (conditionString.IndexOf( " " + ep.Name + "." ) > -1) return true;
                if (conditionString.StartsWith( ep.Name + "." )) return true;
                if (conditionString.IndexOf( "(" + ep.Name + "." ) > -1) return true;
            }
            return false;
        }

        // 用于1)select columnNames 
        // 2)where 中，比如ReceiverId=1 and IsDelete=0
        private String addJoinTablePrefix( String targetString, char seperator ) {
            if (strUtil.IsNullOrEmpty( targetString )) return null;

            String[] _arr_Str;
            if (targetString.Equals( "*" )) {
                _arr_Str = _entityInfo.ColumnList.Split( seperator );
            }
            else {
                _arr_Str = targetString.Split( seperator );
            }

            String _result_string = "";
            for (int i = 0; i < _arr_Str.Length; i++) {
                String _str_item = _arr_Str[i].Trim(); //ReceiverId=1
                if (!strUtil.HasText( _str_item )) continue;

                if (_str_item.IndexOf( '=' ) > 0) {

                    String[] _arrItemPair = _str_item.Split( '=' );

                    _str_item = checkColumn( _arrItemPair[0] ) + "=" + _arrItemPair[1];


                }
                else {
                    _str_item = checkColumn( _str_item );
                }

                _result_string += _str_item + seperator;
            }
            return _result_string.TrimEnd( seperator ).Trim();
        }

        private String checkColumn( String orginalColumnName ) {

            if (orginalColumnName.Equals( "=" ) ||
                orginalColumnName.ToLower().Equals( "and" ) ||
                orginalColumnName.ToLower().Equals( "or" ) ||
                orginalColumnName.Equals( "?" ) ||
                orginalColumnName.Equals( "(" ) ||
                orginalColumnName.Equals( ")" ) ||
                orginalColumnName.Equals( ">" ) ||
                orginalColumnName.Equals( "<" ) ||
                orginalColumnName.Equals( "order" ) ||
                orginalColumnName.ToLower().Equals( "by" ) ||
                orginalColumnName.ToLower().Equals( "asc" ) ||
                orginalColumnName.ToLower().Equals( "desc" ) ||
                orginalColumnName.Equals( "<>" ) ||
                orginalColumnName.ToLower().Equals( "like" ))

                return orginalColumnName;

            if (orginalColumnName.IndexOf( "." ) > 0) return orginalColumnName;

            foreach (EntityPropertyInfo ep in _propertyList) {
                if (orginalColumnName.ToLower().Equals( ep.ColumnName.ToLower() )) {
                    return "t0." + orginalColumnName;
                }
            }

            return orginalColumnName;
        }

        //String sql = "select Id,Name,ArticleCount from TCat";
        //select top 1 Id,CategoryId,Name,FriendUrl,Gender,Pic,TemplateId from (note ： top 1)
        public static String GetColumnString( String sql ) {
            sql = sql.Trim();
            int fromIndex = sql.ToLower().IndexOf( "from" );
            sql = sql.Substring( 0, fromIndex ).Trim();
            sql = sql.Substring( 7 ); // delete "select"

            // TODO:database is mysql : don't use "top"
            if (sql.ToLower().IndexOf( "top" ) > -1) {
                String topOne = sql.Split( ',' )[0].Trim(); //=>top 1 Id
                String[] arrTopOne = topOne.Split( ' ' );//=>["top","1","Id"]
                String firstColumn = arrTopOne[arrTopOne.Length - 1];//=>Id
                sql = sql.Replace( topOne, firstColumn );
            }

            sql = sql.Replace( " ", "" );
            return sql;
        }

        //Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name
        public static String GetIncludeProperty( String propertyString ) {
            String[] _arr_property = propertyString.Trim().Split( ',' );
            StringBuilder sb = new StringBuilder( "," );
            foreach (String propertyItem in _arr_property) {
                String p = propertyItem.Trim();
                if (p.IndexOf( "." ) > 0) {
                    String propertyName = p.Split( '.' )[0];
                    if (sb.ToString().IndexOf( "," + propertyName + "," ) < 0) { //avoid repeat property
                        sb.Append( propertyName );
                        sb.Append( "," );
                    }
                }
            }
            return sb.ToString().TrimStart( ',' ).TrimEnd( ',' );
        }

        // Id,Title,Member.Id,Cat.Name,Cat.ArticleCount,Board.Name
        // => 3 hashtable
        // Hashtable["Member"] = "Id", Hashtable["Cat"]="Name,ArticleCount", Hashtable["Board"]="Name"
        public static Hashtable GetIncludeSelectColumn( String propertyString, IList entityPropertyList ) {
            Hashtable result = new Hashtable();
            if (propertyString.Equals( "*" )) return result;

            String[] _arr_property = propertyString.Trim().Split( ',' );

            foreach (String propertyItem in _arr_property) {

                String p = propertyItem.Trim();
                if (p.IndexOf( "." ) < 0) continue;

                String[] _arr_p_item = p.Split( '.' ); //Cat.ArticleCount
                String propertyName = _arr_p_item[0]; //Cat
                String subPropertyName = _arr_p_item[1]; //ArticleCount

                if (result[propertyName] == null) { //first time fill column
                    result[propertyName] = getColumnNameFromPropertyList( entityPropertyList, propertyName, subPropertyName ) + ",";
                }
                else {
                    result[propertyName] = result[propertyName].ToString() + getColumnNameFromPropertyList( entityPropertyList, propertyName, subPropertyName ) + ",";
                }
            }

            return result;
        }

        private static String getColumnNameFromPropertyList( IList entityPropertyList, String propertyName, String subPropertyName ) {
            foreach (EntityPropertyInfo p in entityPropertyList) {
                if (p.Name.Equals( propertyName )) {
                    return p.EntityInfo.GetProperty( subPropertyName ).ColumnName;
                }
            }
            return null;
        }

    }
}
