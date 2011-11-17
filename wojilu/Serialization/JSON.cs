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

            Dictionary<String, object> map = JsonParser.Parse( oneJsonString ) as Dictionary<String, object>;
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

            foreach (Dictionary<String, object> map in lists) {
                Object result = setValueToObject( typeof( T ), map );
                list.Add( (T)result );
            }

            return list;
        }

        internal static Object setValueToObject( Type t, Dictionary<String, object> map ) {
            Object result = rft.GetInstance( t );

            PropertyInfo[] properties = t.GetProperties( BindingFlags.Public | BindingFlags.Instance );


            foreach (KeyValuePair<String, object> pair in map) {

                String pName = pair.Key;
                String pValue = pair.Value.ToString();


                PropertyInfo info = getPropertyInfo( properties, pName );
                if ((info != null) && !info.IsDefined( typeof( NotSaveAttribute ), false )) {

                    Object objValue;

                    if (ReflectionUtil.IsBaseType( info.PropertyType )) {
                        objValue = Convert.ChangeType( pValue, info.PropertyType );
                    }
                    else if (info.PropertyType == typeof( Dictionary<String, object> )) {
                        objValue = pair.Value;
                    }
                    else if (rft.IsInterface( info.PropertyType, typeof( IList ) )) {
                        objValue = pair.Value;
                    }
                    else {
                        objValue = rft.GetInstance( info.PropertyType );
                        ReflectionUtil.SetPropertyValue( objValue, "Id", cvt.ToInt( pValue ) );
                    }
                    ReflectionUtil.SetPropertyValue( result, pName, objValue );
                }

            }
            return result;
        }

        public static IEntity ToEntity( String jsonString, Type t ) {

            Dictionary<String, object> map = JsonParser.Parse( jsonString ) as Dictionary<String, object>;

            return toEntityByMap( t, map, null );
        }

        private static IEntity toEntityByMap( Type t, Dictionary<String, object> map, Type parentType ) {
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

                else if (pValue is Dictionary<String, object>) {
                    if (p.Type == parentType) continue;

                    Dictionary<String, object> dic = pValue as Dictionary<String, object>;
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

        /// <summary>
        /// 将 json 字符串反序列化为字典对象的列表
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<Dictionary<String, object>> ToDictionaryList( String jsonString ) {


            List<object> list = JsonParser.Parse( jsonString ) as List<object>;

            List<Dictionary<String, object>> results = new List<Dictionary<String, object>>();
            foreach (Object obj in list) {

                Dictionary<String, object> item = obj as Dictionary<String, object>;
                results.Add( item );
            }

            return results;
        }

        /// <summary>
        /// 将 json 字符串反序列化为字典对象
        /// </summary>
        /// <param name="oneJsonString"></param>
        /// <returns></returns>
        public static Dictionary<String, object> ToDictionary( String oneJsonString ) {
            String str = trimBeginEnd( oneJsonString, "[", "]" );

            if (strUtil.IsNullOrEmpty( str )) return new Dictionary<String, object>();

            return JsonParser.Parse( str ) as Dictionary<String, object>;
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

