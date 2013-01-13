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
using System.Reflection;
using System.Web;

using wojilu.Reflection;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Mvc.Utils;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 控制器中的一些高级方法，主要由框架调用
    /// </summary>
    public class ControllerCore {

        private ControllerBase controller;
        private MvcContext ctx;

        public ControllerCore( ControllerBase c ) {
            controller = c;
            ctx = c.ctx;
        }

        private Template _currentView;

        /// <summary>
        /// 设置当前模板
        /// </summary>
        /// <param name="tpl">模板对象</param>
        public void setCurrentView( Template tpl ) {
            _currentView = tpl;
            this.ctx.utils.setGlobalVariable( tpl );
            if (this.getAppLang() != null) tpl.Bind( "alang", getAppLang().getLangMap() );
        }

        /// <summary>
        /// 获取当前模板
        /// </summary>
        /// <returns>返回一个模板对象</returns>
        public Template getCurrentView() {
            return _currentView;
        }

        private Boolean _isSetContent = false;
        private String _actionContent;

        /// <summary>
        /// 得到当前 action 的运行结果
        /// </summary>
        /// <returns></returns>
        public String getActionResult() {

            if (_isSetContent) return _actionContent;

            Template t = this.getCurrentView();
            return t == null ? null : t.ToString();
        }

        /// <summary>
        /// 设置当前 action 的运行结果
        /// </summary>
        /// <param name="content"></param>
        public void setActionContent( String content ) {
            _actionContent = content;
            _isSetContent = true;
        }

        /// <summary>
        /// 请当前 action 内容清空，保持初始状态
        /// </summary>
        internal void initActionResult() {
            _actionContent = null;
            _isSetContent = false;
        }

        private Boolean _isappLangLoaded = false;
        private LanguageSetting _langSetting;

        /// <summary>
        /// 获取当前 app 的语言包数据
        /// </summary>
        /// <returns></returns>
        public LanguageSetting getAppLang() {

            if (_isappLangLoaded) return _langSetting;

            if (ctx.app != null && ctx.app.getAppType() != null) {
                _langSetting = wojilu.lang.getByApp( ctx.app.getAppType() );
            }

            _isappLangLoaded = true;

            return _langSetting;
        }

        /// <summary>
        /// 获取某方法的所有过滤器 ActionFilter 列表
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public List<IActionFilter> getActionFilters( MethodInfo method ) {

            List<IActionFilter> list = new List<IActionFilter>();

            object[] filters = controller.utils.getAttributes( method, typeof( IActionFilter ) );
            if (filters == null || filters.Length == 0) return list;

            foreach (Object obj in filters) list.Add( (IActionFilter)obj );

            list.Sort( this.Compare );
            return list;
        }

        /// <summary>
        /// 给过滤器排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Compare( IActionFilter x, IActionFilter y ) {
            if (x.Order > y.Order) return 1;
            if (x.Order < y.Order) return -1;
            return 0;
        }

        /// <summary>
        /// 合并需要隐藏的 LayoutController
        /// </summary>
        /// <param name="hidedCcontroller"></param>
        internal void addHidedLayouts( ControllerBase hidedCcontroller ) {
            controller.hidedLayouts.AddRange( hidedCcontroller.hidedLayouts );
        }

        internal Boolean isHided( Type layoutType ) {
            return controller.hidedLayouts.Contains( layoutType );
        }

        internal Boolean isCheckPermission( Type layoutType ) {
            return !controller.hidedPermission.Contains( layoutType );
        }

        private Type _appType;

        /// <summary>
        /// 设置当前的 app 类型
        /// </summary>
        /// <param name="t"></param>
        public void setAppType( Type t ) { _appType = t; }

        /// <summary>
        /// 获取当前的 app 类型
        /// </summary>
        /// <returns></returns>
        public Type getAppType() { return _appType; }



        /// <summary>
        /// 将页面元信息(包括Title/Keywords/Description/RssLink)赋予模板
        /// </summary>
        public void renderPageMetaToView() {

            if (strUtil.IsNullOrEmpty( controller.Page.Title )) controller.Page.Title = config.Instance.Site.PageDefaultTitle;
            if (strUtil.IsNullOrEmpty( controller.Page.Keywords )) controller.Page.Keywords = config.Instance.Site.Keywords;
            if (strUtil.IsNullOrEmpty( controller.Page.Description )) controller.Page.Description = config.Instance.Site.Description;

            String lnkRss = controller.Page.RssLink;
            if (strUtil.HasText( lnkRss )) {
                if (lnkRss.StartsWith( "<link title=" ) == false) {
                    lnkRss = String.Format( "<link title=\"RSS\" type=\"application/rss+xml\" rel=\"alternate\" href=\"{0}\" />", lnkRss );
                }
                controller.Page.RssLink = lnkRss;
            }

        }

        internal void switchViewToLayout() {
            setCurrentView( getTemplateByAction( "Layout" ) );
        }

        /// <summary>
        /// 根据 action 名称获取模板对象
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Template getTemplateByAction( String action ) {
            return new Template( getTemplatePathByAction( action ) );
        }

        private String getControllerDir() {

            String pathRaw = strUtil.GetTypeFullName( controller.GetType() );

            // 去掉根目录
            String result = trimRootNamespace( pathRaw ).TrimStart( '.' );

            // 换成路径分隔符
            result = result.Replace( '.', '/' );

            String pathRoot = MvcConfig.Instance.ViewDir;

            result = strUtil.Join( pathRoot, result );
            result = strUtil.TrimEnd( result, "Controller" );

            return result;
        }

        private String trimRootNamespace( String pathRaw ) {

            foreach (String ns in MvcConfig.Instance.RootNamespace) {
                if (pathRaw.StartsWith( ns )) return strUtil.TrimStart( pathRaw, ns );
            }

            return pathRaw;
        }


        /// <summary>
        /// 根据文件名称获取模板对象，文件名必须从视图 view 的根目录算起
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Template getTemplateByFileName( String fileName ) {
            return new Template( getTemplatePathByFile( fileName ) );
        }

        /// <summary>
        /// 获取某 action 的模板文件的绝对路径(包括模板的后缀名)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public String getTemplatePathByAction( String action ) {
            return PathHelper.Map( getControllerDir() + "/" + action + MvcConfig.Instance.ViewExt );
        }

        /// <summary>
        /// 获取某模板文件的绝对路径，文件名必须从视图 view 的根目录算起
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public String getTemplatePathByFile( String fileName ) {
            return PathHelper.Map( strUtil.Join( MvcConfig.Instance.ViewDir, fileName ) + MvcConfig.Instance.ViewExt );
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// 运行某 action
        /// </summary>
        public void runAction() {
            runAction( ctx.route.action );
        }

        /// <summary>
        /// 运行某 action
        /// </summary>
        /// <param name="actionName"></param>
        public void runAction( String actionName ) {

            ControllerRunner.runAction( controller, actionName );
        }

        /// <summary>
        /// 根据名称获取某 action 的方法信息
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public MethodInfo getMethod( String actionName ) {

            return ActionRunner.getActionMethod( controller, actionName );
        }

        /// <summary>
        /// 根据批注类型，获取某方法的特定批注
        /// </summary>
        /// <param name="method"></param>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public Attribute getAttribute( MethodInfo method, Type attrType ) {
            return ReflectionUtil.GetAttribute( method, attrType );
        }

        /// <summary>
        /// 获取某方法的所有批注
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public object[] getAttributesAll( MethodInfo method ) {
            return ReflectionUtil.GetAttributes( method );
        }

        /// <summary>
        /// 根据批注类型，获取某方法的特定批注列表
        /// </summary>
        /// <param name="method"></param>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public object[] getAttributes( MethodInfo method, Type attrType ) {
            return ReflectionUtil.GetAttributes( method, attrType );
        }

        /// <summary>
        /// 获取某方法的所有 HttpMethod 类型的批注
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public object[] getHttpMethodAttributes( MethodInfo method ) {
            return this.getAttributes( method, typeof( IHttpMethod ) );
        }

        /// <summary>
        /// 获取某方法的所有参数信息
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        //public ParameterInfo[] getParameters( MethodInfo method ) {
        //    return method.GetParameters();
        //}

        //------------------------------------------------------------------------------------------

        private Boolean _isrunAction = true;

        internal Boolean IsRunAction() {
            return _isrunAction;
        }

        internal void IsRunAction( Boolean run ) {
            _isrunAction = run;
        }

    }

}
