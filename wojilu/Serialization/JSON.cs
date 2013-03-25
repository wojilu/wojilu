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
using System.Reflection;
using System.Text;

using wojilu.ORM;
using wojilu.Reflection;

namespace wojilu.Serialization {

    /// <summary>
    /// 封装了 json 反序列化中的常见操作：将 json 字符串反序列化为对象、对象列表、字典等。
    /// 序列化工具见 JsonString
    /// </summary>
    public class JSON {

        /// <summary>
        /// 将字典序列化为 json 字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static String DicToString( Dictionary<String, object> dic ) {
            return JsonString.ConvertDictionary( dic, false );
        }

        //----------------------------------------------------- String to obj ---------------------------------------------------------------------

        /// <summary>
        /// 将 json 字符串反序列化为对象
        /// </summary>
        /// <param name="oneJsonString">json 字符串</param>
        /// <param name="t">目标类型</param>
        /// <returns></returns>
        public static Object ToObject( String oneJsonString, Type t ) {

            JsonObject map = JsonParser.Parse( oneJsonString ) as JsonObject;
            return setValueToObject( t, map );
        }

        /// <summary>
        /// 将 json 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">json 字符串</param>
        /// <returns></returns>
        public static T ToObject<T>( String jsonString ) {
            Object result = ToObject( jsonString, typeof( T ) );
            return (T)result;
        }

        /// <summary>
        /// 将 json 字符串反序列化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">json 字符串</param>
        /// <returns>返回对象列表</returns>
        public static List<T> ToList<T>( String jsonString ) {

            List<T> list = new List<T>();
            if (strUtil.IsNullOrEmpty( jsonString )) return list;

            List<object> lists = JsonParser.Parse( jsonString ) as List<object>;

            foreach (JsonObject map in lists) {
                Object result = setValueToObject( typeof( T ), map );
                list.Add( (T)result );
            }

            return list;
        }

        internal static Object setValueToObject( Type t, JsonObject map ) {
            Object result = rft.GetInstance( t );

            PropertyInfo[] properties = t.GetProperties( BindingFlags.Public | BindingFlags.Instance );


            foreach (KeyValuePair<String, object> pair in map) {

                String pName = pair.Key;
                String strValue = pair.Value.ToString();

                PropertyInfo p = getPropertyInfo( properties, pName );
                if (p == null) continue;
                if (p.IsDefined( typeof( NotSaveAttribute ), false )) continue;

                Object objValue;

                if (ReflectionUtil.IsBaseType( p.PropertyType )) {
                    objValue = Convert.ChangeType( strValue, p.PropertyType );
                }
                else if (p.PropertyType == typeof( Dictionary<String, Object> )) {
                    objValue = pair.Value;
                }
                else if (rft.IsInterface( p.PropertyType, typeof( IList ) )) {
                    objValue = pair.Value;
                }
                else {
                    objValue = rft.GetInstance( p.PropertyType );
                    ReflectionUtil.SetPropertyValue( objValue, "Id", cvt.ToInt( strValue ) );
                }
                ReflectionUtil.SetPropertyValue( result, pName, objValue );
            }
            return result;
        }

        public static IEntity ToEntity( String jsonString, Type t ) {

            JsonObject map = JsonParser.Parse( jsonString ) as JsonObject;

            return toEntityByMap( t, map, null );
        }

        private static IEntity toEntityByMap( Type t, JsonObject map, Type parentType ) {
            if (map == null) return null;
            IEntity result = Entity.New( t.FullName );
            EntityInfo ei = Entity.GetInfo( t );



            foreach (KeyValuePair<String, object> pair in map) {

                String pName = pair.Key;
                object pValue = pair.Value;


                EntityPropertyInfo p = ei.GetProperty( pName );

                Object objValue = null;

                if (ReflectionUtil.IsBaseType( p.Type )) {
                    objValue = Convert.ChangeType( pValue, p.Type );
                }
                else if (p.IsList) {
                    continue;
                }

                else if (pValue is JsonObject) {
                    if (p.Type == parentType) continue;

                    JsonObject dic = pValue as JsonObject;
                    if (dic != null && dic.Count > 0) {
                        objValue = toEntityByMap( p.Type, dic, t );
                    }
                }
                else
                    continue;

                p.SetValue( result, objValue );

            }
            return result;
        }

        //--------------------------------------------------------------------------------------------------------------------------

        public static List<T> ConvertList<T>( List<Object> list ) {

            if (list == null) return null;

            List<T> results = new List<T>();
            foreach (Object obj in list) {

                if (obj == null) continue;

                T item;

                if (typeof( T ) == typeof( DateTime )) {
                    item = (T)((Object)cvt.ToTime( obj, DateTime.MinValue ));
                }
                else if (typeof( T ) == typeof( long )) {
                    long x;
                    long.TryParse( obj.ToString(), out x );
                    item = (T)((Object)x);
                }
                else if (typeof( T ) == typeof( double )) {
                    item = (T)((Object)cvt.ToDouble( obj.ToString() ));
                }
                else {
                    item = (T)obj;
                }

                results.Add( item );
            }

            return results;
        }

        /// <summary>
        /// 将 json 字符串反序列化为字典对象的列表
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<JsonObject> ToDictionaryList( String jsonString ) {


            List<object> list = JsonParser.Parse( jsonString ) as List<object>;

            List<JsonObject> results = new List<JsonObject>();
            foreach (Object obj in list) {

                JsonObject item = obj as JsonObject;
                results.Add( item );
            }

            return results;
        }

        /// <summary>
        /// 将 json 字符串反序列化为字典对象
        /// </summary>
        /// <param name="oneJsonString"></param>
        /// <returns></returns>
        public static JsonObject ToDictionary( String oneJsonString ) {
            String str = trimBeginEnd( oneJsonString, "[", "]" );

            if (strUtil.IsNullOrEmpty( str )) return new JsonObject();

            return JsonParser.Parse( str ) as JsonObject;
        }


        private static String trimBeginEnd( String str, String beginStr, String endStr ) {
            str = str.Trim();
            str = strUtil.TrimStart( str, beginStr );
            str = strUtil.TrimEnd( str, endStr );
            str = str.Trim();
            return str;
        }

        private static PropertyInfo getPropertyInfo( PropertyInfo[] propertyList, String pName ) {
            foreach (PropertyInfo info in propertyList) {
                if (info.Name.Equals( pName )) {
                    return info;
                }
            }
            return null;
        }

        /// <summary>
        /// 将引号、冒号、逗号进行编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Encode( String str ) {
            return str.Replace( "\"", "&quot;" ).Replace( ":", "&#58;" ).Replace( ",", "&#44;" ).Replace( "'", "\\'" );
        }

        /// <summary>
        /// 将引号、冒号、逗号进行解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Decode( String str ) {
            return str.Replace( "&quot;", "\"" ).Replace( "&#58;", ":" ).Replace( "&#44;", "," ).Replace( "\\'", "'" );
        }

    }
}

