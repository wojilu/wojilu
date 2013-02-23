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
using System.Text;
using System.Collections.Generic;
using wojilu.DI;
using wojilu.Reflection;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Routes;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 控制器工厂，通过 IOC 容器创建控制器，实现依赖注入
    /// </summary>
    public class ControllerFactory {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ControllerFactory ) );

        //使用编译后的工厂对性能几乎没有提升，影响微乎其微。
        //private static Dictionary<String, IControllerFactory> factories = ControllerFactoryCompiler.GetCompiledFactory();

        /// <summary>
        /// 根据当前上下文中的路由，创建相应的controller
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ControllerBase InitController( MvcContext ctx ) {

            Route route = ctx.route;

            List<String> rootNamespaceList = MvcConfig.Instance.RootNamespace;

            foreach (String rootNamespace in rootNamespaceList) {
                route.setRootNamespace( rootNamespace );
                String typeName = route.getControllerFullName();
                logger.Debug( "init contrller type=" + typeName );

                ControllerBase controller = FindController( typeName, ctx );
                if (controller != null) return controller;
            }
            return null;
        }

        /// <summary>
        /// 从ObjectContext中创建非单例controller，并初始化(注入ctx和controller所属的appType)
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ControllerBase FindController( String typeName, MvcContext ctx ) {

            if (typeName == null) throw new ArgumentNullException( "typeName" );

            if (ObjectContext.Instance.TypeList.ContainsKey( typeName )) {

                Type controllerType = ObjectContext.Instance.TypeList[typeName] as Type;
                return FindController( controllerType, ctx );

            }
            else if (MvcConfig.Instance.IsUrlToLower
                && ObjectContext.Instance.LowerTypeList.ContainsKey( typeName.ToLower() )
                ) {

                Type controllerType = ObjectContext.Instance.LowerTypeList[typeName.ToLower()] as Type;
                return FindController( controllerType, ctx );
            }

            else {
                return null;
            }
        }

        /// <summary>
        /// 从ObjectContext中创建非单例controller，并初始化(注入ctx和controller所属的appType)
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static ControllerBase FindController( Type controllerType, MvcContext ctx ) {
            if (controllerType == null) return null;

            ControllerBase controller = ObjectContext.CreateObject( controllerType ) as ControllerBase;
            if (controller == null) return null;
            ObjectContext.InterceptProperty( controller );
            controller.setContext( ctx );
            setControllerAppInfo( controllerType, controller );

            return controller;
        }

        /// <summary>
        /// 根据容器配置，将依赖关系注入到已有对象中
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="ctx"></param>
        public static void InjectController( ControllerBase controller, MvcContext ctx ) {
            if (controller == null) return;
            ObjectContext.Inject( controller );
            ObjectContext.InterceptProperty( controller );
            controller.setContext( ctx );
            setControllerAppInfo( controller.GetType(), controller );
        }

        private static void setControllerAppInfo( Type type, ControllerBase controller ) {
            // TODO 缓存起app批注
            AppAttribute attribute = ReflectionUtil.GetAttribute( type, typeof( AppAttribute ) ) as AppAttribute;
            if (attribute != null) {
                controller.utils.setAppType( attribute.AppType );
            }
        }


        public static ControllerBase FindLayoutController( String path, MvcContext ctx ) {

            List<String> rootNamespaceList = MvcConfig.Instance.RootNamespace;

            foreach (String namespaceRoot in rootNamespaceList) {

                String typeFullName;
                if (strUtil.HasText( path ))
                    typeFullName = namespaceRoot + "." + path.Replace( "/", "." ) + ".LayoutController";
                else
                    typeFullName = namespaceRoot + ".LayoutController";

                ControllerBase controller = FindController( typeFullName, ctx );
                if (controller != null) return controller;

            }
            return null;
        }

        public static ControllerBase FindSecurityController( String path, MvcContext ctx ) {


            List<String> rootNamespaceList = MvcConfig.Instance.RootNamespace;

            foreach (String namespaceRoot in rootNamespaceList) {

                String typeFullName;
                if (strUtil.HasText( path ))
                    typeFullName = namespaceRoot + "." + path.Replace( "/", "." ) + ".SecurityController";
                else
                    typeFullName = namespaceRoot + ".SecurityController";

                ControllerBase controller = FindController( typeFullName, ctx );
                if (controller != null) return controller;

            }
            return null;
        }

        public static ControllerBase FindFeedCommentsController( String dataType, MvcContext ctx ) {
            String typeName = strUtil.GetTypeName( dataType ) + "CommentsController";
            foreach (KeyValuePair<String, Type> pair in ObjectContext.Instance.TypeList) {
                if (pair.Value.Name.Equals( typeName )) return FindController( pair.Value, ctx );
            }
            return null;
        }



    }

}
