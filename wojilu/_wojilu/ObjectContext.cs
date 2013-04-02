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

using wojilu.Aop;
using wojilu.DI;
using wojilu.Reflection;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Interface;

namespace wojilu {

    /// <summary>
    /// 管理对象 Object 的容器。
    /// 1. 保存了所有纳入容器的类型 Type。
    /// 2. 将 Object 进行 Aop 拦截和 Ioc 注入处理。
    /// </summary>
    public class ObjectContext {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ObjectContext ) );

        private static Object syncRoot = new object();
        private static volatile ObjectContext _instance;

        private ObjectContext() {
        }

        /// <summary>
        /// 容器的实例(单例)
        /// </summary>
        public static ObjectContext Instance {
            get {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null) {
                            ObjectContext ctx = new ObjectContext();
                            InitInject( ctx );
                            _instance = ctx;
                        }
                    }
                }
                return _instance;
            }
        }

        private static void InitInject( ObjectContext ctx ) {
            loadAssemblyAndTypes( ctx );
            resolveAndInject( ctx );
            addNamedObjects( ctx );
            addLowerTypeList( ctx );
        }

        private static void addLowerTypeList( ObjectContext ctx ) {
            foreach (KeyValuePair<String, Type> kv in ctx.TypeList) {
                ctx.LowerTypeList.Add( kv.Key.ToLower(), kv.Value );
            }
        }

        private Dictionary<String, Assembly> _assemblyList = new Dictionary<String, Assembly>();
        private Dictionary<String, Type[]> _assemblyTypes = new Dictionary<String, Type[]>();
        private Dictionary<String, Type> _typeList = new Dictionary<String, Type>();
        private Dictionary<String, Type> _lowerTypeList = new Dictionary<String, Type>();


        private Dictionary<String, MapItem> _resolvedMap = new Dictionary<String, MapItem>();

        private Hashtable _objectsContainerByName = new Hashtable();
        private Hashtable _objectsContainerByType = new Hashtable();

        private Dictionary<String, IDto> _dtoList = new Dictionary<string, IDto>();

        /// <summary>
        /// 所有纳入容器管理的程序集
        /// </summary>
        public Dictionary<String, Assembly> AssemblyList {
            get { return _assemblyList; }
            set { _assemblyList = value; }
        }

        /// <summary>
        /// 所有程序集的 Dictionary
        /// </summary>
        public Dictionary<String, Type[]> AssemblyTypes {
            get { return _assemblyTypes; }
            set { _assemblyTypes = value; }
        }

        /// <summary>
        /// 已经解析过的类型
        /// </summary>
        public Dictionary<String, MapItem> ResolvedMap {
            get { return _resolvedMap; }
            set { _resolvedMap = value; }
        }

        /// <summary>
        /// 所有纳入容器管理的类型
        /// </summary>
        public Dictionary<String, Type> TypeList {
            get { return _typeList; }
            set { _typeList = value; }
        }

        /// <summary>
        /// 所有纳入容器管理的类型(小写)
        /// </summary>
        public Dictionary<String, Type> LowerTypeList {
            get { return _lowerTypeList; }
            set { _lowerTypeList = value; }
        }

        /// <summary>
        /// 根据名称罗列的对象表
        /// </summary>
        public Hashtable ObjectsByName {
            get { return _objectsContainerByName; }
            set { _objectsContainerByName = value; }
        }

        /// <summary>
        /// 根据类型罗列的对象表，用于存储单例对象
        /// </summary>
        public Hashtable ObjectsByType {
            get { return _objectsContainerByType; }
            set { _objectsContainerByType = value; }
        }

        /// <summary>
        /// 获取所有的dto(工厂)，用于创建dto对象
        /// </summary>
        public Dictionary<String, IDto> DtoList {
            get { return _dtoList; }
            set { _dtoList = value; }
        }


        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 根据名称获取类型 t
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Type GetType( String typeFullName ) {
            Type t;
            Instance.TypeList.TryGetValue( typeFullName, out t );
            return t;
        }

        /// <summary>
        /// 根据依赖注入的配置文件中的 name 获取对象。根据配置属性Singleton决定是否单例。
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static Object GetByName( String objectName ) {

            if (Instance.ResolvedMap.ContainsKey( objectName ) == false) return null;

            MapItem item = Instance.ResolvedMap[objectName];
            if (item == null) return null;

            if (item.Singleton)
                return Instance.ObjectsByName[objectName];
            else
                return createInstanceAndInject( item );
        }

        /// <summary>
        /// 从缓存中取对象(经过Aop和Ioc处理)，结果是单例
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Object GetByType( String typeFullName ) {
            if (Instance.TypeList.ContainsKey( typeFullName ) == false) return null;
            Type t = Instance.TypeList[typeFullName];
            return GetByType( t );
        }

        /// <summary>
        /// 从缓存中取对象(经过Aop和Ioc处理)，结果是单例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object GetByType( Type t ) {

            if (t == null) return null;

            Object result = Instance.ObjectsByType[t.FullName];

            if (result == null) {

                result = CreateObject( t );

                Instance.ObjectsByType[t.FullName] = result;

            }

            return result;
        }

        /// <summary>
        /// 创建一个经过Aop和Ioc处理的对象，结果不是单例。
        /// 如果需要拦截，则创建代理子类；然后检测是否需要注入。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>() {
            return (T)CreateObject( typeof( T ) );
        }

        /// <summary>
        /// 创建一个经过Aop和Ioc处理的对象，结果不是单例。
        /// 如果需要拦截，则创建代理子类；然后检测是否需要注入。
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Object CreateObject( String typeFullName ) {
            Type t = GetType( typeFullName );
            if (t == null) return null;
            return CreateObject( t );
        }

        /// <summary>
        /// 创建一个经过Aop和Ioc处理的对象，结果不是单例。
        /// 如果需要拦截，则创建代理子类；然后检测是否需要注入。
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Object Create( String typeFullName ) {
            return CreateObject( typeFullName );
        }

        /// <summary>
        /// 创建一个经过Aop和Ioc处理的对象，结果不是单例。
        /// 如果需要拦截，则创建代理子类；然后检测是否需要注入。
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object Create( Type targetType ) {
            return CreateObject( targetType );
        }

        /// <summary>
        /// 创建一个经过Aop和Ioc处理的对象，结果不是单例。
        /// 如果需要拦截，则创建代理子类；然后检测是否需要注入。
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Object CreateObject( Type targetType ) {
            Object objTarget = AopContext.CreateObjectBySub( targetType );
            Inject( objTarget );
            return objTarget;
        }

        /// <summary>
        /// 根据接口创建代理类，并经过注入处理。
        /// 如果无法创建接口代理，则返回未经代理，但注入过的对象。
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="targetType">需要代理的类型</param>
        /// <returns></returns>
        public static T Create<T>( Type targetType ) {
            return (T)Create( targetType, typeof( T ) );
        }

        /// <summary>
        /// 根据接口创建代理类，并经过注入处理。
        /// 如果无法创建接口代理，则返回未经代理，但注入过的对象。
        /// </summary>
        /// <param name="targetType">需要代理的类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public static Object Create( Type targetType, Type interfaceType ) {
            Object objTarget = AopContext.CreateObjectByInterface( targetType, interfaceType );
            Inject( objTarget );
            return objTarget;
        }

        /// <summary>
        /// 根据type，不从缓存(pool)中取，而是全新创建实例(有注入的就注入，没有注入的直接生成)，肯定不是单例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object CreateObjectByIoc( Type t ) {
            if (t == null) return null;

            MapItem mapItem = getMapItemByType( t );
            if (mapItem == null)
                return rft.GetInstance( t );
            else
                return createInstanceAndInject( mapItem );
        }

        /// <summary>
        /// 根据type，不从缓存(pool)中取，而是全新创建实例(有注入的就注入，没有注入的直接生成)，肯定不是单例
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Object CreateObjectByIoc( String typeFullName ) {
            Type t = GetType( typeFullName );
            if (t == null) return null;
            return CreateObjectByIoc( t );
        }


        private static MapItem getMapItemByType( Type t ) {

            Dictionary<String, MapItem> resolvedMap = Instance.ResolvedMap;
            foreach (KeyValuePair<String, MapItem> entry in resolvedMap) {
                MapItem item = entry.Value;
                if (t.FullName.Equals( item.Type )) return item;
            }
            return null;
        }

        private static Object createInstanceAndInject( MapItem item ) {
            Object currentObject = rft.GetInstance( item.TargetObject.GetType() );
            Dictionary<String, String> maps = item.Map;
            if (maps.Count > 0) {
                foreach (KeyValuePair<String, String> entry in maps) {
                    Object propertyValue = GetByName( entry.Value );
                    if (propertyValue != null) {
                        ReflectionUtil.SetPropertyValue( currentObject, entry.Key, propertyValue );
                    }
                }
            }
            return currentObject;
        }


        /// <summary>
        /// 用Aop拦截所有属性对象。
        /// 如果属性有接口，按照接口拦截；否则按照子类拦截
        /// </summary>
        /// <param name="objTarget"></param>
        public static Object InterceptProperty( Object objTarget ) {

            Type targetType = objTarget.GetType();

            PropertyInfo[] properties = targetType.GetProperties( BindingFlags.Public | BindingFlags.Instance );

            foreach (PropertyInfo p in properties) {
                if (!p.CanRead) continue;
                if (!p.CanWrite) continue;

                if (rft.IsBaseType( p.PropertyType )) continue;

                Object oValue = p.GetValue( objTarget, null );
                if (oValue == null) {
                    logger.Info( "property value is null. type = " + targetType.FullName + ", property = " + p.Name );
                    continue;
                }

                Type pType = oValue.GetType();
                Object propertyValue = oValue;
                if (p.PropertyType.IsInterface) {
                    if (AopContext.CanCreateInterfaceProxy( pType, p.PropertyType )) {
                        propertyValue = AopContext.createProxyByInterface( pType, p.PropertyType );
                        if (propertyValue != null) {
                            p.SetValue( objTarget, propertyValue, null );
                        }
                    }
                }
                else if (AopContext.CanCreateSubProxy( pType )) {
                    propertyValue = AopContext.createProxyBySub( pType );
                    if (propertyValue != null) {
                        p.SetValue( objTarget, propertyValue, null );
                    }
                }

            }

            return objTarget;
        }


        //----------------------------------------------------------------------------------------------


        private static void loadAssemblyAndTypes( ObjectContext ctx ) {

            String appSettings = cfgHelper.GetAppSettings( "InjectAssembly" );
            if (strUtil.IsNullOrEmpty( appSettings )) return;

            String[] strArray = appSettings.Split( new char[] { ',' } );
            foreach (String asmStr in strArray) {

                if (strUtil.IsNullOrEmpty( asmStr )) continue;
                String asmName = asmStr.Trim();
                Assembly assembly = loadAssemblyPrivate( asmName, ctx );
                findTypesPrivate( assembly, asmName, ctx );
            }
        }

        private static Assembly loadAssemblyPrivate( String asmName, ObjectContext ctx ) {
            Assembly assembly = Assembly.Load( asmName );
            ctx.AssemblyList.Add( asmName, assembly );
            return assembly;
        }

        private static void findTypesPrivate( Assembly assembly, String asmName, ObjectContext ctx ) {
            Type[] types = assembly.GetTypes();
            ctx.AssemblyTypes.Add( asmName, types );

            foreach (Type type in types) {

                if (type.Name.StartsWith( "<>f__AnonymousType" )) continue;

                try {
                    ctx.TypeList.Add( type.FullName, type );
                }
                catch (Exception ex) {
                    throw new Exception( ex.Message + ":" + type.FullName, ex );
                }

                if (rft.IsInterface( type, typeof( IDto ) )) {
                    ctx.DtoList.Add( strUtil.TrimEnd( type.FullName, "Dto" ), (IDto)rft.GetInstance( type ) );
                }

            }
        }

        /// <summary>
        /// 加载程序集并返回此程序集，如果容器中已存在，则直接从容器中获取
        /// </summary>
        /// <param name="asmName"></param>
        /// <returns></returns>
        public static Assembly LoadAssembly( String asmName ) {
            Assembly assembly;
            Instance.AssemblyList.TryGetValue( asmName, out assembly );
            if (assembly == null) {
                try {
                    assembly = Assembly.Load( asmName );
                }
                catch (Exception ex) {
                    String msg = string.Format( "无法加载程序集:{0}, 请检查名称是否正确", asmName );
                    logger.Error( msg );
                    logger.Error( ex.Message );
                    logger.Error( ex.StackTrace );
                    throw new AssemblyNotFoundException( msg );
                }
                Instance.AssemblyList.Add( asmName, assembly );
            }
            return assembly;
        }

        /// <summary>
        /// 加载某程序集里的所有类型，如果容器中已存在，则直接从容器中获取
        /// </summary>
        /// <param name="asmName"></param>
        /// <returns></returns>
        public static Type[] FindTypes( String asmName ) {
            Type[] types;// = Instance.AssemblyTypes[asmName];
            Instance.AssemblyTypes.TryGetValue( asmName, out types );
            if (types == null) {
                types = LoadAssembly( asmName ).GetTypes();
                Instance.AssemblyTypes.Add( asmName, types );
                foreach (Type type in types) {
                    try {
                        Instance.TypeList.Add( type.FullName, type );
                    }
                    catch (Exception ex) {
                        String msg = "type exist: " + type.FullName;
                        logger.Error( msg );
                        throw new Exception( msg, ex );
                    }
                }
            }
            return types;
        }

        //----------------------------------------------------------------------------------------------

        private static void resolveAndInject( ObjectContext ctx ) {
            List<MapItem> maps = cdb.findAll<MapItem>();
            if (maps.Count <= 0) return;

            Dictionary<String, MapItem> resolvedMap = new Dictionary<String, MapItem>();

            logger.Info( "resolve item begin..." );
            resolveMapItem( maps, resolvedMap, ctx );

            logger.Info( "inject Object begin..." );
            injectObjects( maps, resolvedMap );

            ctx.ResolvedMap = resolvedMap;
        }

        private static void resolveMapItem( List<MapItem> maps, Dictionary<String, MapItem> resolvedMap, ObjectContext ctx ) {
            foreach (MapItem oneMap in maps) {
                Type type;// = ctx.TypeList[oneMap.Type];
                ctx.TypeList.TryGetValue( oneMap.Type, out type );
                if (type == null) continue;
                logger.Info( "resolve:" + oneMap.Name );

                if (oneMap.Singleton) {
                    // ObjectsByType中存储的都是单例对象
                    oneMap.TargetObject = checkByCache( type, ctx );
                }
                else {
                    oneMap.TargetObject = rft.GetInstance( type );
                }
                resolvedMap[oneMap.Name] = oneMap;

            }
        }

        private static Object checkByCache( Type t, ObjectContext ctx ) {
            if (ctx.ObjectsByType[t.FullName] == null) {
                ctx.ObjectsByType.Add( t.FullName, rft.GetInstance( t ) );
            }
            return ctx.ObjectsByType[t.FullName];
        }


        private static void injectObjects( List<MapItem> mapItems, Dictionary<String, MapItem> resolvedMap ) {
            foreach (MapItem item in mapItems) {
                logger.Info( "inject:" + item.Name );
                Dictionary<String, String> maps = item.Map;
                if (maps.Count <= 0) continue;

                foreach (KeyValuePair<String, String> entry in maps) {
                    logger.Info( "------inject key:" + entry.Key );
                    MapItem referencedItem;// = resolvedMap[entry.Value.ToString()];
                    resolvedMap.TryGetValue( entry.Value, out referencedItem );
                    if (referencedItem != null) {
                        ReflectionUtil.SetPropertyValue( item.TargetObject, entry.Key, referencedItem.TargetObject );
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------

        private static void addNamedObjects( ObjectContext ctx ) {
            Dictionary<String, MapItem> resolvedMap = ctx.ResolvedMap;
            Hashtable namedObjects = new Hashtable();
            foreach (KeyValuePair<String, MapItem> entry in resolvedMap) {
                MapItem item = entry.Value;
                namedObjects.Add( item.Name, item.TargetObject );
            }
            ctx.ObjectsByName = namedObjects;
        }


        /// <summary>
        /// 根据容器配置，将依赖关系注入到已创建的对象中
        /// </summary>
        /// <param name="obj"></param>
        public static void Inject( Object obj ) {

            if (obj == null) return;

            Type t = obj.GetType();

            Dictionary<String, MapItem> resolvedMap = ObjectContext.Instance.ResolvedMap;

            foreach (KeyValuePair<String, MapItem> pair in resolvedMap) {

                MapItem item = pair.Value;
                if (item.Type.Equals( t.FullName ) == false) continue;

                Dictionary<String, String> maps = item.Map;
                if (maps.Count <= 0) return;

                injectObjectSingle( obj, resolvedMap, maps );
                return;

            }
        }

        private static void injectObjectSingle( Object obj, Dictionary<String, MapItem> resolvedMap, Dictionary<String, String> maps ) {
            foreach (KeyValuePair<String, String> entry in maps) {

                logger.Info( "------inject key:" + entry.Key );
                MapItem referencedItem;
                resolvedMap.TryGetValue( entry.Value, out referencedItem );
                if (referencedItem != null) {
                    ReflectionUtil.SetPropertyValue( obj, entry.Key, referencedItem.TargetObject );
                }

            }
        }

    }
}

