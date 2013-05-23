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
using System.Data;
using System.Text;

using wojilu.ORM;
using wojilu.Data;
using wojilu.ORM.Operation;
using wojilu.Web;
using wojilu.ORM.Caching;

namespace wojilu {

    /// <summary>
    /// wojilu ORM 最主要的工具，集中了对象的常用 CRUD (读取/插入/更新/删除) 操作。主要方法都是泛型方法。
    /// </summary>
    public class db {

        /// <summary>
        /// 查询 T 类型对象的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> findAll<T>() where T : IEntity {

            ObjectInfo state = new ObjectInfo( typeof( T ) );
            state.includeAll();
            IList objList = ObjectPool.FindAll( typeof( T ) );
            if (objList == null) {
                objList = ObjectDB.FindAll( state );
                ObjectPool.AddAll( typeof( T ), objList );
            }

            return getResults<T>( objList );
        }

        /// <summary>
        /// 根据 id 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T findById<T>( int id ) where T : IEntity {

            if (id < 0) return default( T );

            IEntity objCache = ObjectPool.FindOne( typeof( T ), id );
            if (objCache == null) {
                ObjectInfo state = new ObjectInfo( typeof( T ) );
                objCache = ObjectDB.FindById( id, state );
                ObjectPool.Add( objCache );
            }

