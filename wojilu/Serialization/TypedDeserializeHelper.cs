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

namespace wojilu.Serialization {

    internal class TypedDeserializeHelper {

        public static T Deserialize<T>( String jsonString ) {
            return (T)DeserializeObject( jsonString, typeof( T ) );
        }

        public static List<T> DeserializeList<T>( String jsonString ) {

            List<Object> list = Json.ParseList( jsonString );

            IList retList = convertListToTypedList( list, typeof( List<T> ) );

            return (List<T>)retList;
        }

        public static Object DeserializeObject( String jsonString, Type t ) {

            JsonObject obj = Json.ParseJson( jsonString );
            if (t == typeof( JsonObject )) return obj;

            return deserializeType( t, obj );
        }

        internal static object deserializeType( Type t, JsonObject obj ) {

            if (t == typeof( JsonObject )) return obj;

            if (t.FullName.IndexOf( "__AnonymousType" ) > 0) return DeserializeAnonymous( obj, t );

            Object ret = rft.GetInstance( t );
            PropertyInfo[] properties = t.GetProperties( BindingFlags.Public | BindingFlags.Instance );
            foreach (PropertyInfo p in properties) {
                setPropertyValue( ret, p, obj );
            }
            return ret;
        }

        public static object DeserializeAnonymous( JsonObject obj, Type t ) {

            PropertyInfo[] ps = t.GetProperties();
            List<Object> values = new List<object>();
            foreach (PropertyInfo p in ps) {

                Object objValue = obj.GetValue( p.Name );
                if (objValue == null) continue;

                if (objValue is JsonObject) {
                    Object val = DeserializeAnonymous( (JsonObject)objValue, p.PropertyType );
                    values.Add( val );
                }
                else if (objValue is IList) {
                    Object val = getListValue( p, objValue as List<Object> );
                    values.Add( val );
                }
                else {
                    Object val = obj.GetValue( p.Name, p.PropertyType );
                    values.Add( val );
                }
            }

            return rft.GetInstance( t, values.ToArray() );
        }

        private static object getListValue( PropertyInfo p, List<Object> values ) {

            if (p.PropertyType.IsArray) {
                return convertListToArray( values, p.PropertyType.GetElementType() );
            }

            if (rft.IsInterface( p.PropertyType, typeof( IList ) ) && p.PropertyType.IsGenericType) {
                return convertListToTypedList( values, p.PropertyType );
            }

            return null;
        }


        private static void setPropertyValue( Object ret, PropertyInfo p, JsonObject obj ) {

            if (p.CanWrite == false) return;

            Object pValue = obj.GetValue( p.Name );
            if (pValue == null) return;

            if (p.PropertyType == typeof( int ) ||
                p.PropertyType == typeof( long ) ||
                p.PropertyType == typeof( String ) ||
                p.PropertyType == typeof( Decimal ) ||
                p.PropertyType == typeof( Double ) ||
                p.PropertyType == typeof( Boolean ) ||
                p.PropertyType == typeof( DateTime )) {

                rft.SetPropertyValue( ret, p.Name, obj.GetValue( p.Name, p.PropertyType ) );
                return;
            }

            if (pValue is JsonObject) {
                setJsonObjectValue( ret, p, pValue as JsonObject );
                return;
            }

            if (pValue is List<Object>) {
                setJsonListValue( ret, p, pValue as List<Object>, obj );
                return;
            }

        }

        private static void setJsonListValue( Object ret, PropertyInfo p, List<Object> pValue, JsonObject obj ) {

            if (p.PropertyType == typeof( List<Object> )) {
                rft.SetPropertyValue( ret, p.Name, pValue );
                return;
            }

            if (p.PropertyType.IsArray) {

                Array arr = convertListToArray( pValue, p.PropertyType.GetElementType() );
                rft.SetPropertyValue( ret, p.Name, arr );
                return;
            }

            if (rft.IsInterface( p.PropertyType, typeof( IList ) ) && p.PropertyType.IsGenericType) {

                IList list = convertListToTypedList( pValue, p.PropertyType );
                rft.SetPropertyValue( ret, p.Name, list );
                return;
            }

        }

