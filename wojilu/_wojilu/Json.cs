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
using wojilu.Serialization;
using System.Collections;
using System.Reflection;

namespace wojilu {

    /// <summary>
    /// 提供 json 序列化和反序列化的功能
    /// </summary>
    public class Json {

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToString( Object obj ) {
            return JsonString.Convert( obj );
        }

        /// <summary>
        /// 将对象转换成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isBreakline">是否换行(默认不换行，阅读起来更加清晰)</param>
        /// <returns></returns>
        public static String ToString( Object obj, Boolean isBreakline ) {
            return JsonString.Convert( obj, isBreakline );
        }        

        /// <summary>
        /// 将简单对象列表转换成 json 字符串，用于存储。对象之间换行，对象内部不换行。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static String ToStringList( IList list ) {
            return SimpleJsonString.ConvertList( list );
        }

        /// <summary>
        /// 解析字符串，返回强类型的对象。如果要返回List列表，请使用 DeserializeList 方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>( String jsonString ) {
            if (strUtil.IsNullOrEmpty( jsonString )) return default( T );
            return TypedDeserializeHelper.Deserialize<T>( jsonString );
        }

        /// <summary>
        /// 将 json 字符串反序列化为对象
        /// </summary>
        /// <param name="jsonString">json 字符串</param>
        /// <param name="t">目标类型</param>
        /// <returns></returns>
        public static Object DeserializeObject( String jsonString, Type t ) {
            return TypedDeserializeHelper.DeserializeObject( jsonString, t );
        }

        /// <summary>
        /// 将 json 字符串反序列化为对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<T> DeserializeList<T>( String jsonString ) {
            return TypedDeserializeHelper.DeserializeList<T>( jsonString );
        }

        /// <summary>
        /// 将 json 字符串反序列化为匿名对象
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="objDefinition">一个object，可以定义或代表匿名对象</param>
        /// <returns></returns>
        public static Object DeserializeAnonymous( String jsonString, Object objDefinition ) {
            if (objDefinition == null) throw new ArgumentNullException( "anonymousObject" );
            return DeserializeAnonymous( jsonString, objDefinition.GetType() );
        }
        
        /// <summary>
        /// 将 json 字符串反序列化为匿名对象
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="t">匿名对象的类型</param>
        /// <returns></returns>
        public static Object DeserializeAnonymous( String jsonString, Type t ) {

            if (strUtil.IsNullOrEmpty( jsonString )) return null;
            if (t == null) throw new ArgumentNullException( "anonymous type is null" );

            JsonObject obj = ParseJson( jsonString );
            if (obj == null) return null;

            return TypedDeserializeHelper.DeserializeAnonymous( obj, t );
        }

        /// <summary>
        /// 解析字符串，返回原始的 json 类型对象。
        /// 根据 json 的不同，可能返回整数(int)、布尔类型(bool)、字符串(string)、一般Json对象(JsonObject)、数组(List&lt;object&gt;)等不同类型
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Object Parse( String jsonString ) {
            if (strUtil.IsNullOrEmpty( jsonString )) return null;
            return JsonParser.Parse( jsonString );
        }

        /// <summary>
        /// 将 json 字符串反序列化为原始的 JsonObject 类型
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static JsonObject ParseJson( String jsonString ) {
            if (strUtil.IsNullOrEmpty( jsonString )) return new JsonObject();
            return JsonParser.Parse( jsonString ) as JsonObject;
        }

        /// <summary>
        /// 将 json 字符串解析为 json 原始数据类型的列表，比如 ["abc", 88, {name:"aa", gender:"male"}]
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<Object> ParseList( String jsonString ) {
            if (strUtil.IsNullOrEmpty( jsonString )) return new List<Object>();
            return JsonParser.Parse( jsonString ) as List<Object>;
        }

        /// <summary>
        /// 将 json 字符串反序列化为 原始的json强类型(int,string,JsonObject等) 的数据列表。
        /// 当列表内数据类型相同时使用，比如 ["abc", "www", "qqqxyz", "uuu"]
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<T> ParseList<T>( String jsonString ) {
            if (strUtil.IsNullOrEmpty( jsonString )) return new List<T>();
            return DeserializeList<T>( jsonString );
        }


    }

}
