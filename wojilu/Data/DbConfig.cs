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
using System.IO;

using wojilu.IO;
using wojilu.ORM;
using wojilu.Reflection;
using wojilu.Serialization;
using wojilu.Web;

namespace wojilu.Data {

    /// <summary>
    /// 数据库连接字符串内容的封装
    /// </summary>
    public class ConnectionString {
        public String Name { get; set; }
        public String StringContent { get; set; }
        public DatabaseType DbType { get; set; }
    }

    /// <summary>
    /// ORM 的数据库配置
    /// </summary>
    public class DbConfig {

        private static readonly ILog logger = LogManager.GetLogger( typeof( DbConfig ) );

        private static DbConfig _instance = loadConfig( getConfigPath() );

        public DbConfig() {
            this.ConnectionStringTable = new Dictionary<String, String>();
            this.AssemblyList = new List<String>();
            this.DbType = new Dictionary<String, String>();
            this.IdType = wojilu.Data.IdType.Auto;
            this.Interceptor = new List<JsonObject>();
            this.IsCheckDatabase = true;
            this.ContextCache = true;
            this.Mapping = new List<MappingInfo>();       
        }

        /// <summary>
        /// 默认数据库名称(值为default)
        /// </summary>
        public static readonly String DefaultDbName = "default";

        /// <summary>
        /// 配置的缓存内容(单例模式缓存)
        /// </summary>
        public static DbConfig Instance {
            get { return _instance; }
        }

        /// <summary>
        /// 直接解析json的结果：多个数据库连接字符串(connectionString)的键值对
        /// </summary>
        public Dictionary<String, String> ConnectionStringTable { get; set; }

        private Dictionary<String, ConnectionString> _connectionStringMap = new Dictionary<String, ConnectionString>();

        /// <summary>
        /// 多个数据库连接字符串对象的map，值是ConnectionString对象(包括Name/StringContent/DbType)
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, ConnectionString> GetConnectionStringMap() {
            return _connectionStringMap;
        }

        internal void SetConnectionStringMap( Dictionary<String, ConnectionString> cmap ) {
            _connectionStringMap = cmap;
        }

        /// <summary>
        /// 直接解析json的结果：数据库类型
        /// </summary>
        public Dictionary<String,String> DbType { get; set; }

        /// <summary>
        /// 实体键值类型
        /// </summary>
        public String IdType { get; set; }

