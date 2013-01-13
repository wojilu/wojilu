using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using System.Reflection;
using System.Web;
using System.Collections;

namespace wojilu.Web.Mvc.Utils {

    /// <summary>
    /// 运行某个 controller 的 action，并返回 html 结果
    /// </summary>
    public class ControllerRunner {

        /// <summary>
        /// 运行某 action
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static String Run( MvcContext ctx, aAction action ) {

            ControllerBase targetController = action.Target as ControllerBase;
            ControllerFactory.InjectController( targetController, ctx );
            //ctx.utils.setController( targetController ); // 此处不能改成ctx，在loadHtml中，如果改变ctx，会造成Layout出错

            targetController.view( action.Method.Name );
            action();
            return targetController.utils.getActionResult();
        }

        /// <summary>
        /// 运行某 action
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String Run( MvcContext ctx, aActionWithId action, int id ) {

            ControllerBase targetController = action.Target as ControllerBase;
            ControllerFactory.InjectController( targetController, ctx );
            //ctx.utils.setController( targetController );

            targetController.view( action.Method.Name );
            action( id );
            return targetController.utils.getActionResult();
        }

        /// <summary>
        /// 运行某action，id由ctx.route.id自动注入。
        /// 如果action有参数，请预先设置 ctx.route.id；
        /// 如果方法中涉及到owner，请预先设置 ctx.owner；
        /// controller是经过依赖注入处理的。
        /// 注意1：未处理action过滤器批注。
        /// 注意2：因为通过controller的字符串运行，所以经过反射调用。
        /// </summary>
        /// <param name="ctx">提供 ctx.route.id 和 ctx.owner 等可能需要的参数</param>
        /// <param name="controllerFullName">控制器的完整类型type的full name</param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static String Run( MvcContext ctx, String controllerFullName, String actionName ) {

            ControllerBase targetController = ControllerFactory.FindController( controllerFullName, ctx );
            if (targetController == null) throw new Exception( "controller not found" );
            //ctx.utils.setController( targetController );

            targetController.view( actionName );
            targetController.utils.runAction( actionName );

            return targetController.utils.getActionResult();
        }

        /// <summary>
        /// 运行某个controller的action方法，ctx已经注入controller
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        internal static String runAction( ControllerBase controller, String actionName ) {

            MethodInfo method = getMethod( controller, actionName );
            if (method == null) {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            MvcContext ctx = controller.ctx;

            ParameterInfo[] parameters = getParameters( method );
            if (parameters.Length == 1) {
                if (parameters[0].ParameterType == typeof( String )) {
                    method.Invoke( controller, new object[] { HttpUtility.UrlDecode( ctx.route.query ) } );
                }
                else {
                    method.Invoke( controller, new object[] { ctx.route.id } );
                }
            }
            else if (parameters.Length == 0) {
                method.Invoke( controller, null );
            }
            else {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            return controller.utils.getActionResult();
        }


        /// <summary>
        /// 运行某 action
        /// </summary>
        /// <param name="actionName"></param>
        public static String Run( MvcContext ctx, String controllerName, String actionName, params object[] args ) {

            if (controllerName == null) throw new NullReferenceException();

            ControllerBase controller = ControllerFactory.FindController( controllerName, ctx );
            if (controller == null) throw new Exception( "controller not found" );

            controller.view( actionName );
            //ctx.utils.setController( controller );

            MethodInfo method = getMethod( controller, actionName );
            if (method == null) {
                throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
            }

            method.Invoke( controller, args );

            return controller.utils.getActionResult();
        }

        /// <summary>
        /// 根据名称获取某 action 的方法信息
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static MethodInfo getMethod( ControllerBase controller, String actionName ) {

            return ActionRunner.getActionMethod( controller, actionName );
        }

        /// <summary>
        /// 获取某方法的所有参数信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static ParameterInfo[] getParameters( MethodInfo method ) {
            return method.GetParameters();
        }

        //---------------------------------------------------------------------------------------------------------------------



        /// <summary>
        /// 运行某 controller 的 Layout 方法，controller 已经注入 ctx 中
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static String RunLayout( MvcContext ctx ) {

            ControllerBase controller = ctx.controller;

            String layoutContent;
            if (controller.LayoutControllerType == null) {
                layoutContent = runCurrentLayout( controller, ctx );
            }
            else {
                layoutContent = runOtherLayout( controller, ctx );
            }

            return layoutContent;
        }

        private static String runCurrentLayout( ControllerBase controller, MvcContext ctx ) {

            MethodInfo layoutMethod = controller.utils.getMethod( "Layout" );
            if (layoutMethod.DeclaringType != controller.GetType()) {
                String filePath = MvcUtil.getParentViewPath( layoutMethod, ctx.route.getRootNamespace( layoutMethod.DeclaringType.FullName ) );
                controller.utils.setCurrentView( controller.utils.getTemplateByFileName( filePath ) );
            }
            else {
                controller.utils.switchViewToLayout();
            }

            // 清理当前内容，否则下面的utils.getActionResult()得不到正确结果
            // 因为当前 controller 被其他 action 使用过，所以在运行 Layout() 之前，必须重新 Init 初始化
            controller.utils.initActionResult();            
            controller.Layout();

            return controller.utils.getActionResult();
        }

        private static String runOtherLayout( ControllerBase controller, MvcContext ctx ) {

            ControllerBase layoutController = ControllerFactory.FindController( controller.LayoutControllerType, ctx );
            layoutController.utils.switchViewToLayout();

            ActionRunner.runLayoutAction( ctx, layoutController, layoutController.Layout );

            return layoutController.utils.getActionResult();
        }


    }

}
