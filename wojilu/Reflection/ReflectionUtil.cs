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
using System.Reflection;
using wojilu;
using System.Collections.Generic;

namespace wojilu.Reflection {

    /// <summary>
    /// 封装了反射的常用操作方法
    /// </summary>
    public class ReflectionUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ReflectionUtil ) );


        /// <summary>
        /// 通过反射创建对象(Activator.CreateInstance)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object GetInstance( Type t ) {
            return Activator.CreateInstance( t );
        }

        /// <summary>
        /// 通过反射创建对象(Activator.CreateInstance)，并提供构造函数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Object GetInstance( Type t, params object[] args ) {
            return Activator.CreateInstance( t, args );
        }

        /// <summary>
        /// 创建对象(通过加载指定程序集中的类型)
        /// </summary>
        /// <param name="asmName">不需要后缀名</param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Object GetInstance( String asmName, String typeName ) {
            // Load不需要ext，LoadFrom需要
            Assembly asm = Assembly.Load( asmName );
            return asm.CreateInstance( typeName );
        }

        /// <summary>
        /// 初始化匿名类型
        /// </summary>
        /// <param name="t">匿名类型的type</param>
        /// <param name="values">参数的值</param>
        /// <returns></returns>
        public static Object GetAnonymousInstance( Type t, Object[] values ) {
            ConstructorInfo constructor = t.GetConstructors()[0];
            return constructor.Invoke( values );
        }

        public static Object GetInstanceFromProgId( String progId ) {
            return rft.GetInstance( Type.GetTypeFromProgID( progId ) );
        }

        //---------------------------------------------------------------------------------------------------------------

        public static IList GetPropertyList( Type t ) {
            PropertyInfo[] properties = t.GetProperties( BindingFlags.Public | BindingFlags.Instance );
            IList list = new ArrayList();
            foreach (PropertyInfo info in properties) {
                list.Add( info );
            }
            return list;
        }

        public static Object GetPropertyValue( Object currentObject, String propertyName ) {

            if (currentObject == null) return null;

            if (strUtil.IsNullOrEmpty( propertyName )) return null;

            PropertyInfo p = currentObject.GetType().GetProperty( propertyName );
            if (p == null) return null;

            return p.GetValue( currentObject, null );
        }

        public static void SetPropertyValue( Object currentObject, String propertyName, Object propertyValue ) {

            if (currentObject == null) {
                throw new NullReferenceException( String.Format( "propertyName={0}, propertyValue={1}", propertyName, propertyValue ) );
            }

            PropertyInfo p = currentObject.GetType().GetProperty( propertyName );
            if (p == null) {
                throw new NullReferenceException( "property not exist=" + propertyName + ", type=" + currentObject.GetType().FullName );
            }

            try {
                p.SetValue( currentObject, propertyValue, null );
            }
            catch (Exception exception) {
                throw new Exception( exception.Message + " (property=" + propertyName + ", type=" + currentObject.GetType().FullName + ")" );
            }
        }

        /// <summary>
        /// 获取属性的类型的fullName(对泛型名称做了特殊处理)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static String getPropertyTypeName( PropertyInfo p ) {

            if (p.PropertyType.IsGenericType == false) {
                return p.PropertyType.FullName;
            }

            Type pGenericType = p.PropertyType.GetGenericTypeDefinition();
            String genericTypeName = pGenericType.FullName.Split( '`' )[0];

            Type[] ts = p.PropertyType.GetGenericArguments();
            String args = null;
            foreach (Type at in ts) {
                if (args != null) args += ", ";
                args += at.FullName;
            }

            return genericTypeName + "<" + args + ">";

        }


        //---------------------------------------------------------------------------------------------------------------

        public static Object CallMethod( Object obj, String methodName ) {
            return CallMethod( obj, methodName, null );
        }

        public static Object CallMethod( Type currentType, String methodName ) {
            return CallMethod( currentType, methodName, null );
        }

        public static Object CallMethod( Object obj, String methodName, object[] args ) {
            return obj.GetType().InvokeMember( methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, obj, args );
        }

        public static Object CallMethod( Type currentType, String methodName, object[] args ) {
            return CallMethod( rft.GetInstance( currentType ), methodName, args );
        }

        /// <summary>
        /// 获取 public 实例方法，不包括继承的方法
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethods( Type t ) {
            return t.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );
        }

        /// <summary>
        /// 获取 public 实例方法，包括继承的方法。子对象的方法在前，父对象的方法在后。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsAll( Type t ) {
            return t.GetMethods( BindingFlags.Public | BindingFlags.Instance );
        }

        /// <summary>
        /// 获取 public 实例方法，包括继承的方法
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsWithInheritance( Type t ) {
            return t.GetMethods( BindingFlags.Public | BindingFlags.Instance );
        }


        //---------------------------------------------------------------------------------------------------------------


        public static Attribute GetAttribute( MemberInfo memberInfo, Type attributeType ) {

            object[] customAttributes = memberInfo.GetCustomAttributes( attributeType, false );
            if (customAttributes.Length == 0) {
                return null;
            }
            return customAttributes[0] as Attribute;
        }

        public static object[] GetAttributes( MemberInfo memberInfo ) {
            return memberInfo.GetCustomAttributes( false );
        }

        public static object[] GetAttributes( MemberInfo memberInfo, Type attributeType ) {
            return memberInfo.GetCustomAttributes( attributeType, false );
        }

        public static Boolean IsBaseType( Type type ) {
            return type == typeof( int ) ||
                type == typeof( String ) ||
                type == typeof( DateTime ) ||
                type == typeof( bool ) ||
                type == typeof( long ) ||
                type == typeof( double ) ||
                type == typeof( decimal );
        }

        /// <summary>
        /// 判断 t 是否实现了某种接口
        /// </summary>
        /// <param name="t">需要判断的类型</param>
        /// <param name="interfaceType">是否实现的接口</param>
        /// <returns></returns>
        public static Boolean IsInterface( Type t, Type interfaceType ) {
            if (t.IsInterface) return false;
            return interfaceType.IsAssignableFrom( t );
        }

        /// <summary>
        /// 判断某个方法是否是接口的实现
        /// </summary>
        /// <param name="method"></param>
        /// <param name="objType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static Boolean IsMethodInInterface( MethodInfo method, Type objType, Type interfaceType ) {

            InterfaceMapping map = objType.GetInterfaceMap( interfaceType );

            foreach (MethodInfo x in map.TargetMethods) {
                if (x == method) return true;
            }

            return false;
        }

        /// <summary>
        /// 判断此 method 是否是属性 property
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Boolean IsMethodProperty( MethodInfo m ) {
            return m.IsSpecialName && (m.Name.StartsWith( "set_" ) || m.Name.StartsWith( "get_" ));
        }


    }
}