        /// <summary>
        /// 自动键值
        /// </summary>
        [NotSerialize]
        public bool IsAutoId
        {
            get { return IdType.Equals(wojilu.Data.IdType.Auto, StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// 直接解析json的结果：程序集列表
        /// </summary>
        public List<String> AssemblyList { get; set; }

        /// <summary>
        /// 是否坚持数据库，如果检查，则会将尚未创建的数据表自动创建
        /// </summary>
        public Boolean IsCheckDatabase { get; set; }

        /// <summary>
        /// 数据表的前缀(默认没有前缀)
        /// </summary>
        public String TablePrefix { get; set; }

        /// <summary>
        /// 是否开启一级缓存，默认开启，并且建议开启
        /// </summary>
        public Boolean ContextCache { get; set; }

        /// <summary>
        /// 是否开启二级缓存
        /// </summary>
        public Boolean ApplicationCache { get; set; }

        /// <summary>
        /// 二级缓存的时间(分钟)
        /// </summary>
        public int ApplicationCacheMinutes { get; set; }

        /// <summary>
        /// 二级缓存管理程序，请填写类型(type)的全名(full name)，比如 wojilu.somens.myCache；
        /// 如果不填写，则使用默认的System.Web.Caching
        /// </summary>
        public String ApplicationCacheManager { get; set; }

        /// <summary>
        /// ORM 的元数据文件名称，一般不需填写(建议不要填写)。如果为了提高网站启动时候的速度，可以填写。
        /// 系统会根据文件名自动生成元数据文件，可以避免以后网站启动过程中的反射，能略微提高启动速度；
        /// 文件名不包括路径(必须放在bin目录中)，比如 wojilu.meta.dll
        /// </summary>
        public String MetaDLL { get; set; }

        /// <summary>
        /// 直接解析json的结果：数据表映射
        /// </summary>
        public List<MappingInfo> Mapping { get; set; }

        /// <summary>
        /// 拦截器列表
        /// </summary>
        public List<JsonObject> Interceptor { get; set; }

        /// <summary>
        /// 反射优化模式，目前只实现了 CodeDom 方式
        /// </summary>
        [NotSerialize]
        internal OptimizeMode OptimizeMode {
            get { return OptimizeMode.CodeDom; }
            set { }
        }

        /// <summary>
        /// 获取元数据库文件的绝对路径
        /// </summary>
        /// <returns></returns>
        public String GetMetaDllAbsPath() {

            if (strUtil.IsNullOrEmpty( this.MetaDLL )) return "";

            String dllPath = this.MetaDLL;
            if (dllPath.ToLower().EndsWith( ".dll" ) == false)
                dllPath = dllPath + ".dll";

            dllPath = Path.Combine( PathTool.GetBinDirectory(), dllPath );
            

            return dllPath;
        }

        private Dictionary<String, MappingInfo> _mappings = new Dictionary<String, MappingInfo>();

        private void addMapping( MappingInfo mi ) {
            _mappings.Add( mi.name, mi );
        }

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, MappingInfo> GetMappingInfo() {
            return _mappings;
        }

        //----------------------------------------------------------------------

        public static DbConfig loadConfig( String cfgPath ) {

            String str = file.Read( cfgPath );
            return loadConfigByString( str );
        }

        public static DbConfig loadConfigByString( String str ) {

            DbConfig dbc = Json.Deserialize<DbConfig>( str );

            if (dbc.AssemblyList.Count == 0) {
                logger.Warn( "AssemblyList.Count == 0" );
            }

            loadMappingInfo( dbc );
            checkConnectionString( dbc );

            return dbc;
        }

        private static void loadMappingInfo( DbConfig dbc ) {
            if (dbc.Mapping.Count == 0) return;
            foreach (MappingInfo x in dbc.Mapping) {
                dbc.addMapping( x );
            }
        }

        private static String getConfigPath() {
            return PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "orm.config" ) );
        }

        private static void checkConnectionString( DbConfig result ) {

            logger.Info( "checkConnectionString..." );

            if (result.ConnectionStringTable == null) return;

            Dictionary<String, ConnectionString> connStringMap = new Dictionary<String, ConnectionString>();

            Dictionary<String, String> newString = new Dictionary<String, String>();
            foreach (KeyValuePair<String, String> kv in result.ConnectionStringTable) {

                String connectionString = kv.Value;
                DatabaseType dbtype = getDbType( kv.Key, connectionString, result );

                ConnectionString objConnString = new ConnectionString {
                    Name = kv.Key,
                    StringContent = connectionString,
                    DbType = dbtype
                };

                connStringMap.Add( kv.Key, objConnString );

                logger.Info( "connectionString:" + connectionString );

                IDatabaseDialect dialect = DataFactory.GetDialect( dbtype );

                if ((dbtype == DatabaseType.Access)) {
                    String connectionItem = dialect.GetConnectionItem( connectionString, ConnectionItemType.Database );
                    logger.Info( "database path original:" + connectionItem );
                    if (connectionItem == null) throw new Exception( "没有设置access地址：" + connectionString );

                    if (IsRelativePath( connectionItem )) {
                        connectionItem = PathHelper.Map( strUtil.Join( SystemInfo.ApplicationPath, connectionItem ) );
                        logger.Info( "database path now:" + connectionItem );
                        String newConnString = String.Format( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", connectionItem );

                        newString.Add( kv.Key, newConnString );

                    }
                }
            }

            foreach (KeyValuePair<String, String> kv in newString) {
                result.ConnectionStringTable[kv.Key] = kv.Value;
                connStringMap[kv.Key].StringContent = kv.Value;
            }

            result.SetConnectionStringMap( connStringMap );
        }

        private static bool IsRelativePath( String connectionItem ) {
            return connectionItem.IndexOf( ":" ) < 0;
        }

        private static DatabaseType getDbType( String dbname, String connectionString, DbConfig result ) {

            foreach (KeyValuePair<String, String> kv in result.DbType) {
                if (kv.Key == dbname) return DbTypeChecker.GetFromString( kv.Value );
            }

            DatabaseType dbtype = DbTypeChecker.GetDatabaseType( connectionString );
            return dbtype;
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// 根据命名，获取数据库连接字符串
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static String GetConnectionString( String db ) {
            if (DbConfig.Instance.ConnectionStringTable.ContainsKey( db ) == false)
                throw new Exception( lang.get( "dbNotExist" ) + ": " + db );
            return (String)DbConfig.Instance.ConnectionStringTable[db];
        }

        /// <summary>
        /// 获取默认数据库的ConnectionString
        /// </summary>
        /// <returns></returns>
        public static String GetConnectionString() {
            return GetConnectionString( DefaultDbName );
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static String GetDatabaseType( String db ) {
            String ret;
            DbConfig.Instance.DbType.TryGetValue( db, out ret );
            return ret;
        }

        /// <summary>
        /// 获取默认数据库的类型
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static String GetDatabaseType() {
            return GetDatabaseType( DefaultDbName );
        }

        internal static void SaveConnectionString( String connectionString ) {

            String cfgPath = getConfigPath();

            if (DbConfig.Instance.ConnectionStringTable == null)
                DbConfig.Instance.ConnectionStringTable = new Dictionary<String, String>();

            DbConfig.Instance.ConnectionStringTable[DefaultDbName] = connectionString;

            String str = JsonString.ConvertObject( DbConfig.Instance, true );

            file.Write( cfgPath, str );
        }


        public static void Reset() {
            _instance = loadConfig( getConfigPath() );
        }

    }

}
