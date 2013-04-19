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
using System.Text;

using wojilu.Reflection;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Context;
using wojilu.DI;
using wojilu.Members.Interface;

namespace wojilu.Web.Mvc {


    /// <summary>
    /// 控制器基类，wojilu mvc 中最常使用的对象
    /// </summary>
    public abstract class ControllerBase {

        /// <summary>
        /// 当前 controller 中发生的错误信息
        /// </summary>
        protected Result errors;

        /// <summary>
        /// 当前mvc的上下文(context)，包括一些通用的数据和方法
        /// </summary>
        public MvcContext ctx;

        /// <summary>
        /// 设置或获取布局控制器
        /// </summary>
        public Type LayoutControllerType;

        private ControllerCore _utils;

        /// <summary>
        /// 页面元信息(包括Title/Keywords/Description/RssLink)
        /// </summary>
        public PageMeta Page {
            get { return this.ctx.Page; }
        }

        internal void setContext( MvcContext wctx ) {
            ctx = wctx;
            errors = wctx.errors;
            _utils = new ControllerCore( this );
        }

        /// <summary>
        /// 一些帮助方法，主要是框架调用
        /// </summary>
        public ControllerCore utils {
            get { return _utils; }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        public virtual void CheckPermission() { }

        /// <summary>
        /// 布局方法
        /// </summary>
        public virtual void Layout() { }

        internal readonly List<Type> hidedLayouts = new List<Type>();

        internal readonly List<Type> hidedPermission = new List<Type>();

        /// <summary>
        /// 隐藏某个布局
        /// </summary>
        /// <param name="layoutType">需要被隐藏的 LayoutController 类型</param>
        public void HideLayout( Type layoutType ) {
            hidedLayouts.Add( layoutType );
        }

        /// <summary>
        /// 隐藏某个权限检查步骤
        /// </summary>
        /// <param name="layoutType">需要被隐藏的权限检查 controller 类型</param>
        protected void HidePermission( Type layoutType ) {
            hidedPermission.Add( layoutType );
        }

        //-------------------------------------- 绑定 ----------------------------------------------

        /// <summary>
        /// 绑定链接
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="lbl"></param>
        /// <param name="obj"></param>
        protected virtual void bindOtherLink( IBlock tpl, String lbl, Object obj ) {
        }

        /// <summary>
        /// 根据名称获取模板中某个block
        /// </summary>
        /// <param name="blockName">block名称</param>
        /// <returns></returns>
        public IBlock getBlock( String blockName ) {
            return utils.getCurrentView().GetBlock( blockName );
        }

        /// <summary>
        /// 手动指定当前 action 的视图文件(指定视图文件之后，默认的模板将被忽略)
        /// </summary>
        /// <param name="actionViewName">视图文件的名称，比如 List，或者带上路径 /Blog/Show(路径从视图view根目录算起)</param>
        public void view( String actionViewName ) {
            if (actionViewName == null) throw new ArgumentNullException( "actionViewName" );
            if (actionViewName.IndexOf( '/' ) >= 0) {
                utils.setCurrentView( utils.getTemplateByFileName( actionViewName ) );
            }
            else {
                utils.setCurrentView( utils.getTemplateByAction( actionViewName ) );
            }
        }

        /// <summary>
        /// 自定义当前视图模的内容(自定义内容之后，默认的模板将被忽略)
        /// </summary>
        /// <param name="templateContent">模板的内容</param>
        public void viewContent( String templateContent ) {
            Template t = new Template();
            t.InitContent( templateContent );
            utils.setCurrentView( t );
        }

        /// <summary>
        /// 给模板中的变量赋值
        /// </summary>
        /// <param name="lbl">变量名称</param>
        /// <param name="val"></param>
        public void set( String lbl, Object val ) {
            utils.getCurrentView().Set( lbl, val );
        }

        /// <summary>
        /// 给模板中的变量赋值
        /// </summary>
        /// <param name="lbl">变量名称</param>
        /// <param name="val"></param>
        public void set( String lbl, String val ) {
            utils.getCurrentView().Set( lbl, val );
        }

        /// <summary>
        /// 将对象绑定到模板
        /// </summary>
        /// <param name="val"></param>
        protected void bind( Object val ) {
            utils.getCurrentView().Bind( val );
        }

        /// <summary>
        /// 将对象绑定到模板，并指定对象在模板中的变量名
        /// </summary>
        /// <param name="lbl">对象在模板中的变量名</param>
        /// <param name="val"></param>
        protected void bind( String lbl, Object val ) {
            utils.getCurrentView().Bind( lbl, val );
        }

        /// <summary>
        /// 将对象列表绑定到模板
        /// </summary>
        /// <param name="listName">需要被绑定的列表名</param>
        /// <param name="lbl">对象在模板中的变量名</param>
        /// <param name="objList">对象的列表</param>
        protected void bindList( String listName, String lbl, IList objList ) {
            bindList( listName, lbl, objList, bindOtherLink );
        }

        /// <summary>
        /// 将对象列表绑定到模板
        /// </summary>
        /// <param name="listName">需要被绑定的列表名</param>
        /// <param name="lbl">对象在模板中的变量名</param>
        /// <param name="objList">对象的列表</param>
        /// <param name="otherBinder">附加的绑定器otherBindFunction( IBlock tpl, String lbl, Object obj )</param>
        protected void bindList( String listName, String lbl, IList objList, otherBindFunction otherBinder ) {
            utils.getCurrentView().bindOtherFunc = otherBinder;
            utils.getCurrentView().BindList( listName, lbl, objList );
        }

        /// <summary>
        /// 将对象列表绑定到模板
        /// </summary>
        /// <param name="listName">需要被绑定的列表名</param>
        /// <param name="lbl">对象在模板中的变量名</param>
        /// <param name="objList">对象的列表</param>
        /// <param name="otherBinder">附加的绑定器bindFunction( IBlock tpl, int id )</param>
        protected void bindList( String listName, String lbl, IList objList, bindFunction otherBinder ) {
            utils.getCurrentView().bindFunc = otherBinder;
            utils.getCurrentView().BindList( listName, lbl, objList );
        }

        /// <summary>
        /// 设置模板中表单提交的target
        /// </summary>
        /// <param name="url"></param>
        protected void target( String url ) {
            set( "ActionLink", url );
        }

        /// <summary>
        /// 设置模板中表单提交的target
        /// </summary>
        /// <param name="action"></param>
        protected void target( aAction action ) {
            set( "ActionLink", to( action ) );
        }

        /// <summary>
        /// 设置模板中表单提交的target
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        protected void target( aActionWithId action, int id ) {
            set( "ActionLink", to( action, id ) );
        }

        //-------------------------------------- 控件 ----------------------------------------------

        /// <summary>
        /// 编辑器，工具栏只包括基本按钮
        /// </summary>
        /// <param name="propertyName">属性名称(也是编辑器名称)</param>
        /// <param name="propertyValue">需要被编辑的内容</param>
        /// <param name="height">编辑器高度(必须手动指定px单位)</param>
        protected void editor( String propertyName, String propertyValue, String height ) {

            if (ctx.route.isAdmin) {
                editorFull( propertyName, propertyValue, height );
                return;
            }

            IEditor ed = EditorFactory.NewOne( propertyName, propertyValue, height, Editor.ToolbarType.Basic );
            ed.AddUploadUrl( ctx );
            set( "Editor", ed );
        }

        /// <summary>
        /// 编辑器，包括全部的工具栏
        /// </summary>
        /// <param name="propertyName">属性名称(也是编辑器名称)</param>
        /// <param name="propertyValue">需要被编辑的内容</param>
        /// <param name="height">编辑器高度(必须手动指定px单位)</param>
        protected void editorFull( String propertyName, String propertyValue, String height ) {

            IEditor ed = EditorFactory.NewOne( propertyName, propertyValue, height, Editor.ToolbarType.Full );
            ed.AddUploadUrl( ctx );
            set( "Editor", ed );
        }

        /// <summary>
        /// 编辑器，工具栏只包括基本按钮
        /// </summary>
        /// <param name="varName">模板中的变量名称</param>
        /// <param name="propertyName">需要编辑的属性名称</param>
        /// <param name="propertyValue">需要编辑的内容</param>
        /// <param name="height">编辑器高度(必须手动指定px单位)</param>
        protected void editor( String varName, String propertyName, String propertyValue, String height ) {
            editor( varName, propertyName, propertyValue, height, Editor.ToolbarType.Basic );
        }

        /// <summary>
        /// 编辑器
        /// </summary>
        /// <param name="varName">模板中的变量名称</param>
        /// <param name="propertyName">需要编辑的属性名称</param>
        /// <param name="propertyValue">需要编辑的内容</param>
        /// <param name="height">编辑器高度(必须手动指定px单位)</param>
        /// <param name="toolbar">工具栏类型：基本按钮或全部按钮</param>
        protected void editor( String varName, String propertyName, String propertyValue, String height, Editor.ToolbarType toolbar ) {

            String currentEditorKey = "currentEditor";

            List<String> editorList = ctx.GetItem( currentEditorKey ) as List<String>;
            if (editorList != null && editorList.Contains( propertyName )) throw new Exception( lang( "exEditorNameUnique" ) );

            IEditor ed = EditorFactory.NewOne( propertyName, propertyValue, height, toolbar );
            ed.AddUploadUrl( ctx );
            if (editorList != null && editorList.Count > 0) ed.IsUnique = false;
            set( varName, ed );

            if (editorList == null) editorList = new List<string>();
            editorList.Add( propertyName );
            ctx.SetItem( currentEditorKey, editorList );
        }


        /// <summary>
        /// 下拉控件(用数组填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="options">填充下拉框的字符数组</param>
        /// <param name="val">选定的值</param>
        protected void dropList( String varName, String[] options, Object val ) {
            set( varName, Html.DropList( options, varName, val ) );
        }

        /// <summary>
        /// 下拉控件(用 Dictionary 填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="dic">填充下拉框的Dictionary</param>
        /// <param name="val">选定的值</param>
        protected void dropList( string varName, Dictionary<string, string> dic, string val ) {
            set( varName, Html.DropList( dic, varName, val ) );
        }

        /// <summary>
        /// 下拉控件(用对象列表填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="list">填充下拉框的对象列表</param>
        /// <param name="nameValuePair">名值对，比如"Name=Id"表示用对象的属性Name填充选项的文本，用对象的属性Id填充选项的值</param>
        /// <param name="val">选定的值</param>
        protected void dropList( String varName, IList list, String nameValuePair, Object val ) {

            if (list == null || list.Count == 0) {
                set( varName, "" );
                return;
            }

            String[] arr = nameValuePair.Split( '=' );
            String dropString = Html.DropList( list, varName, arr[0], arr[1], val );
            set( varName, dropString );
        }

        /// <summary>
        /// 多个单选的列表(用字符数组填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="options">填充列表的字符数组</param>
        /// <param name="val">选定的值</param>
        protected void radioList( String varName, String[] options, Object val ) {
            set( varName, Html.RadioList( options, varName, val ) );
        }

        /// <summary>
        /// 多个单选的列表(用 Dictionary 填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="dic">填充列表的 Dictionary</param>
        /// <param name="val">选定的值</param>
        protected void radioList( string varName, Dictionary<string, string> dic, string val ) {
            set( varName, Html.RadioList( dic, varName, val ) );
        }

        /// <summary>
        /// 多个单选的列表(用对象列表填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="list">填充单选列表的对象列表</param>
        /// <param name="nameValuePair">名值对，比如"Name=Id"表示用对象的属性Name填充选项的文本，用对象的属性Id填充选项的值</param>
        /// <param name="val">选定的值</param>
        protected void radioList( String varName, IList list, String nameValuePair, Object val ) {
            String[] arr = nameValuePair.Split( '=' );
            set( varName, Html.RadioList( list, varName, arr[0], arr[1], val ) );
        }

        /// <summary>
        /// 多选框(用数组填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="options">填充列表的字符数组</param>
        /// <param name="val">选定的值，多个选值之间用英文逗号分开，比如 "2, 6, 13"</param>
        protected void checkboxList( String varName, String[] options, Object val ) {
            set( varName, Html.CheckBoxList( options, varName, val ) );
        }

        /// <summary>
        /// 多选框(用 Dictionary 填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="dic">填充列表的 Dictionary</param>
        /// <param name="val">选定的值，多个选值之间用英文逗号分开，比如 "2, 6, 13"</param>
        protected void checkboxList( string varName, Dictionary<string, string> dic, string val ) {
            set( varName, Html.CheckBoxList( dic, varName, val ) );
        }

        /// <summary>
        /// 多选框(用对象列表填充)
        /// </summary>
        /// <param name="varName">控件名称</param>
        /// <param name="list">填充多选列表的对象列表</param>
        /// <param name="nameValuePair">名值对，比如"Name=Id"表示用对象的属性Name填充选项的文本，用对象的属性Id填充选项的值</param>
        /// <param name="val">选定的值，多个选值之间用英文逗号分开，比如 "2, 6, 13"</param>
        protected void checkboxList( String varName, IList list, String nameValuePair, Object val ) {
            String[] arr = nameValuePair.Split( '=' );
            set( varName, Html.CheckBoxList( list, varName, arr[0], arr[1], val ) );
        }

        //-------------------------------------- 内部链接(快捷方式) ----------------------------------------------

        /// <summary>
        /// 链接到某个 action
        /// </summary>
        /// <param name="action"></param>
        /// <returns>返回一个链接</returns>
        public String to( aAction action ) {
            return ctx.link.To( action );
        }

        /// <summary>
        /// 链接到某个 action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns>返回一个链接</returns>
        public String to( aActionWithId action, int id ) {
            return ctx.link.To( action, id );
        }

        /// <summary>
        /// 链接到某个 action，链接中不包含 appId 信息
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public String t2( aAction action ) {
            return ctx.link.T2( action );
        }

        /// <summary>
        /// 链接到某个 action，链接中不包含 appId 信息
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public String t2( aActionWithId action, int id ) {
            return ctx.link.T2( action, id );
        }

        public String toUser( IMember member ) {
            return Link.ToMember( member );
        }

        public String toUser( String friendlyUrl ) {
            return Link.ToUser( friendlyUrl );
        }

        /// <summary>
        /// 设置当前 action 返回的内容（一旦设置，先前绑定的模板内容将被覆盖）
        /// </summary>
        /// <param name="content"></param>
        public void content( String content ) {
            utils.setActionContent( content );
        }

        /// <summary>
        /// 设置当前 action 返回的内容（一旦设置，先前绑定的模板内容将被覆盖）
        /// </summary>
        /// <param name="content"></param>
        public void actionContent( String content ) {
            utils.setActionContent( content );
        }

        //-------------------------------------- 显示消息 ----------------------------------------------

        /// <summary>
        /// 根据模板显示提示信息。ajax情况不使用模板，只显示内容。
        /// </summary>
        /// <param name="msg"></param>
        public void echo( String msg ) {
            if (ctx.utils.isAjax) {
                echoText( msg );
            }
            else if (isFrame()) {
                ctx.utils.endMsgBox( msg );
            }
            else {
                ctx.utils.endMsgByView( msg );
            }
        }

        /// <summary>
        /// 直接显示内容(不根据模板)，然后结束
        /// </summary>
        /// <param name="msg"></param>
        public void echoText( String msg ) {
            ctx.utils.endMsgByText( msg );
        }

        /// <summary>
        /// 将对象序列化，然后输出到客户端(ContentType="application/json")，不再输出布局页面
        /// </summary>
        /// <param name="msg"></param>
        protected void echoJson( Object obj ) {
            setJsonContentType();
            echoText( Json.ToString( obj ) );
        }

        /// <summary>
        /// 将json字符串直接输出到客户端(ContentType="application/json")，不再输出布局页面
        /// </summary>
        /// <param name="msg"></param>
        protected void echoJson( String jsonString ) {
            setJsonContentType();
            echoText( jsonString );
        }

        /// <summary>
        /// 将xml直接输出到客户端(ContentType="text/xml")
        /// </summary>
        /// <param name="xml"></param>
        protected void echoXml( String xml ) {
            setXmlContentType();
            echoText( xml );
        }

        /// <summary>
        /// 将字符串 ok 显示到客户端
        /// </summary>
        protected void echoAjaxOk() {
            echoText( "ok" );
        }


        /// <summary>
        /// 将操作结果返回到客户端
        /// </summary>
        /// <param name="result"></param>
        protected void echoResult( Result result ) {
            echoResult( result, null );
        }

        /// <summary>
        /// 将操作结果返回到客户端
        /// </summary>
        /// <param name="result"></param>
        /// <param name="okMsg">成功的提示信息(可为空)</param>
        protected void echoResult( Result result, String okMsg ) {

            if (result == null) result = new Result();

            if (result.HasErrors) {
                echoError( result );
            }
            else {
                if (ctx.utils.isAjax) {
                    echoAjaxOk();
                }
                else {
                    if (strUtil.HasText( okMsg )) {
                        echoRedirectPart( okMsg );
                    }
                    else {
                        echoRedirectPart( lang( "opok" ) );
                    }
                }
            }
        }

        /// <summary>
        /// 显示错误信息并跳转到默认页。如果是ajax，输出错误信息的json格式。
        /// </summary>
        protected void echoError() {
            if (ctx.utils.isAjax) {
                echoJson( errors.ErrorsJson );
            }
            else {
                echoRedirect( errors.ErrorsHtml );
            }
        }

        /// <summary>
        /// 显示错误信息。如果是ajax，输出错误信息的json格式。
        /// </summary>
        /// <param name="msg"></param>
        protected void echoError( String msg ) {
            errors.Add( msg );
            echoError();
        }

        /// <summary>
        /// 显示错误信息。如果是ajax，输出错误信息的json格式。
        /// </summary>
        /// <param name="result"></param>
        protected void echoError( Result result ) {
            errors.Join( result );
            echoError();
        }

        /// <summary>
        /// 显示 json 信息给客户端，提示是否 valid ，返回 {"IsValid":true, "Msg":"", "Info":"这里是字符串"}
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isValid"></param>
        /// <param name="otherInfo"></param>
        protected void echoJsonMsg( String msg, Boolean isValid, String otherInfo ) {
            echoJson( MvcUtil.renderValidatorJson( msg, isValid, otherInfo ) );
        }

        /// <summary>
        /// 显示 json 信息给客户端，提示是否 valid ，返回 {"IsValid":true, "Msg":"", "Info":{这里是具体的json对象信息}}
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isValid"></param>
        /// <param name="otherObject"></param>
        protected void echoJsonMsg( String msg, Boolean isValid, Object otherObject ) {
            echoJson( MvcUtil.renderValidatorJsonObject( msg, isValid, otherObject ) );
        }

        /// <summary>
        /// 将 json 信息 {Msg:"ok", IsValid:true, Info:""} 显示给客户端
        /// </summary>
        protected void echoJsonOk() {
            echoJsonMsg( "ok", true, "" );
        }

        private void setJsonContentType() {
            ctx.web.ResponseContentType( "application/json" );
        }

        private void setXmlContentType() {
            ctx.web.ResponseContentType( "text/xml" );
        }


        //----------------------------- 显示信息然后跳转 -----------------------------------

        /// <summary>
        /// 先显示提示信息(echo)，然后跳转页面(redirect)。支持ajax。
        /// </summary>
        /// <param name="msg"></param>
        public void echoRedirect( String msg ) {
            // ajaxPostForm状态下，不跳转页面
            String returnUrl = ctx.utils.isAjax ? "" : getReturnUrl();

            returnUrl = clearLayoutInfo( returnUrl );

            echoRedirect( msg, returnUrl );
        }

        /// <summary>
        /// 显示信息，然后跳转到指定的action
        /// </summary>
        /// <param name="msg">显示的信息</param>
        /// <param name="action">跳转的目标action</param>
        public void echoRedirect( String msg, aAction action ) {
            echoRedirectPart( msg, to( action ), 9999 );
        }

        /// <summary>
        /// 显示信息，然后跳转到指定的url
        /// </summary>
        /// <param name="msg">显示的信息</param>
        /// <param name="url">跳转的目标网址</param>
        public void echoRedirect( String msg, String url ) {
            echoRedirectPart( msg, url, 9999 );
        }

        public void echoRedirectPart( String msg ) {
            String returnUrl = ctx.utils.isAjax ? "" : getReturnUrl();
            echoRedirectPart( msg, returnUrl );
        }

        /// <summary>
        /// 仅在当前局部中刷新
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        public void echoRedirectPart( String msg, String url ) {
            echoRedirectPart( msg, url, 0 );
        }

        /// <summary>
        /// 在指定范围的局部中刷新，从当前局部算起，倒着计数的layout中刷新：0、1、2、3、4
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        /// <param name="partNumber"></param>
        public void echoRedirectPart( String msg, String url, int partNumber ) {

            if (ctx.utils.isAjax) {

                String returnJson = "{\"IsValid\":true, \"IsParent\":true, \"PartNumber\":" + partNumber + ", \"Msg\":\"" + strUtil.Text2Html( msg ).Replace( "\"", "'" ) + "\", \"ForwardUrl\":\"" + url + "\"}";

                echoJson( returnJson );
            }
            else if (ctx.utils.isBox() || ctx.utils.isFrame()) {
                url = addFrameParams( url );
                forward( msg, url );
            }
            else {
                String str = "<script>_run( function() {wojilu.tool.forwardPart('" + url + "', " + partNumber + ");});</script><div class=\"forward\" id=\"forward\">" + msg + "</div>";
                ctx.utils.endMsgForward( str );
            }
        }

        private string clearLayoutInfo( string returnUrl ) {

            String[] arr = returnUrl.Split( '?' );
            if (arr.Length != 2) return returnUrl;

            String[] qItems = arr[1].Split( '&' );
            String url = arr[0] + "?";
            foreach (String item in qItems) {
                String[] arrItem = item.Split( '=' );
                if (isPartUrl( arrItem[0] )) continue;
                url += item + "&";
            }

            return url.TrimEnd( '&' ).TrimEnd( '?' );
        }

        private Boolean isPartUrl( String item ) {
            return item.Equals( "nolayout" ) || item.Equals( "frm" ) || item.Equals( "rd" );
        }

        // 表单下
        private String addFrameParamsWhenForm( String url ) {

            if (ctx.Post( "frm" ) != null && ctx.Post( "frm" ) == "true") {
                return addFrameParams( url );
            }

            return url;
        }


        /// <summary>
        /// (本方法不常用)将一段 html 字符串添加到父窗口的某个 elementID，一般客户端配合 ajaxUpdateForm 使用
        /// </summary>
        /// <param name="elementID"></param>
        /// <param name="htmlValue"></param>
        protected void echoHtmlTo( String elementID, String htmlValue ) {
            echoJsonMsg( htmlValue, true, elementID );
        }

        /// <summary>
        /// (用于弹窗中)显示提示信息，然后关闭弹窗，并刷新父页面
        /// </summary>
        /// <param name="msg"></param>
        protected void echoToParent( String msg ) {


            if (ctx.utils.isAjax) {

                String returnJson = "{\"IsValid\":true, \"Msg\":\"" + strUtil.Text2Html( msg ).Replace( "\"", "'" ) + "\", \"ForwardUrl\":\"\", \"IsParent\":true}";
                echoJson( returnJson );
                return;
            }

            String cmd = "wojilu.tool.getRootParent().wojilu.tool.reloadPage();";
            echoMsgAndJs( cmd, 500, msg );
        }

        protected void echoToParentPart( String msg ) {
            echoToParentPart( msg, "", 0 );
        }

        protected void echoToParentPart( String msg, String url ) {
            echoToParentPart( msg, url, 0 );
        }

        protected void echoToParentPart( String msg, String url, int partNumber ) {

            if (ctx.utils.isAjax) {

                String returnJson = "{\"IsValid\":true, \"Msg\":\"" + strUtil.Text2Html( msg ).Replace( "\"", "'" ) + "\", \"ForwardUrl\":\"" + url + "\", \"IsParent\":true}";
                echoJson( returnJson );
                return;
            }

            String cmd = "wojilu.tool.getBoxParent().wojilu.tool.forwardPart('" + url + "', " + partNumber + ");";

            echoMsgAndJs( cmd, 500, msg );

        }

        /// <summary>
        /// (用于弹窗中)显示提示信息，然后关闭弹窗，并让父页面跳转到指定url
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        protected void echoToParent( String msg, String url ) {
            echoToParentPart( msg, url, 9999 );
        }

        private void forward( String msg, String url ) {
            String str = "<script>_run( function() {wojilu.tool.forward('" + url + "');});</script><div class=\"forward\" id=\"forward\">" + msg + "</div>";
            ctx.utils.endMsgForward( str );
        }

        private void forwardPage( String msg, String url ) {
            String str = "<script>_run( function() {wojilu.tool.forwardPage('" + url + "');});</script><div class=\"forward\" id=\"forward\">" + msg + "</div>";
            ctx.utils.endMsgForward( str );
        }

        //-------------------------------------- 直接跳转 ----------------------------------------------

        /// <summary>
        /// 自动跳转页面到来时的 url
        /// </summary>
        protected void redirect() {
            redirectUrl( getReturnUrl() );
        }

        /// <summary>
        /// 跳转页面到指定 action
        /// </summary>
        /// <param name="action"></param>
        public void redirect( aAction action ) {
            redirectUrl( ctx.link.To( action ) );
        }

        /// <summary>
        /// 跳转页面到指定 action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        public void redirect( aActionWithId action, int id ) {
            redirectUrl( ctx.link.To( action, id ) );
        }