        private static void setJsonObjectValue( Object ret, PropertyInfo p, JsonObject pValue ) {

            if (rft.IsInterface( p.PropertyType, typeof( IDictionary ) )) {

                if (p.PropertyType == typeof( JsonObject )) {
                    rft.SetPropertyValue( ret, p.Name, pValue );
                }
                else {
                    setDictionaryValues( ret, p, pValue );
                }

            }
            else {

                try {
                    Object objProperty = deserializeType( p.PropertyType, pValue );
                    rft.SetPropertyValue( ret, p.Name, objProperty );
                }
                catch (InvalidCastException ex) {
                    throw new Exception( "类型转换错误，属性名称：" + p.Name + ", 属性类型：" + p.PropertyType + Environment.NewLine + ex.Message );
                }

            }
        }

        private static void setDictionaryValues( object ret, PropertyInfo p, JsonObject pValue ) {

            Type ptype = p.PropertyType;
            if (p.PropertyType.IsGenericType == false) {

                Type baseType = p.PropertyType.BaseType;
                if (baseType.IsGenericType == false) {
                    throw new Exception( "属性的类型必须是泛型 Dictionary，属性名称：" + p.Name + ", 属性类型：" + p.PropertyType );
                }

                ptype = baseType;
            }

            Type[] typeList = ptype.GetGenericArguments();

            if (typeList[0] != typeof( String )) {
                throw new Exception( "属性 Dictionary 的 key 必须是 string 类型，属性名称：" + p.Name + ", 属性类型：" + p.PropertyType );
            }

            Type targetType = typeList[1];
            if (targetType == typeof( Object )) {
                rft.SetPropertyValue( ret, p.Name, pValue );
                return;
            }

            IDictionary objDic = (IDictionary)rft.GetInstance( p.PropertyType );
            foreach (KeyValuePair<String, Object> kv in pValue) {
                if (kv.Value == null) continue;
                Object item = convertJsonValueSingle( kv.Value, targetType );
                objDic.Add( kv.Key, item );
            }

            rft.SetPropertyValue( ret, p.Name, objDic );
        }

        private static Array convertListToArray( List<Object> list, Type targeType ) {

            ArrayList ret = new ArrayList();
            foreach (Object obj in list) {
                if (obj == null) continue;
                Object item = convertJsonValueSingle( obj, targeType );
                ret.Add( item );
            }

            return ret.ToArray( targeType );
        }

        private static IList convertListToTypedList( List<Object> list, Type listType ) {

            IList ret = (IList)rft.GetInstance( listType );

            Type targeType = listType.GetGenericArguments()[0];

            foreach (Object obj in list) {

                if (obj == null) continue;

                Object item = convertJsonValueSingle( obj, targeType );

                ret.Add( item );
            }

            return ret;
        }

        private static Object convertJsonValueSingle( Object obj, Type targeType ) {

            if (targeType == typeof( int )) return cvt.ToInt( obj );
            if (targeType == typeof( String )) return obj.ToString();
            if (targeType == typeof( Boolean )) return cvt.ToBool( obj );
            if (targeType == typeof( Decimal )) return cvt.ToDecimal( obj.ToString() );

            if (targeType == typeof( DateTime )) {
                return (Object)cvt.ToTime( obj, DateTime.MinValue );
            }

            if (targeType == typeof( long )) {
                long x;
                long.TryParse( obj.ToString(), out x );
                return x;
            }

            if (targeType == typeof( double )) {
                return cvt.ToDouble( obj.ToString() );
            }

            if (obj is JsonObject) {
                return deserializeType( targeType, (JsonObject)obj );
            }

            if (targeType == typeof( Object )) {
                return obj;
            }

            return null;
        }

    }
}
