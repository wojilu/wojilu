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
using System.IO;
using System.Configuration;
using System.Reflection;

namespace wojilu {

    /// <summary>
    /// 读取、修改配置文件的帮助类。
    /// </summary>
    /// <remarks>
    /// 每行包括键和值，中间的分隔符默认是英文冒号。
    /// 如果某行以双斜杠 // 或井号 # 开头，就表示此行内容是注释
    /// </remarks>
    public class cfgHelper {

        // 此处不能使用日志，因为日志配置部分引用了此工具，属于循环引用
        //private static readonly ILog logger = LogManager.GetLogger( typeof( cfgHelper ) );

        /// <summary>
        /// 框架的根目录，默认是在 /framework/ 目录下。
        /// 你也可以在 web.config 的 appSettings 中，添加 framework 项，来自定义框架的根目录
        /// </summary>
        public static String FrameworkRoot = getFrameworkRoot();

        private static String getFrameworkRoot() {
            String root = GetAppSettings( "framework" );
            if (strUtil.IsNullOrEmpty( root )) return "/framework/";
            return root.EndsWith( "/" ) ? root : root + "/";
        }

        /// <summary>
        /// 框架配置文件的根目录，在 /framework/config/ 目录下
        /// </summary>
        public static String ConfigRoot {
            get { return strUtil.Join( FrameworkRoot, "config" ); }
        }

        /// <summary>
        /// 获取 web.config 的 AppSettings 中某项的值
        /// </summary>
        /// <param name="key">项的名称</param>
        /// <returns>返回一个字符串值</returns>
        public static String GetAppSettings( String key ) {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 读取配置文件，将结果放到字典Dictionary中返回
        /// </summary>
        /// <param name="path">请使用绝对路径</param>
        /// <returns>返回一个字符串字典</returns>
        public static Dictionary<String, String> Read( String absPath ) {
            return Read( absPath, defaultSeparator );
        }

        /// <summary>
        /// 读取配置文件，返回一个对象。配置文件的路径是 /framework/config/{typeFullName}.config
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <returns>返回 T 类型的对象</returns>
        public static T Read<T>() {
            return ReadByFile<T>( typeof( T ).Name + ".config" );
        }

        /// <summary>
        /// 读取配置文件，返回一个对象。
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="fileName">纯文件名称，不包括路径(默认是在 /framework/config/ 目录下)</param>
        /// <returns>返回 T 类型的对象</returns>
        private static T ReadByFile<T>( String fileName ) {
            return ReadByFile<T>( fileName, defaultSeparator );
        }

        /// <summary>
        /// 读取配置文件，返回一个对象。
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="fileName">纯文件名称，不包括路径(默认是在 /framework/config/ 目录下)</param>
        /// <param name="separator">键和值之间的分隔符</param>
        /// <returns>返回 T 类型的对象</returns>
        private static T ReadByFile<T>( String fileName, char separator ) {
            String path = PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, fileName ) );
            return Read<T>( path, separator );
        }

        /// <summary>
        /// 读取配置文件，返回一个对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">配置文件的路径(相对路径，相对于项目的根目录)</param>
        /// <returns>返回 T 类型的对象</returns>
        public static T Read<T>( String path ) {
            return Read<T>( path, defaultSeparator );
        }

        /// <summary>
        /// 读取配置文件，然后将结果通过反射，赋值给 T 类型的对象并返回。
        /// (对象的属性只支持 int/string/bool/decimal/DateTime 五种类型)
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="path">配置文件的路径(相对路径，相对于项目的根目录)</param>
        /// <param name="separator">键和值之间的分隔符</param>
        /// <returns>返回一个对象</returns>
        public static T Read<T>( String path, char separator ) {

            Dictionary<String, String> dic = Read( path, separator );

            Object result = rft.GetInstance( typeof( T ) );

            PropertyInfo[] arrp = typeof( T ).GetProperties();
            foreach (PropertyInfo p in arrp) {

                if (p.CanWrite == false) continue;

                String valString;
                dic.TryGetValue( p.Name, out valString );
                if (valString == null) continue;

                Object val = null;
                if (p.PropertyType == typeof( int ))
                    val = cvt.ToInt( valString );
                else if (p.PropertyType == typeof( Boolean ))
                    val = cvt.ToBool( valString );
                else if (p.PropertyType == typeof( decimal ))
                    val = cvt.ToDecimal( valString );
                else if (p.PropertyType == typeof( DateTime ))
                    val = cvt.ToTime( valString );
                else if (p.PropertyType == typeof( String ))
                    val = valString;

                p.SetValue( result, val, null );

            }

            return (T)result;
        }

        /// <summary>
        /// 读取配置文件，返回一个 Dictionary，键值都是字符串
        /// </summary>
        /// <param name="path">配置文件的路径(相对路径，相对于项目的根目录)</param>
        /// <param name="separator">键和值之间的分隔符</param>
        /// <returns>返回一个 Dictionary</returns>
        public static Dictionary<String, String> Read( String absPath, char separator ) {

            if (strUtil.IsNullOrEmpty( absPath ))
                throw new IOException( "config path is empty" );

            cfgHelper cfg = new cfgHelper();
            try {
                cfg.Content = file.Read( absPath );
            }
            catch (IOException) {
                cfg.Content = "";
            }

            return cfg.toDic( separator );
        }

