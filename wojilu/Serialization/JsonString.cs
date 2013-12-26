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
using System.Reflection;
using wojilu.Reflection;
using wojilu.ORM;

namespace wojilu.Serialization {

    /// <summary>
    /// json 序列化工具：将对象转换成 json 字符串
    /// </summary>
    public class JsonString {

        private static Boolean getDefaultIsBreakline() {
            return false;
        }

        private static String empty() {
            return "\"\"";
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String Convert( Object obj ) {
            return Convert( obj, getDefaultIsBreakline() );
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <returns></returns>
        public static String Convert( Object obj, Boolean isBreakline ) {
            return Convert( obj, isBreakline, false );
        }

        public static String Convert( Object obj, Boolean isBreakline, Boolean allowNotSave ) {

            if (obj == null) return empty();

            Type t = obj.GetType();
            if (t.IsArray) return ConvertArray( obj, allowNotSave );
            if (rft.IsInterface( t, typeof( IList ) )) return ConvertList( (IList)obj, allowNotSave );
            if (rft.IsInterface( t, typeof( IDictionary ) )) return ConvertDictionary( (IDictionary)obj, isBreakline, allowNotSave );

            if (t == typeof( int ) ||
                t == typeof( long ) ||
                t == typeof( decimal ) ||
                t == typeof( double )) {
                return obj.ToString();
            }

            if (t == typeof( Boolean )) return obj.ToString().ToLower();
            if (t == typeof( DateTime ) || t == typeof( long )) return "\"" + obj.ToString() + "\"";
            if (t == typeof( String )) {
                // 转义双引号，消除换行
                return "\"" + ClearNewLine( obj.ToString() ) + "\"";
            }

            return ConvertObject( obj, isBreakline, true, allowNotSave );
        }

        /// <summary>
        /// 清楚json字符串中的换行符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ClearNewLine( String str ) {

            if (str == null) return null;

            return str
                    .Replace( @"\", @"\\" )
                    .Replace( "\"", "\\" + "\"" )
                    .Replace( "\r", "" )
                    .Replace( "\n", "" )
                    .Replace( "\t", "" );
        }

        public static String ConvertArray( Object obj ) {
            return ConvertArray( obj, false );
        }

        public static String ConvertArray( Object obj, Boolean allowNotSave ) {

            if (obj == null) return "[]";

            List<Object> items = new List<Object>();

            IEnumerable myList = obj as IEnumerable;
            foreach (Object element in myList) {
                items.Add( element );
            }

            return ConvertArray( items.ToArray(), allowNotSave );
        }


        /// <summary>
        /// 将对象数组转换成 json 字符串
        /// </summary>
        /// <param name="arrObj"></param>
        /// <returns></returns>
        public static String ConvertArray( object[] arrObj ) {
            return ConvertArray( arrObj, false );
        }

        public static String ConvertArray( object[] arrObj, Boolean allowNotSave ) {
            if (arrObj == null) return "[]";
            StringBuilder sb = new StringBuilder();
            sb.Append( "[ " );
            for (int i = 0; i < arrObj.Length; i++) {
                if (arrObj[i] == null) continue;
                sb.Append( Convert( arrObj[i], getDefaultIsBreakline(), allowNotSave ) );
                if (i < arrObj.Length - 1) sb.Append( ", " );
            }
            sb.Append( " ]" );
            return sb.ToString();
        }

        /// <summary>
        /// 将对象列表转换成 json 字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String ConvertList( IList list ) {
            return ConvertList( list, getDefaultIsBreakline() );
        }

        /// <summary>
        /// 将对象列表转换成 json 字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <returns></returns>
        public static String ConvertList( IList list, Boolean isBreakline ) {
            return ConvertList( list, isBreakline, false );
        }

        public static String ConvertList( IList list, Boolean isBreakline, Boolean allowNotSave ) {
            if (list == null) return "[]";
            StringBuilder sb = new StringBuilder();
            sb.Append( "[ " );
            if (isBreakline) sb.AppendLine();

            for (int i = 0; i < list.Count; i++) {
                if (list[i] == null) continue;
                sb.Append( Convert( list[i], isBreakline, allowNotSave ) );
                if (i < list.Count - 1) sb.Append( ", " );
                if (isBreakline) sb.AppendLine();

            }
            sb.Append( " ]" );
            return sb.ToString();
        }

        /// <summary>
        /// 将字典 Dictionary 转换成 json 字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static String ConvertDictionary( IDictionary dic ) {
            return ConvertDictionary( dic, getDefaultIsBreakline() );
        }

        /// <summary>
        /// 将字典 Dictionary 转换成 json 字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <returns></returns>
        public static String ConvertDictionary( IDictionary dic, Boolean isBreakline ) {
            return ConvertDictionary( dic, isBreakline, false );
        }