            return (T)objCache;
        }

        /// <summary>
        /// 根据查询条件，返回一个查询对象。一般用于参数化查询。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">查询条件</param>
        /// <returns>返回查询对象xQuery，可以进一步参数化赋值，并得到结果</returns>
        public static xQuery<T> find<T>( String condition ) where T : IEntity {

            ObjectInfo state = new ObjectInfo( typeof( T ) );
            Query q = ObjectDB.Find( state, condition );
            return new xQuery<T>( q );
        }

        /// <summary>
        /// 根据查询条件，返回分页数据集合(默认每页返回20条记录)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">查询条件</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<T> findPage<T>( String condition ) where T : IEntity {

            return findPage<T>( condition, 20 );
        }

        /// <summary>
        /// 根据查询条件、每页数量，返回分页数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">每页需要显示的数据量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<T> findPage<T>( String condition, int pageSize ) where T : IEntity {

            if (pageSize <= 0) pageSize = 20;

            ObjectInfo state = new ObjectInfo( typeof( T ) );
            state.includeAll();
            state.Pager.setSize( pageSize );

            IPageList result = ObjectPool.FindPage( typeof( T ), condition, state.Pager );
            if (result == null) {

                IList list = ObjectDB.FindPage( state, condition );
                PageHelper p = state.Pager;
                ObjectPool.AddPage( typeof( T ), condition, p, list );

                result = new DataPageInfo();
                result.Results = list;
                result.PageCount = p.PageCount;
                result.RecordCount = p.RecordCount;
                result.Size = p.getSize();
                result.PageBar = p.PageBar;
                result.Current = p.getCurrent();
            }
            else {
                result.PageBar = new PageHelper( result.RecordCount, result.Size, result.Current ).PageBar;
            }

            return new DataPage<T>( result );
        }

        /// <summary>
        /// 根据查询条件、当前页码和每页数量，返回分页数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">查询条件</param>
        /// <param name="current">当前页码</param>
        /// <param name="pageSize">每页需要显示的数据量</param>
        /// <returns>分页数据列表，包括当前页、总记录数、分页条等</returns>
        public static DataPage<T> findPage<T>( String condition, int current, int pageSize ) where T : IEntity {

            ObjectInfo state = new ObjectInfo( typeof( T ) );
            state.includeAll();
            state.Pager.setSize( pageSize );
            state.Pager.setCurrent( current );

            IList list = ObjectDB.FindPage( state, condition );
            IPageList result = new DataPageInfo();
            result.Results = list;
            result.PageCount = state.Pager.PageCount;
            result.RecordCount = state.Pager.RecordCount;
            result.Size = state.Pager.getSize();
            result.PageBar = state.Pager.PageBar;
            result.Current = state.Pager.getCurrent();

            return new DataPage<T>( result );
        }

        /// <summary>
        /// 根据 sql 语句，返回对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> findBySql<T>( String sql ) where T : IEntity {

            IList objList = ObjectPool.FindBySql( sql, typeof( T ) );
            if (objList == null) {
                objList = ObjectDB.FindBySql( sql, typeof( T ) );
                ObjectPool.AddSqlList( sql, objList );
            }

            return getResults<T>( (IList)objList );

        }

        //public static DataPage<T> findPageBySql<T>( String sql, int pageSize ) where T : IEntity {

        //    if (sql == null) throw new ArgumentNullException();

        //    String mysql = sql.ToLower();

        //    String[] arrItem = strUtil.Split( mysql, "where" );

        //    String queryString = arrItem[1];

        //    String[] arrSelect = strUtil.Split( arrItem[0], "from" );
        //    String selectProperty = arrSelect[0];


        //    PageCondition pc = new PageCondition();
        //    pc.ConditionStr = queryString;
        //    pc.Property = selectProperty;

        //    //pc.CurrentPage = state.Pager.getCurrent();
        //    pc.Size = pageSize;

        //    String sql = new SqlBuilder( state.EntityInfo ).GetPageSql( pc );



        //}


        /// <summary>
        /// 返回一个不经过缓存的查询工具，用于直接从数据库检索数据
        /// </summary>
        public static NoCacheDbFinder nocache {
            get { return new NoCacheDbFinder(); }
        }

        //-------------------------------------------------------------------------

        /// <summary>
        /// 将对象插入数据库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>返回一个结果对象 Result。如果发生错误，则 Result 中包含错误信息；如果没有错误，result.Info即是obj</returns>
        public static Result insert( Object obj ) {

            if (obj == null) throw new ArgumentNullException();

            Result result = ObjectDB.Insert( (IEntity)obj );
            return result;
        }

        /// <summary>
        /// 更新对象，并存入数据库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>返回一个结果对象 Result。如果发生错误，则 Result 中包含错误信息</returns>
        public static Result update( Object obj ) {

            if (obj == null) throw new ArgumentNullException();

            Result result = ObjectDB.Update( (IEntity)obj );
            ObjectPool.Update( (IEntity)obj );
            return result;
        }

        /// <summary>
        /// 只修改对象的某个特定属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName">需要修改的属性名称</param>
        public static void update( Object obj, String propertyName ) {

            if (obj == null) throw new ArgumentNullException();

            ObjectDB.Update( (IEntity)obj, propertyName );
            ObjectPool.Update( (IEntity)obj );
        }

        /// <summary>
        /// 只修改对象的特定属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="arrPropertyName">需要修改的属性的数组</param>
        public static void update( Object obj, String[] arrPropertyName ) {

            if (obj == null) throw new ArgumentNullException();

            ObjectDB.Update( (IEntity)obj, arrPropertyName );
            ObjectPool.Update( (IEntity)obj );
        }

        /// <summary>
        /// 根据条件批量更新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">更新的操作</param>
        /// <param name="condition">更新的条件</param>
        public static void updateBatch<T>( String action, String condition ) where T : IEntity {
            IEntity obj = Entity.New( typeof( T ).FullName );
            ObjectDB.UpdateBatch( obj, action, condition );
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>返回受影响的行数</returns>
        public static int delete( Object obj ) {

            if (obj == null) throw new ArgumentNullException();

            int num = ObjectDB.Delete( (IEntity)obj );
            ObjectPool.Delete( (IEntity)obj );
            return num;
        }

        /// <summary>
        /// 根据 id 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">对象的 id</param>
        /// <returns>返回受影响的行数</returns>
        public static int delete<T>( int id ) where T : IEntity {
            int num = ObjectDB.Delete( typeof( T ), id );
            ObjectPool.Delete( typeof( T ), id );
            return num;
        }

        /// <summary>
        /// 根据条件批量删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">删除条件</param>
        /// <returns>返回受影响的行数</returns>
        public static int deleteBatch<T>( String condition ) where T : IEntity {
            return ObjectDB.DeleteBatch( typeof( T ), condition );
        }

        //-------------------------------------------------------------------------

        /// <summary>
        /// 统计对象的所有数目
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>对象数量</returns>
        public static int count<T>() where T : IEntity {

            int countResult = ObjectPool.FindCount( typeof( T ) );
            if (countResult == -1) {
                countResult = ObjectDB.Count( typeof( T ) );
                ObjectPool.AddCount( typeof( T ), countResult );
            }

            return countResult;
        }

        /// <summary>
        /// 根据条件统计对象的所有数目
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">统计条件</param>
        /// <returns>对象数量</returns>
        public static int count<T>( String condition ) where T : IEntity {

            int countResult = ObjectPool.FindCount( typeof( T ), condition );
            if (countResult == -1) {
                countResult = ObjectDB.Count( typeof( T ), condition );
                ObjectPool.AddCount( typeof( T ), condition, countResult );
            }

            return countResult;
        }

        //-------------------------------------------------------------------------

        internal static List<T> getResults<T>( IList list ) {
            return cvt.ToList<T>( list );
        }

        //-------------------------------------------------------------------------

        /// <summary>
        /// 根据 sql 语句查询，返回一个 IDataReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>返回一个 IDataReader</returns>
        public static IDataReader RunReader<T>( String sql ) {
            return DataFactory.GetCommand( sql, DbContext.getConnection( typeof( T ) ) ).ExecuteReader( CommandBehavior.CloseConnection );
        }

        /// <summary>
        /// 根据 sql 语句查询，返回单行单列数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>返回单行单列数据</returns>
        public static Object RunScalar<T>( String sql ) {
            return DataFactory.GetCommand( sql, DbContext.getConnection( typeof( T ) ) ).ExecuteScalar();
        }

        /// <summary>
        /// 执行 sql 语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        public static void RunSql<T>( String sql ) {
            IDbCommand cmd = DataFactory.GetCommand( sql, DbContext.getConnection( typeof( T ) ) );
            cmd.ExecuteNonQuery();
            CacheTime.updateTable( typeof( T ) );
        }

        /// <summary>
        /// 根据 sql 语句查询，返回一个 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable RunTable<T>( String sql ) {
            DataTable dataTable = new DataTable();
            DataFactory.GetAdapter( sql, DbContext.getConnection( typeof( T ) ) ).Fill( dataTable );
            return dataTable;
        }

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 获取默认的数据库连接(default)，需要自己管理Open和Close
        /// </summary>
        /// <returns></returns>
        public static IDbConnection getConnection() {
            return getConnection( DbConfig.DefaultDbName );
        }

        /// <summary>
        /// 获取数据库连接，需要自己管理Open和Close
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static IDbConnection getConnection( String dbName ) {
            ConnectionString cs = DbConfig.Instance.GetConnectionStringMap()[dbName] as ConnectionString;
            return DataFactory.GetConnection( cs.StringContent, cs.DbType );
        }

        /// <summary>
        /// 获取数据库连接，需要自己管理Open和Close
        /// </summary>
        /// <param name="dataType">实体的类型</param>
        /// <returns></returns>
        public static IDbConnection getConnection( Type dataType ) {
            EntityInfo et = Entity.GetInfo( dataType );
            return getConnection( et.Database );
        }

        /// <summary>
        /// 获取一个数据库 Command
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="cn"></param>
        /// <returns></returns>
        public static IDbCommand getCommand( String commandText ) {
            IDbConnection cn = getConnection();
            return DataFactory.GetCommand( commandText, cn );
        }

        /// <summary>
        /// 获取一个数据库 Command
        /// </summary>
        /// <returns></returns>
        public static IDbCommand getCommand( String dbName, String commandText ) {
            IDbConnection cn = getConnection( dbName );
            return DataFactory.GetCommand( commandText, cn );
        }

        /// <summary>
        /// 获取一个数据库 Command
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IDbCommand getCommand( IDbConnection cn, String commandText ) {
            return DataFactory.GetCommand( commandText, cn );
        }

        /// <summary>
        /// 获取一个数据库 Command
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IDbCommand getCommand( Type dataType, String commandText ) {
            IDbConnection cn = getConnection( dataType );
            return DataFactory.GetCommand( commandText, cn );
        }

        /// <summary>
        /// 获取默认数据库的ConnectionString
        /// </summary>
        /// <returns></returns>
        public static String getConnectionString() {
            return DbConfig.GetConnectionString();
        }

        /// <summary>
        /// 获取数据库的ConnectionString
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static String getConnectionString( String db ) {
            return DbConfig.GetConnectionString( db );
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static String getDatabaseType( String db ) {
            return DbConfig.GetDatabaseType( db );
        }

        /// <summary>
        /// 获取默认数据库类型
        /// </summary>
        /// <returns></returns>
        public static String getDatabaseType() {
            return DbConfig.GetDatabaseType();
        }


    }
}