        /// <summary>
        /// 将 Dictionary 对象持久化到磁盘
        /// </summary>
        /// <param name="dic">一个 Dictionary</param>
        /// <param name="path">配置文件的路径(相对路径，相对于项目的根目录)</param>
        public static void Write( Dictionary<String, String> dic, String path ) {
            Write( dic, path, defaultSeparator );
        }

        /// <summary>
        /// 将 Dictionary 对象持久化到磁盘
        /// </summary>
        /// <param name="dic">一个 Dictionary</param>
        /// <param name="path">配置文件的路径(相对路径，相对于项目的根目录)</param>
        /// <param name="separator">键和值之间的分隔符</param>
        public static void Write( Dictionary<String, String> dic, String path, char separator ) {
            if (strUtil.IsNullOrEmpty( path ))
                throw new IOException( "config path is empty" );

            cfgHelper cfg = new cfgHelper();
            cfg.Dic = dic;
            cfg.setSeparator( separator );

            file.Write( PathHelper.Map( path ), cfg.Content );
        }

        /// <summary>
        /// 将对象持久化到磁盘。保存的路径是 /framework/config/{typeFullName}.config
        /// </summary>
        /// <param name="obj">某特定对象</param>
        public static void Write( Object obj ) {
            WriteToFile( obj, obj.GetType().Name + ".config" );
        }

        /// <summary>
        /// 将对象持久化到磁盘。
        /// </summary>
        /// <param name="obj">某特定对象</param>
        /// <param name="fileName">纯文件名称，不包括路径(默认是在 /framework/config/ 下)</param>
        public static void WriteToFile( Object obj, String fileName ) {
            WriteToFile( obj, fileName, defaultSeparator );
        }

        /// <summary>
        /// 将对象持久化到磁盘
        /// </summary>
        /// <param name="obj">某特定对象</param>
        /// <param name="fileName">纯文件名称，不包括路径(默认是在 /framework/config/ 下)</param>
        /// <param name="separator">键和值之间的分隔符</param>
        public static void WriteToFile( Object obj, String fileName, char separator ) {

            Dictionary<String, String> dic = new Dictionary<String, String>();
            PropertyInfo[] arrp = obj.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );
            foreach (PropertyInfo p in arrp) {
                if (p.CanRead == false) continue;
                Object val = p.GetValue( obj, null );
                dic[p.Name] = (val == null ? null : val.ToString());
            }

            String path = strUtil.Join( cfgHelper.ConfigRoot, fileName );

            Write( dic, path, separator );                
        }

        /// <summary>
        /// 将 Dictionary 序列化为字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static String GetDicString( Dictionary<String, String> dic ) {
            cfgHelper cfg = new cfgHelper();
            cfg.Dic = dic;
            return cfg.ToString();
        }


        //--------------------------------------------------------------

        private String _content;

        /// <summary>
        /// 配置文件的内容
        /// </summary>
        public String Content {
            get {
                if (_dictionary != null && _content == null)
                    toString();
                return _content;
            }
            set { _content = value; }
        }

        private char _separator = defaultSeparator;

        private char getSeparator() {
            return _separator;
        }

        /// <summary>
        /// 设置键和值之间的分隔符
        /// </summary>
        /// <param name="separator"></param>
        public void setSeparator( char separator ) {
            _separator = separator;
        }

        private Dictionary<String, String> _dictionary;

        /// <summary>
        /// 以 Dictionary 的形式设置或获取配置
        /// </summary>
        public Dictionary<String, String> Dic {
            get {
                if (_dictionary == null && strUtil.HasText( Content ))
                    toDic( getSeparator() );
                return _dictionary;
            }
            set { _dictionary = value; }
        }

        private Dictionary<String, String> toDic( char separator ) {

            Dictionary<String, String> result = new Dictionary<String, String>();

            String[] arrLine = Content.Split( new char[] { '\n', '\r' } );

            foreach (String oneLine in arrLine) {

                //无值的行跳过
                if (strUtil.IsNullOrEmpty( oneLine )) continue;

                //注释行跳过
                if (oneLine.StartsWith( "//" ) || oneLine.StartsWith( "#" )) continue; 

                String[] arrPair = oneLine.Split( new char[] { separator }, 2 );
                if (arrPair.Length < 2) continue;

                char[] arrTrim = new char[] { '"', '\'' };

                String itemKey = arrPair[0].Trim().TrimStart( arrTrim ).TrimEnd( arrTrim ).Trim();
                String itemValue = arrPair[1].Trim().TrimStart( arrTrim ).TrimEnd( arrTrim ).Trim();

                result[itemKey] = itemValue;

            }

            _dictionary = result;

            return result;
        }

        private void toString() {
            StringBuilder sb = new StringBuilder();

            char s = getSeparator();
            foreach (KeyValuePair<String, String> pair in this.Dic) {
                sb.Append( pair.Key );
                sb.Append( " " );
                sb.Append( s );
                sb.Append( " " );
                sb.Append( pair.Value );
                sb.Append( Environment.NewLine );
            }

            _content = sb.ToString();
        }

        /// <summary>
        /// 配置文件的内容
        /// </summary>
        /// <returns></returns>
        public override String ToString() {
            return this.Content;
        }

        private static readonly char  defaultSeparator = ':';

    }
}