        public static String ConvertDictionary( IDictionary dic, Boolean isBreakline, Boolean allowNotSave ) {

            if (dic == null) return empty();

            StringBuilder builder = new StringBuilder();
            builder.Append( "{ " );
            if (isBreakline) builder.AppendLine();
            foreach (DictionaryEntry pair in dic) {
                builder.Append( "\"" );
                builder.Append( pair.Key );
                builder.Append( "\":" );
                builder.Append( Convert( pair.Value, isBreakline, allowNotSave ) );
                builder.Append( ", " );
                if (isBreakline) builder.AppendLine();

            }

            String result = builder.ToString().Trim().TrimEnd( ',' );
            if (isBreakline) result += Environment.NewLine;
            return result + " }";

        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ConvertObject( Object obj ) {
            return ConvertObject( obj, getDefaultIsBreakline() );
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <returns></returns>
        public static String ConvertObject( Object obj, Boolean isBreakline ) {
            return ConvertObject( obj, isBreakline, true );
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <param name="withQuotation">属性名是否使用引号(默认不启用)</param>
        /// <returns></returns>
        public static String ConvertObject( Object obj, Boolean isBreakline, Boolean withQuotation ) {
            return ConvertObject( obj, isBreakline, withQuotation, false );
        }

        public static String ConvertObject( Object obj, Boolean isBreakline, Boolean withQuotation, Boolean allowNotSave ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "{ " );
            if (isBreakline) builder.AppendLine();

            PropertyInfo[] properties = obj.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );

            Boolean isIdFind = false;
            Boolean isNameFind = false;
            Object idValue = "";
            Object nameValue = "";
            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            foreach (PropertyInfo info in properties) {

                if (isSkip( info )) {
                    continue;
                }

                if (info.Name.Equals( "Id" )) {
                    isIdFind = true;
                    idValue = ReflectionUtil.GetPropertyValue( obj, "Id" );
                }
                else if (info.Name.Equals( "Name" )) {
                    isNameFind = true;
                    nameValue = ReflectionUtil.GetPropertyValue( obj, "Name" );
                }
                else {
                    propertyList.Add( info );
                }
            }

            if (withQuotation) {
                if (isIdFind) {
                    builder.AppendFormat( "\"Id\":{0}, ", idValue );
                }
                if (isNameFind) {
                    builder.AppendFormat( "\"Name\":\"{0}\", ", nameValue );
                }
            }
            else {

                if (isIdFind) {
                    builder.AppendFormat( "Id:{0}, ", idValue );
                }
                if (isNameFind) {
                    builder.AppendFormat( "Name:\"{0}\", ", nameValue );
                }

            }

            foreach (PropertyInfo info in propertyList) {

                Object propertyValue = ReflectionUtil.GetPropertyValue( obj, info.Name );

                String jsonValue;
                if (info.PropertyType.IsArray) {
                    jsonValue = ConvertArray( propertyValue );
                }
                else if (rft.IsInterface( info.PropertyType, typeof( IList ) )) {
                    jsonValue = ConvertList( (IList)propertyValue, isBreakline );
                }
                else {
                    jsonValue = Convert( propertyValue, isBreakline );
                }

                if (withQuotation) {
                    builder.AppendFormat( "\"{0}\":{1}", info.Name, jsonValue );
                }
                else {
                    builder.AppendFormat( "{0}:{1}", info.Name, jsonValue );
                }

                builder.Append( ", " );
                if (isBreakline) builder.AppendLine();

            }
            String result = builder.ToString().Trim().TrimEnd( ',' );
            if (isBreakline) result += Environment.NewLine;
            return result + " }";
        }

        private static Boolean isSkip( PropertyInfo info ) {

            if (info.CanRead == false) {
                return true;
            }

            if (info.IsDefined( typeof( NotSerializeAttribute ), false )) {
                return true;
            }

            //if (info.IsDefined( typeof( NotSaveAttribute ), false )) {
            //    return true;
            //}

            return false;
        }

        public static String ConvertEntity( IEntity obj ) {


            StringBuilder builder = new StringBuilder();
            builder.Append( "{ " );

            EntityInfo ei = Entity.GetInfo( obj );
            List<EntityPropertyInfo> ps = ei.PropertyListAll;

            foreach (EntityPropertyInfo info in ps) {

                if (shouldPass( info )) continue;

                Object propertyValue = info.GetValue( obj );

                String jsonValue;

                if (propertyValue == null) {
                    jsonValue = empty();
                }
                else {

                    if (info.IsEntity) {
                        jsonValue = ConvertEntity( propertyValue as IEntity );
                    }
                    else
                        jsonValue = Convert( propertyValue, false );
                }


                builder.AppendFormat( "{0}:{1}", info.Name, jsonValue );

                builder.Append( ", " );

            }
            String result = builder.ToString().Trim().TrimEnd( ',' );
            return result + " }";
        }

        private static bool shouldPass( EntityPropertyInfo info ) {
            if (info.Type == typeof( int )) return false;
            if (info.Type == typeof( long )) return false;
            if (info.Type == typeof( string )) return false;
            if (info.Type == typeof( decimal )) return false;
            if (info.Type == typeof( DateTime )) return false;
            if (info.Type == typeof( bool )) return false;
            if (info.Type == typeof( double )) return false;
            if (info.IsEntity) return false;
            return true;
        }


    }


}
