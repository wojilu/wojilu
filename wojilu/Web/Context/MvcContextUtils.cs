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
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Mvc;
using wojilu.Data;

namespace wojilu.Web.Context {

    /// <summary>
    /// 框架常用高级方法
    /// </summary>
    public class MvcContextUtils {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MvcContext ) );

        private MvcContext ctx;

        private Route _route;
        private IViewerContext _viewerContext;
        private IOwnerContext _ownerContext;
        private ControllerBase _currentController;
        private IAppContext _appContext;

        private String _outputString;
        private IList _layoutPath;

        private List<Type> canceledProcessor = new List<Type>();
        private Exception _exception;
        private Boolean _isHome = false;
        private Boolean _responseEnd = false;

        public MvcContextUtils( MvcContext mvcContext ) {
            this.ctx = mvcContext;
        }

        /// <summary>
        /// 设置所有的布局文件的路径
        /// </summary>
        /// <param name="pathList"></param>
        public void setLayoutPath( IList pathList ) { _layoutPath = pathList; }

        /// <summary>
        /// 获取所有的布局文件的路径
        /// </summary>
        /// <returns></returns>
        public IList getLayoutPath() {
            if (_layoutPath == null) _layoutPath = PathHelper.GetPathList( ctx.route.getRootNamespace(), ctx.controller.GetType().Namespace );
            return _layoutPath;
        }

        /// <summary>
        /// 设置当前需要呈现给客户端的 html 内容
        /// </summary>
        /// <param name="str"></param>
        public void setCurrentOutputString( String str ) { _outputString = str; }

        /// <summary>
        /// 获取当前需要呈现给客户端的 html 内容
        /// </summary>
        /// <returns></returns>
        public String getCurrentOutputString() { return _outputString; }

        /// <summary>
        /// 设置当前路由 route
        /// </summary>
        /// <param name="route"></param>
        public void setRoute( Route route ) { _route = route; }

        /// <summary>
        /// 获取当前路由 route
        /// </summary>
        /// <returns></returns>
        public Route getRoute() { return _route; }

        /// <summary>
        /// 设置当前访问者信息
        /// </summary>
        /// <param name="vctx"></param>
        public void setViewerContext( IViewerContext vctx ) { _viewerContext = vctx; }

        /// <summary>
        /// 获取当前访问者信息
        /// </summary>
        /// <returns></returns>
        public IViewerContext getViewerContext() { return _viewerContext; }

        /// <summary>
        /// 设置当前被访问者(网站、群组、用户空间等)信息
        /// </summary>
        /// <param name="octx"></param>
        public void setOwnerContext( IOwnerContext octx ) { _ownerContext = octx; }

        /// <summary>
        /// 获取当前访问者信息
        /// </summary>
        /// <returns></returns>
        public IOwnerContext getOwnerContext() { return _ownerContext; }

        /// <summary>
        /// 设置当前控制器 controller
        /// </summary>
        /// <param name="controller"></param>
        public void setController( ControllerBase controller ) { _currentController = controller; }

        /// <summary>
        /// 获取当前控制器 controller
        /// </summary>
        /// <returns></returns>
        public ControllerBase getController() { return _currentController; }

        /// <summary>
        /// 设置当前 app 信息
        /// </summary>
        /// <param name="appctx"></param>
        public void setAppContext( IAppContext appctx ) { _appContext = appctx; appctx.setContext( this.ctx ); }

        /// <summary>
        /// 获取当前 app 信息
        /// </summary>
        /// <returns></returns>
        public IAppContext getAppContext() { return _appContext; }

        /// <summary>
        /// 取消某个 mvc 处理器：在后续流程中，会跳过此处理器
        /// </summary>
        /// <param name="t"></param>
        public void CancelMvcProcessor( Type t ) { canceledProcessor.Add( t ); }

        /// <summary>
        /// 获取所有被取消的 mvc 处理器
        /// </summary>
        /// <returns></returns>
        public List<Type> GetCancelMvcProcessor() { return canceledProcessor; }

        private Boolean _skipCurrentProcessor = false;

        /// <summary>
        /// 跳过当前处理器的后续内容，进入下一个处理器。
        /// </summary>
        public void skipCurrentProcessor( Boolean isSkip ) {
            _skipCurrentProcessor = isSkip;
        }

        /// <summary>
        /// 是否跳过当前处理器的后续内容
        /// </summary>
        /// <returns></returns>
        public Boolean isSkipCurrentProcessor() {
            return _skipCurrentProcessor;
        }

        /// <summary>
        /// 设置当前异常
        /// </summary>
        /// <param name="ex"></param>
        public void setException( Exception ex ) { _exception = ex; }

        /// <summary>
        /// 获取当前异常
        /// </summary>
        /// <returns></returns>
        public Exception getException() { return _exception; }

        /// <summary>
        /// 设置当前是否被访问者首页（网站首页，或群组首页，或空间首页等）
        /// </summary>
        /// <param name="isHome"></param>
        public void setIsHome( Boolean isHome ) { _isHome = isHome; }

        /// <summary>
        /// 当前是否是被访问者首页（网站首页，或群组首页，或空间首页等）
        /// </summary>
        /// <returns></returns>
        public Boolean getIsHome() { return _isHome; }

        /// <summary>
        /// 结束所有后续流程，进入 render 阶段(直接由 RenderProcessor 处理)。
        /// 如果连 RenderProcessor 都想取消，请另外再调用方法 skipRender() 
        /// </summary>
        public void end() { _responseEnd = true; }

        /// <summary>
        /// 是否后续流程都被取消
        /// </summary>
        /// <returns></returns>
        public Boolean isEnd() { return _responseEnd; }

        private Boolean _isSkipRender = false;

        /// <summary>
        /// 跳过 render 步骤
        /// </summary>
        public void skipRender() { _isSkipRender = true; }

        /// <summary>
        /// 是否跳过 render 步骤
        /// </summary>
        /// <returns></returns>
        public Boolean IsSkinRender() { return _isSkipRender; }

        /// <summary>
        /// 当前请求是否来自 ajax
        /// </summary>
        public Boolean isAjax {
            get { return ((ctx.Get( "ajax" ) != null) && ctx.Get( "ajax" ).Equals( "true" )); }
        }

        /// <summary>
        /// 当前请求是否来自 frame
        /// </summary>
        /// <returns></returns>
        public Boolean isFrame() {
            return ctx.Params( "frm" ) != null && ctx.Params( "frm" ).Equals( "true" );
        }

        /// <summary>
        /// 当前请求是否来自box
        /// </summary>
        /// <returns></returns>
        public Boolean isBox() {
            return ctx.Params( "srcFrmId" ) != null;
        }

        /// <summary>
        /// 禁止呈现的layout层级，从顶部开始，1表示禁止呈现第一个layout，2表示禁止呈现1和2这两个layout……n表示从1到n的所有layout都禁止呈现，999表示禁止呈现所有layout，0或者-1则表示呈现全部layout
        /// </summary>
        /// <returns></returns>
        public int getNoLayout() {

            return ctx.ParamInt( "nolayout" );
        }

        /// <summary>
        /// 清理当前连接中的资源：关闭数据库连接，将日志写入磁盘
        /// </summary>
        public void clearResource() {

            // 完成尚未提交的事务
            DbContext.commitAll();

            DbContext.closeConnectionAll();

            String strMock = ctx.IsMock ? " [mocked]" : "";

            if (ctx.utils.isAjax) {
                logger.Info( "------------------ ajax end" + strMock + "(" + ctx.web.PathAbsolute + ") ------------------" );
            }
            else {
                logger.Info( "------------------- output end" + strMock + "(" + ctx.web.PathAbsolute + ")  ---------------------" );
            }

            LogManager.Flush();
        }


        public void setPageMeta( PageMeta p ) {

            ctx.Page.Title = p.Title;
            ctx.Page.Description = p.Description;
            ctx.Page.Keywords = p.Keywords;
            ctx.Page.RssLink = p.RssLink;

        }

        /// <summary>
        /// 显示信息，并发出 http 状态码(可选)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="httpStatusCode">可以为null</param>
        public void endMsg( String msg, String httpStatusCode ) {
            if (httpStatusCode != null) {
                this.ctx.web.ResponseStatus( httpStatusCode );
            }

            if (this.ctx.utils.isAjax) {
                endMsgByText( msg );
            }
            else {
                endMsgByView( msg );
            }
        }

        /// <summary>
        /// 显示信息(不带模板)，然后结束下面的流程
        /// </summary>
        /// <param name="msg"></param>
        public void endMsgByText( String msg ) {
            this.setCurrentOutputString( msg );
            this.end();
        }

        /// <summary>
        /// 显示信息(带模板)，然后结束下面的流程
        /// </summary>
        /// <param name="msg"></param>
        public void endMsgByView( String msg ) {
            endMsgByView( msg, MvcConfig.Instance.GetMsgTemplatePath() );
        }

        /// <summary>
        /// 使用 box 模板显示信息，然后结束下面的流程
        /// </summary>
        /// <param name="msg"></param>
        public void endMsgBox( String msg ) {
            endMsgByView( msg, MvcConfig.Instance.GetMsgBoxTemplatePath() );
        }

        /// <summary>
        /// 使用 forward 模板显示信息，然后结束下面的流程
        /// </summary>
        /// <param name="msg"></param>
        public void endMsgForward( String msg ) {
            endMsgByView( msg, MvcConfig.Instance.GetForwardTemplatePath() );
        }

        /// <summary>
        /// 根据指定模板显示信息，然后结束下面的流程
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="templatePath">模板路径</param>
        public void endMsgByView( String msg, String templatePath ) {

            ITemplate msgView = getMsgTemplate( msg, templatePath );
            this.setCurrentOutputString( msgView.ToString() );
            this.end();
        }

        public ITemplate getMsgTemplate( String msg, String templatePath ) {

            String viewsPath = PathHelper.Map( templatePath );

            Template msgView = new Template( viewsPath );
            msgView.Set( "siteName", config.Instance.Site.SiteName );
            msgView.Set( "siteUrl", SystemInfo.SiteRoot );
            msgView.Set( "msg", msg );
            this.setGlobalVariable( msgView );

            return msgView;
        }


        /// <summary>
        /// 给模板中的全局变量赋值(不带app lang)
        /// </summary>
        /// <param name="tpl"></param>
        public void setGlobalVariable( ITemplate tpl ) {

            if (tpl == null) return;

            tpl.Bind( "lang", wojilu.lang.getCoreLang().getLangMap() );

            tpl.Set( "langStr", wojilu.lang.getLangString() );
            tpl.Set( "jsVersion", MvcConfig.Instance.JsVersion );
            tpl.Set( "cssVersion", MvcConfig.Instance.CssVersion );

            tpl.Set( "path", sys.Path.Root );
            tpl.Set( "path.img", sys.Path.Img );
            tpl.Set( "path.css", sys.Path.Css );
            tpl.Set( "path.js", sys.Path.Js );
            tpl.Set( "path.static", sys.Path.Static );
            tpl.Set( "path.skin", sys.Path.Skin );
        }



    }

}