        /// <summary>
        /// 跳转页面到指定 action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        public void redirect( String action, int id ) {
            redirectUrl( Link.To( ctx.owner.obj, LinkHelper.GetControllerName( base.GetType() ), action, id ) );
        }

        /// <summary>
        /// 跳转页面到指定 url
        /// </summary>
        /// <param name="url"></param>
        public void redirectUrl( String url ) {

            if (ctx.utils.isAjax) {

                url = addFrameParamsWhenForm( url );

                String returnJson = "{\"IsValid\":true, \"ForwardUrl\":\"" + url + "\", \"Time\":0, \"PartNumber\":0}";
                echoJson( returnJson );
            }
            else if (isFrame()) {

                ctx.utils.end();
                ctx.utils.skipRender();
                url = addFrameParams( url );
                ctx.utils.clearResource();
                ctx.web.Redirect( url, false );
            }
            else if (hasNolayout()) {
                ctx.utils.end();
                ctx.utils.skipRender();
                url = addLayoutParams( url );
                ctx.utils.clearResource();
                ctx.web.Redirect( url, false );
            }
            else {
                redirectDirect( url );
            }
        }

        /// <summary>
        /// 直接跳转，不经过layout参数处理
        /// </summary>
        /// <param name="url"></param>
        public void redirectDirect( String url ) {
            ctx.utils.end();
            ctx.utils.skipRender();
            ctx.utils.clearResource();
            ctx.web.Redirect( url, false );
        }

