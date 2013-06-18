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
using System.Reflection;
using wojilu.DI;

namespace wojilu.Aop {

    /// <summary>
    /// Aop 容器，可以创建代理类，或者获取所有被监控的对象、方法。
    /// 本容器创建的所有对象都未经过Ioc处理。
    /// </summary>
    public class AopContext {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AopContext ) );

        private static Dictionary<Type, ObservedType> _oTypes = loadObservers();

        private static readonly Assembly _aopAssembly = loadCompiledAssembly();

        /// <summary>
        /// 获取所有被监控的类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, ObservedType> GetObservedTypes() {
            return _oTypes;
        }

        public static ObservedType GetObservedType( Type t ) {
            ObservedType x;
            _oTypes.TryGetValue( t, out x );
            return x;
        }

        /// <summary>
        /// 获取某方法的监控器
        /// </summary>
        /// <param name="t"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static List<MethodObserver> GetMethodObservers( Type t, String methodName ) {

            ObservedType oType = GetObservedType( t );
            if (oType == null) return new List<MethodObserver>();

            List<ObservedMethod> list = oType.MethodList;
            foreach (ObservedMethod x in list) {
                if (x.Method.Name == methodName) return x.Observer;
            }

            return null;
        }

        /// <summary>
        /// 获取某方法的 "混合运行" 监控器。为了避免被监控方法的多次调用，此监控器只返回第一个。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodObserver GetInvokeObserver( Type t, string methodName ) {

            List<MethodObserver> list = GetMethodObservers( t, methodName );
            foreach (MethodObserver x in list) {

                MethodInfo m = x.GetType().GetMethod( "Invoke" );

                if (m.DeclaringType != typeof( MethodObserver )) {

                    return x;

                }

            }

            return null;
        }

        /// <summary>
        /// 获取所有代理类所在的程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetAopAssembly() {
            return _aopAssembly;
        }

        /// <summary>
        /// 根据类型创建对象。如果被拦截，则创建代理类。否则返回自身的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateObjectBySub<T>() {
            return (T)CreateObjectBySub( typeof( T ) );
        }

        /// <summary>
        /// 根据类型创建对象。如果被拦截，则创建代理类。否则返回自身的实例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object CreateObjectBySub( Type t ) {
            Object result = CreateProxyBySub( t );
            if (result == null) {
                return rft.GetInstance( t );
            }
            else {
                return result;
            }
        }

        /// <summary>
        /// 根据接口创建对象。如果被拦截，则创建代理类。否则返回自身的实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="targetType">需要代理的类型</param>
        /// <returns></returns>
        public static T CreateObjectByInterface<T>( Type targetType ) {
            return (T)CreateObjectByInterface( targetType, typeof( T ) );
        }

        /// <summary>
        /// 根据接口创建对象。如果被拦截，则创建代理类。否则返回自身的实例
        /// </summary>
        /// <param name="targetType">需要代理的类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public static Object CreateObjectByInterface( Type targetType, Type interfaceType ) {

            if (CanCreateInterfaceProxy( targetType, interfaceType )) {
                return createProxyByInterface( targetType, interfaceType );
            }
            else {
                Object objTarget = rft.GetInstance( targetType );
                logger.Info( "can't create interface proxy" );
                return objTarget;
            }
        }

        /// <summary>
        /// 根据类型创建它的代理类。如果代理不存在，返回 null。
        /// 如果方法非虚，不可以创建，那么抛出异常。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateProxyBySub<T>() {
            return (T)CreateProxyBySub( typeof( T ) );
        }

        /// <summary>
        /// 根据类型创建它的代理类。如果代理不存在，返回 null。
        /// 如果方法非虚，不可以创建，那么抛出异常。
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object CreateProxyBySub( Type targetType ) {

            if (targetType == null) throw new NullReferenceException( "targetType" );

            ObservedType oType = GetObservedType( targetType );
            if (oType == null) return null;
            if (oType.CanCreateSubProxy() == false) {
                String exMsg = "method not virtual. type=" + targetType.FullName + ", method=" + oType.GetNotVirtualMethodString();
                logger.Error( exMsg );
                throw new MethodNotVirtualException( exMsg );
            }

            return createProxyBySub( targetType );
        }

        internal static Object createProxyBySub( Type targetType ) {
            AopCoderDialect dialect = new AopCoderDialectSub();
            String name = strUtil.Join( targetType.Namespace, dialect.GetClassFullName( targetType, "" ), "." );
            return _aopAssembly.CreateInstance( name );
        }

        internal static Boolean CanCreateSubProxy( Type targetType ) {
            ObservedType oType = GetObservedType( targetType );
            if (oType == null) return false;
            return oType.CanCreateSubProxy();
        }

        /// <summary>
        /// 根据接口创建代理类。如果不能创建，则返回null
        /// </summary>
        /// <typeparam name="T">接口的类型</typeparam>
        /// <param name="targetType">被代理的类型</param>
        /// <returns></returns>
        public static T CreateProxyByInterface<T>( Type targetType ) {
            return (T)CreateProxyByInterface( targetType, typeof( T ) );
        }

        /// <summary>
        /// 根据接口创建代理类。如果不能创建，则返回null
        /// </summary>
        /// <param name="objTarget"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static Object CreateProxyByInterface( Type targetType, Type interfaceType ) {

            if (targetType == null) throw new NullReferenceException( "targetType" );
            if (interfaceType == null) throw new NullReferenceException( "interfaceType" );

            if (CanCreateInterfaceProxy( targetType, interfaceType ) == false) {
                String exMsg = "cannot create interface proxy. target type=" + targetType.FullName + ", interface type=" + interfaceType;
                logger.Error( exMsg );
                return null;
            }

            return createProxyByInterface( targetType, interfaceType );
        }

        internal static Object createProxyByInterface( Type targetType, Type interfaceType ) {

            AopCoderDialect dialect = new AopCoderDialectInerface();

            String proxyName = strUtil.Join( targetType.Namespace, dialect.GetClassFullName( targetType, interfaceType.FullName ), "." );

            Object interfaceProxy = _aopAssembly.CreateInstance( proxyName );
            if (interfaceProxy == null) {
                String exMsg = "cannot create interface proxy. target type=" + targetType.FullName + ", interface type=" + interfaceType + ", proxy=" + proxyName;
                logger.Error( exMsg );
                return null;
            }

            rft.SetPropertyValue( interfaceProxy, dialect.GetInvokeTargetBase(), rft.GetInstance( targetType ) );

            return interfaceProxy;

        }

        internal static Boolean CanCreateInterfaceProxy( Type targetType, Type interfaceType ) {
            ObservedType oType = GetObservedType( targetType );
            if (oType == null) return false;
            return oType.CanCreateInterfaceProxy( interfaceType );
        }

        private static Assembly loadCompiledAssembly() {

            Dictionary<Type, ObservedType> observers = loadObservers();
            String proxyCode = AopCoder.GetProxyClassCode( observers );
            return AopCoder.CompileCode( proxyCode, ObjectContext.Instance.AssemblyList );
        }

        private static Dictionary<Type, ObservedType> loadObservers() {

            Dictionary<Type, ObservedType> results = new Dictionary<Type, ObservedType>();

            Dictionary<String, Type> typeList = ObjectContext.Instance.TypeList;

            foreach (KeyValuePair<String, Type> kv in typeList) {

                Type type = kv.Value;

                if (type.IsSubclassOf( typeof( MethodObserver ) )) {

                    MethodObserver obj = rft.GetInstance( type ) as MethodObserver;
                    addType( results, obj );
                }

            }

            return results;
        }

        private static void addType( Dictionary<Type, ObservedType> results, MethodObserver obj ) {

            Dictionary<Type, String> dic = obj.GetRelatedMethods();
            foreach (KeyValuePair<Type, String> kv in dic) {

                List<MethodInfo> methods = getMethods( kv.Value, kv.Key );

                foreach (MethodInfo method in methods) {

                    addTypeSingle( results, obj, kv.Key, method );
                }

            }
        }

        private static void addTypeSingle( Dictionary<Type, ObservedType> results, MethodObserver obj, Type t, MethodInfo method ) {

            ObservedType oType;
            results.TryGetValue( t, out oType );
            if (oType == null) {
                oType = new ObservedType();
            }

            oType.Type = t;
            populateMethodList( oType, obj, method );

            results[t] = oType;
        }

        private static void populateMethodList( ObservedType oType, MethodObserver obj, MethodInfo method ) {

            if (oType.MethodList == null) {
                oType.MethodList = new List<ObservedMethod>();
            }

            if (hasAddMethod( oType, method )) {
                oType.MethodList = addObserverToMethodList( oType.MethodList, method, obj );
            }
            else {
                ObservedMethod om = addNewObserverMethod( obj, method );
                om.ObservedType = oType;
                oType.MethodList.Add( om );
            }
        }

        private static List<ObservedMethod> addObserverToMethodList( List<ObservedMethod> list, MethodInfo method, MethodObserver obj ) {

            foreach (ObservedMethod m in list) {
                if (m.Method == method) {
                    addObserverToMethodSingle( m, obj );
                }
            }

            return list;
        }

        private static void addObserverToMethodSingle( ObservedMethod m, MethodObserver obj ) {

            if (m.Observer == null) m.Observer = new List<MethodObserver>();

            if (m.Observer.Contains( obj )) return;

            m.Observer.Add( obj );
        }

        private static ObservedMethod addNewObserverMethod( MethodObserver objObserver, MethodInfo method ) {
            ObservedMethod om = new ObservedMethod();
            om.Method = method;
            om.Observer = new List<MethodObserver>();
            om.Observer.Add( objObserver );
            return om;
        }

        private static bool hasAddMethod( ObservedType ot, MethodInfo method ) {
            foreach (ObservedMethod x in ot.MethodList) {
                if (x.Method == method) {
                    return true;
                }
            }
            return false;
        }

        private static List<MethodInfo> getMethods( String strMethods, Type t ) {

            MethodInfo[] existMethods = t.GetMethods( BindingFlags.Public | BindingFlags.Instance );

            List<MethodInfo> list = new List<MethodInfo>();

            String[] arr = strMethods.Split( '/' );
            foreach (String methodName in arr) {

                if (strUtil.IsNullOrEmpty( methodName )) continue;

                MethodInfo x = getMethodInfo( existMethods, methodName );
                if (x == null) {
                    String exMsg = "method not exist. type=" + t.FullName + ", method=" + methodName;
                    logger.Error( exMsg );
                    throw new MethodNotFoundException( exMsg );
                }


                if (list.Contains( x )) continue;

                list.Add( x );
            }

            return list;
        }

        private static MethodInfo getMethodInfo( MethodInfo[] existMethods, String methodName ) {

            String[] args = new String[] { };
            if (methodName.IndexOf( "(" ) > 0) {

                String[] arrM = methodName.TrimEnd( ')' ).Split( '(' );

                methodName = arrM[0];
                args = arrM[1].Split( ',' );
            }

            foreach (MethodInfo x in existMethods) {

                if (args.Length > 0) {

                    if (isMethodMatch( x, methodName, args )) {
                        return x;
                    }

                }
                else {

                    if (x.Name == methodName) {
                        return x;
                    }

                }
            }
            return null;
        }

        private static Boolean isMethodMatch( MethodInfo x, String methodName, String[] args ) {

            if (x.Name != methodName) return false;

            ParameterInfo[] ps = x.GetParameters();
            if (ps.Length != args.Length) return false;

            for (int i = 0; i < ps.Length; i++) {
                if (ps[i].ParameterType.FullName == args[i]) return true;
            }

            return false;
        }



    }

}
