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
using wojilu.Serialization;

namespace wojilu {

    /// <summary>
    /// 封装了 json 对象信息
    /// </summary>
    public class JsonObject : Dictionary<String, Object> {

        /// <summary>
        /// 获取(字符串类型)属性的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String Get( String key ) {
            Object result;
            this.TryGetValue( key, out result );
            return result == null ? null : result.ToString();
        }

        /// <summary>
        /// 获取属性的值，返回强类型。支持 int, string, bool, long, decimal, double, DateTime, JsonObject, List&lt;Object&gt; 一共9种类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>( String key ) {

            Object obj = this.GetValue( key );
            if (obj == null) return default( T );

            return (T)this.GetValue( key, typeof( T ) );
        }

        /// <summary>
        /// 获取属性的值，返回 Json 对象类型
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        public JsonObject GetJson( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return result == null ? null : result as JsonObject;
        }

        /// <summary>
        /// 获取属性的值，返回数据列表
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        public List<Object> GetList( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return result == null ? null : result as List<Object>;
        }

        /// <summary>
        /// 获取属性的值，返回 强类型 的数据列表(当列表内数据类型相同时使用)
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        public List<T> GetList<T>( String key ) {
            return JSON.ConvertList<T>( this.GetList( key ) );
        }

        /// <summary>
        /// 获取属性的值，可能是string,int等各种类型，需要自己转换
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        public Object GetValue( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return result;
        }

        /// <summary>
        /// 获取属性的值，返回强类型。支持 int, string, bool, long, decimal, double, DateTime, JsonObject, List&lt;Object&gt; 一共9种类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="retType"></param>
        /// <returns></returns>
        public Object GetValue( String key, Type retType ) {

            if (retType == typeof( DateTime )) return this.getTime( key );
            if (retType == typeof( long )) return this.getLong( key );
            if (retType == typeof( Double )) return this.getDouble( key );
            if (retType == typeof( Decimal )) return this.getDecimal( key );

            Object obj = this.GetValue( key );
            if (obj == null) {
                if (retType == typeof( int )) return 0;
                if (retType == typeof( Boolean )) return false;
                return null;
            }

            return obj;
        }

        /// <summary>
        /// 获取属性的值，返回 long
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private long getLong( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            if (result == null) return 0;

            long x;
            long.TryParse( result.ToString(), out x );
            return x;
        }

        /// <summary>
        /// 获取属性的值，返回时间类型
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        private DateTime getTime( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return cvt.ToTime( result, DateTime.MinValue );
        }

        /// <summary>
        /// 获取属性的值，返回 Double 类型
        /// </summary>
        /// <param name="key">属性名称</param>
        /// <returns></returns>
        private Double getDouble( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return result == null ? 0 : cvt.ToDouble( result.ToString() );
        }

        /// <summary>
        /// 获取属性的值，返回 Decimal 类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Decimal getDecimal( String key ) {

            Object result;
            this.TryGetValue( key, out result );

            return result == null ? 0 : cvt.ToDecimal( result.ToString() );
        }

    }
}