        private Boolean hasNolayout() {
            return ctx.utils.getNoLayout() > 0 || referrerHasNolayout();
        }

        private Boolean referrerHasNolayout() {
            if (ctx.web.PathReferrer == null) return false;
            return ctx.web.PathReferrer.IndexOf( "nolayout" ) > 0;
        }

        private Boolean isFrame() {
            return ctx.utils.isFrame();
        }

        /// <summary>
        /// 向客户端返回没有权限的信息(401 Unauthorized)，同时跳转到登录页面
        /// </summary>
        public void redirectLogin() {

            if (ctx.utils.isAjax) {
                ctx.utils.endMsg( lang( "exPlsLogin" ), null );
            }
            else {
                ctx.utils.end();
                ctx.utils.skipRender();
                ctx.utils.clearResource();
                ctx.web.ResponseStatus( HttpStatus.Unauthorized_401 );
            }
        }

        // 如果 url 中已经有 frm=true了，则直接返回
        private String addFrameParams( String url ) {

            String frmStr = "frm=true";
            if (url.IndexOf( frmStr ) > 0) return url;

            return appendParam( url, frmStr );
        }

        private string addLayoutParams( string url ) {

            if (url.IndexOf( "nolayout" ) > 0) return url;

            int nolayout = ctx.utils.getNoLayout();
            if (nolayout <= 0) nolayout = getReferrerNolayout();

            String param = "nolayout=" + nolayout;

            return appendParam( url, param );
        }

