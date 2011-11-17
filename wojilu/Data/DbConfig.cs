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

using wojilu.ORM;
using wojilu.Reflection;
using wojilu.Serialization;
using wojilu.Web;
using System.IO;
using System.Web;
using wojilu.IO;

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

        public DbConfig() {
            this.ConnectionStringTable = new Dictionary<String, object>();
            this.AssemblyList = new List<object>();
            this.DbType = new Dictionary<String, object>();
            this.Interceptor = new List<object>();
            this.IsCheckDatabase = true;
            this.ContextCache = true;
            this.Mapping = new List<object>();
        }

        /// <summary>
        /// 默认数据库名称(值为default)
        /// </summary>
        public static readonly String DefaultDbName = "default";

        /// <summary>
        /// 配置的缓存内容(单例模式缓存)
        /// </summary>
        public static DbConfig Instance = loadConfig( getConfigPath() );

        /// <summary>
        /// 直接解析json的结果：多个数据库连接字符串(connectionString)的键值对
        /// </summary>
        public Dictionary<String, object> ConnectionStringTable { get; set; }

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
        public Dictionary<String, object> DbType { get; set; }

        /// <summary>
        /// 直接解析json的结果：程序集列表
        /// </summary>
        public List<object> AssemblyList { get; set; }

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
        public List<object> Mapping { get; set; }

        /// <summary>
        /// 拦截器列表
        /// </summary>
        public List<object> Interceptor { get; set; }

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
            _mappings.Add( mi.TypeName, mi );
        }

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <returns></returns>
        internal Dictionary<String, MappingInfo> GetMappingInfo() {
            return _mappings;
        }

        //----------------------------------------------------------------------

        private static DbConfig loadConfig( String cfgPath ) {

            String str = file.Read( cfgPath );
            DbConfig dbc = JSON.ToObject<DbConfig>( str );

            loadMappingInfo( dbc );
            checkConnectionString( dbc );

            return dbc;
        }

        private static void loadMappingInfo( DbConfig dbc ) {
            if (dbc.Mapping.Count == 0) return;
            foreach (Dictionary<String, object> dic in dbc.Mapping) {

                MappingInfo mi = new MappingInfo();

                if (dic.ContainsKey( "name" )) mi.TypeName = dic["name"].ToString();
                if (dic.ContainsKey( "database" )) mi.Database = dic["database"].ToString();
                if (dic.ContainsKey( "table" )) mi.Table = dic["table"].ToString();

                dbc.addMapping( mi );
            }
        }

        private static String getConfigPath() {
            return PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "orm.config" ) );
        }

        private static void checkConnectionString( DbConfig result ) {

            logger.Info( "checkConnectionString..." );

            if (result.ConnectionStringTable == null) return;

            Dictionary<String, ConnectionString> connStringMap = new Dictionary<String, ConnectionString>();

            Dictionary<String, String> newString = new Dictionary<string, string>();
            foreach (KeyValuePair<String, object> kv in result.ConnectionStringTable) {

                String connectionString = kv.Value.ToString();
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

        private static bool IsRelativePath( string connectionItem ) {
            return connectionItem.IndexOf( ":" ) < 0;
        }

        private static DatabaseType getDbType( String dbname, String connectionString, DbConfig result ) {

            foreach (KeyValuePair<String, Object> kv in result.DbType) {
                if (kv.Key == dbname) return DbTypeChecker.GetFromString( kv.Value.ToString() );
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


        internal static void SaveConnectionString( String connectionString ) {

            String cfgPath = getConfigPath();

            if (DbConfig.Instance.ConnectionStringTable == null)
                DbConfig.Instance.ConnectionStringTable = new Dictionary<String, object>();

            DbConfig.Instance.ConnectionStringTable[DefaultDbName] = connectionString;

            String str = JsonString.ConvertObject( DbConfig.Instance, true );

            file.Write( cfgPath, str );
        }

    }

}