        private int getReferrerNolayout() {

            String rUrl = ctx.web.PathReferrer;
            String[] arrItems = rUrl.Split( '?' );
            if (arrItems.Length != 2) return 0;

            String[] queryItems = arrItems[1].Split( '&' );
            foreach (String item in queryItems) {
                if (item.StartsWith( "nolayout=" )) {
                    return cvt.ToInt( strUtil.TrimStart( item, "nolayout=" ) );
                }
            }
            return 0;
        }

        private String appendParam( String url, String param ) {
            if (url.IndexOf( '?' ) < 0) return url + "?" + param;
            if (url.EndsWith( "?" )) return url + param;
            if (url.EndsWith( "&" )) return url + param;
            return url + "&" + param;
        }

        //-------------------------------------- ajax返回值 ----------------------------------------------

        private void echoMsgAndJs( String cmd, int msTime, String msg ) {
            String tcall = "setTimeout( function(){" + cmd + ";wojilu.tool.getBoxParent().wojilu.ui.box.hideBox();}, " + msTime + " );";

            String content = "_run( function() { " + tcall + " });";
            content = String.Format( "<script>{0}</script>", content ) + msg;
            ctx.utils.endMsgBox( content );
        }

        private String getReturnUrl() {

            if (strUtil.HasText( ctx.web.PathReferrer ))
                return ctx.web.PathReferrer;

            return ctx.url.AppPath;
        }

        //-------------------------------------- 绑定(加载)局部页面内容 ----------------------------------------------

        /// <summary>
        /// 将某 action 的内容加载到指定位置
        /// </summary>
        /// <param name="sectionName">需要加载内容的位置</param>
        /// <param name="action">被加载的 action</param>
        protected void load( String sectionName, aAction action ) {
            set( sectionName, loadHtml( action ) );
        }

        /// <summary>
        /// 将某 action 的内容加载到指定位置
        /// </summary>
        /// <param name="sectionName">需要加载内容的位置</param>
        /// <param name="action">被加载的 action</param>
        protected void load( String sectionName, aActionWithId action, int id ) {
            set( sectionName, loadHtml( action, id ) );
        }

        /// <summary>
        /// 获取某 action 的内容
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public String loadHtml( String controller, String action ) {
            return ControllerRunner.Run( ctx, controller, action );
        }

        /// <summary>
        /// 获取某 action 的内容
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public String loadHtml( String controller, String action, int id ) {
            return ControllerRunner.Run( ctx, controller, action, id );
        }

        // TODO 在被load的action中，使用showEnd无效
        /// <summary>
        /// 获取某 action 的内容
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public String loadHtml( aAction action ) {

            String result;

            if (isSameType( action.Method )) {

                String actionName = action.Method.Name;
                Template originalView = utils.getCurrentView();

                setView( action.Method );
                action();

                Template resultView = utils.getCurrentView();
                utils.setCurrentView( originalView );
                result = resultView.ToString();
            }
            else {

                String actionName = action.Method.Name;
                ControllerBase otherController = ControllerFactory.FindController( action.Method.DeclaringType, ctx );
                otherController.view( actionName );
                otherController.utils.runAction( actionName );
                result = otherController.utils.getActionResult();

            }

            return result;
        }

        /// <summary>
        /// 获取某 action 的内容
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public String loadHtml( aActionWithId action, int id ) {

            String result;

            if (isSameType( action.Method )) {

                String actionName = action.Method.Name;
                Template originalView = utils.getCurrentView();

                setView( action.Method );

                action( id );

                Template resultView = utils.getCurrentView();
                utils.setCurrentView( originalView );
                result = resultView.ToString();
            }
            else {

                //ControllerBase targetController = action.Target as ControllerBase;
                //ControllerFactory.InjectController( targetController, ctx );
                //targetController.view( action.Method.Name );
                //action( id );
                //result = targetController.utils.getCurrentView().ToString();

                result = ControllerRunner.Run( ctx, action, id );
            }

            return result;
        }

        /// <summary>
        /// 运行其他 action，并将运行结果作为当前 action 的内容
        /// </summary>
        /// <param name="action"></param>
        protected void run( aAction action ) {

            if (ctx.utils.isAjax) {
                if (ctx.HasErrors)
                    echoError();
                else
                    echoAjaxOk();
                return;
            }

            if (isSameType( action.Method )) {
                setView( action.Method );
                action();
            }
            else {

                //ControllerBase mycontroller = ControllerFactory.FindController( action.Method.DeclaringType, ctx );
                //mycontroller.view( action.Method.Name );
                //action.Method.Invoke( mycontroller, null );
                //actionContent( mycontroller.utils.getActionResult() );

                content( ControllerRunner.Run( ctx, action ) );
            }
        }

        /// <summary>
        /// 运行其他 action，并将运行结果作为当前 action 的内容
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        protected void run( aActionWithId action, int id ) {

            if (ctx.utils.isAjax) {
                if (ctx.HasErrors)
                    echoError();
                else
                    echoAjaxOk();
                return;
            }

            if (isSameType( action.Method )) {
                setView( action.Method );
                action( id );
            }
            else {

                //ControllerBase mycontroller = ControllerFactory.FindController( action.Method.DeclaringType, ctx );
                //mycontroller.view( action.Method.Name );
                //action.Method.Invoke( mycontroller, new object[] { id } );
                //actionContent( mycontroller.utils.getActionResult() );

                content( ControllerRunner.Run( ctx, action, id ) );
            }
        }



        /// <summary>
        /// 运行其他 action，并将运行结果作为当前 action 的内容
        /// </summary>
        /// <param name="controllerType">被运行的 action 所属的 controller 类型</param>
        /// <param name="actionName">action 名称</param>
        /// <param name="args">需要的参数</param>
        protected void run( String controllerFullTypeName, String actionName, params object[] args ) {

            Type controllerType = ObjectContext.Instance.TypeList[controllerFullTypeName];

            if (controllerType == base.GetType()) {

                view( actionName );

                MethodInfo method = ActionRunner.getActionMethod( this, actionName );
                if (method == null) {
                    throw new Exception( "action " + wojilu.lang.get( "exNotFound" ) );
                }
                else {
                    method.Invoke( this, args );
                }

            }
            else {
                content( ControllerRunner.Run( ctx, controllerFullTypeName, actionName, args ) );
            }
        }

        private Boolean isSameType( MethodInfo method ) {
            Boolean result = this.GetType() == method.DeclaringType || this.GetType().IsSubclassOf( method.DeclaringType );
            return result;
        }

        private void setView( MethodInfo method ) {
            if (this.GetType().IsSubclassOf( method.DeclaringType )) {
                String filePath = MvcUtil.getParentViewPath( method, ctx.route.getRootNamespace( method.DeclaringType.FullName ) );
                this.utils.setCurrentView( this.utils.getTemplateByFileName( filePath ) );
            }
            else {
                view( method.Name );
            }
        }


        //------------------------------------------------------------------------

        /// <summary>
        /// 从核心语言包(core.config)中获取多国语言的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected String lang( String key ) {
            return wojilu.lang.get( key );
        }

        /// <summary>
        /// 从各 app 的语言包中获取多国语言的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String alang( String key ) {
            return utils.getAppLang() == null ? null : utils.getAppLang().get( key );
        }

    }
}
