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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Utils;
using wojilu.Web.Mock;

namespace wojilu.Web.Context {

    /// <summary>
    /// mvc 上下文数据：即整个执行流程中常用的数据封装
    /// </summary>
    public class MvcContext {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MvcContext ) );

        private IWebContext _context;
        private MvcContextUtils _thisUtils;
        private Boolean _isMock = false;

        public MvcContext( IWebContext context ) {
            _context = context;
            _thisUtils = new MvcContextUtils( this );

            if (context is IMockContext) _isMock = true;

            _pageMeta = new PageMeta( this );
        }

        /// <summary>
        /// 高级工具方法MvcContextUtils
        /// </summary>
        public MvcContextUtils utils {
            get {
                if (_thisUtils != null) return _thisUtils;
                _thisUtils = new MvcContextUtils( this );
                return _thisUtils;
            }
        }

        public Boolean IsMock {
            get { return _isMock; }
        }

        /// <summary>
        /// web 原始数据和方法封装
        /// </summary>
        public IWebContext web { get { return _context; } }

        private Result _errors = new Result();

        /// <summary>
        /// 获取当前ctx中的错误信息
        /// </summary>
        public Result errors { get { return _errors; } }

        /// <summary>
        /// 当前路由信息
        /// </summary>
        public Route route { get { return utils.getRoute(); } }

        /// <summary>
        /// 当前控制器
        /// </summary>
        public ControllerBase controller { get { return utils.getController(); } }

        /// <summary>
        /// 当前 owner(被访问的对象信息)
        /// </summary>
        public IOwnerContext owner { get { return utils.getOwnerContext(); } }

        /// <summary>
        /// 访问者的信息
        /// </summary>
        public IViewerContext viewer { get { return utils.getViewerContext(); } }

        /// <summary>
        /// 当前 app
        /// </summary>
        public IAppContext app { get { return utils.getAppContext(); } }

        private PageMeta _pageMeta;

        /// <summary>
        /// 页面元信息(包括Title/Keywords/Description/RssLink)
        /// </summary>
        /// <returns></returns>
        public PageMeta Page {
            get { return _pageMeta; }
        }


        //---------------------------------------------------------

        private Hashtable _contextItems = new Hashtable();

        /// <summary>
        /// 根据 key 获取存储在 ctx 中某项的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetItem( String key ) {
            return _contextItems[key];
        }

        /// <summary>
        /// 根据 key 获取存储在 ctx 中某项的值，以字符串形式返回
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetItemString( String key ) {
            if (_contextItems[key] == null) return null;
            return _contextItems[key].ToString();
        }

        /// <summary>
        /// 将某个对象存储在 ctx 中，方便不同的 controller 或 action 之间调用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetItem( String key, Object val ) {
            _contextItems[key] = val;
        }

        /// <summary>
        /// 判断 ctx 的存储器中是否具有某个 key 。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean HasItem( string key ) {
            return _contextItems.ContainsKey( key );
        }

        //---------------------------------------------------------


        private UrlInfo _url;

        /// <summary>
        /// 获取经过封装的 url 信息
        /// </summary>
        public UrlInfo url { get { return getUrl(); } }

        private UrlInfo getUrl() {

            if (_url == null) {
                _url = new UrlInfo( _context.Url, _context.PathApplication, _context.PathInfo );
            }
            return _url;
        }

        /// <summary>
        /// 设置当前网址，用于自定义网址
        /// </summary>
        /// <param name="url"></param>
        public void setUrl( String url ) {
            _url = new UrlInfo( url, _context.PathApplication, _context.PathInfo );
        }

        /// <summary>
        /// 当前客户端上传的所有文件
        /// </summary>
        /// <returns></returns>
        public List<HttpFile> GetFiles() {
            HttpFileCollection files = _context.getUploadFiles();

            List<HttpFile> list = new List<HttpFile>();
            for (int i = 0; i < files.Count; i++) {
                list.Add( new HttpFile( files[i] ) );
            }
            return list;
        }

        /// <summary>
        /// 当前客户端上传的第一个文件
        /// </summary>
        /// <returns></returns>
        public HttpFile GetFileSingle() {
            return this.GetFiles().Count == 0 ? null : GetFiles()[0];
        }

        /// <summary>
        /// 客户端是否上传了文件
        /// </summary>
        public Boolean HasUploadFiles {
            get { return GetFiles().Count > 0 && GetFileSingle().ContentLength > 10; }
        }

        /// <summary>
        /// 当前 ctx 中是否有错误信息
        /// </summary>
        public Boolean HasErrors {
            get { return errors.HasErrors; }
        }

        /// <summary>
        /// 客户端提交的 HttpMethod，返回GET/POST/DELETE/PUT 等
        /// </summary>
        public String HttpMethod { get { return getMethod(); } }

        /// <summary>
        /// 当前客户端提交方法是否是 GET 方法
        /// </summary>
        public Boolean IsGetMethod {
            get { return strUtil.EqualsIgnoreCase( "get", this.HttpMethod ); }
        }

        private String getMethod() {

            if ("POST".Equals( _context.post( "_httpmethod" ) )) return "POST";
            if ("DELETE".Equals( _context.post( "_httpmethod" ) )) return "DELETE";
            if ("PUT".Equals( _context.post( "_httpmethod" ) )) return "PUT";

            return _context.ClientHttpMethod;
        }

        private MethodInfo _actionMethodInfo;

        internal void setActionMethodInfo( MethodInfo mi ) {
            _actionMethodInfo = mi;
        }
        public MethodInfo ActionMethodInfo {
            get {
                if (_actionMethodInfo == null) {
                }
                return _actionMethodInfo;
            }
        }


        private List<Attribute> _attributes;
        public List<Attribute> ActionMethods {
            get {
                if (_attributes == null) {
                    Object[] attrs = this.controller.utils.getAttributesAll( this.ActionMethodInfo );
                    _attributes = new List<Attribute>();
                    foreach (Object obj in attrs) {
                        _attributes.Add( (Attribute)obj );
                    }
                }
                return _attributes;
            }
        }

        /// <summary>
        /// 清理所有资源，准备抛出异常
        /// </summary>
        /// <param name="httpStatus">给客户端的 httpStatus 状态信息</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public MvcException ex( String httpStatus, String msg ) {
            utils.clearResource();
            return new MvcException( httpStatus, msg );
        }

        /// <summary>
        /// 呈现 json 到客户端
        /// </summary>
        /// <param name="jsonContent"></param>
        public void RenderJson( String jsonContent ) {
            _context.RenderJson( jsonContent );
        }

        /// <summary>
        /// 呈现 xml 到客户端
        /// </summary>
        /// <param name="xmlContent"></param>
        public void RenderXml( String xmlContent ) {
            _context.RenderXml( xmlContent );
        }


        //---------------------------------------- Get ---------------------------------------------

        /// <summary>
        /// 获取 url 中的某项值，结果已被过滤(不允许html)
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        public String Get( String queryItem ) {
            String val = _context.get( queryItem );
            return checkClientValue( val );
        }

        /// <summary>
        /// 获取当前路由中的 id 对应的数据(必须先通过 DataAttribute 指定数据类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() {
            return (T)this.GetItem( "__currentRouteIdData" );
        }

        /// <summary>
        /// 检查 url 中是否具有某项 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean GetHas( String key ) {
            return _context.getHas( key );
        }

        /// <summary>
        /// 从 url 的查询信息 (query string) 中获取 id 列表，结果经过了验证，是类型安全的。如果不合法，则返回null
        /// </summary>
        /// <param name="idname"></param>
        /// <returns></returns>
        public String GetIdList( String idname ) {
            String ids = Get( idname );
            if (!cvt.IsIdListValid( ids )) {
                return null;
            }
            return ids;
        }

        /// <summary>
        /// 从 url 中获取某项的值，并转换成整数
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public int GetInt( String queryItemName ) {
            if ((_context.get( queryItemName ) != null) && cvt.IsInt( _context.get( queryItemName ) )) {
                return int.Parse( _context.get( queryItemName ) );
            }
            return 0;
        }

        /// <summary>
        /// 从 url 中获取某项的值，并转换成 Decimal
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public Decimal GetDecimal( String queryItemName ) {
            if ((_context.get( queryItemName ) != null)) {
                return cvt.ToDecimal( _context.get( queryItemName ) );
            }
            return 0;
        }

        /// <summary>
        /// 从 url 中获取某项的值，并转换成 Double
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public Double GetDouble( String queryItemName ) {
            if ((_context.get( queryItemName ) != null)) {
                return cvt.ToDouble( _context.get( queryItemName ) );
            }
            return 0;
        }

        private String _ip;

        /// <summary>
        /// 获取客户端 ip 地址
        /// </summary>
        public String Ip {
            get {
                if (_ip != null) return _ip;
                _ip = getIp();
                return _ip;
            }
        }

        private String getIp() {

            String ip;
            if (_context.ClientVar( "HTTP_VIA" ) != null) {
                ip = _context.ClientVar( "HTTP_X_FORWARDED_FOR" );
                if (strUtil.IsNullOrEmpty( ip ) || checkIp( ip ) == "unknow") {
                    ip = _context.ClientVar( "REMOTE_ADDR" );
                }
            }
            else {
                ip = _context.ClientVar( "REMOTE_ADDR" );
                if (strUtil.IsNullOrEmpty( ip ) || checkIp( ip ) == "unknow") {
                    ip = _context.ClientVar( "HTTP_X_FORWARDED_FOR" );
                }
            }

            String result = checkIp( ip );
            if (result == "unknow") {
                logger.Error( "ip unknow" );
                logger.Error( "HTTP_VIA=" + _context.ClientVar( "HTTP_VIA" ) );
                logger.Error( "HTTP_X_FORWARDED_FOR=" + _context.ClientVar( "HTTP_X_FORWARDED_FOR" ) );
                logger.Error( "REMOTE_ADDR=" + _context.ClientVar( "REMOTE_ADDR" ) );
            }
            return result;
        }

        private String checkIp( String ip ) {

            int maxLength = 3 * 15 + 2;
            String unknow = "unknow";

            if (ip == "::1") return "127.0.0.1";

            if (strUtil.IsNullOrEmpty( ip ) || ip.Length > maxLength || ip.Length < 7) return unknow;

            char[] arr = ip.ToCharArray();
            foreach (char c in arr) {
                if (!char.IsDigit( c ) && c != '.' && c != ',') return unknow;
            }

            return ip;
        }

        //------------------------------------------- POST ------------------------------------------

        /// <summary>
        /// 获取客户端 post 的值，结果已被过滤(不允许html)
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String Post( String postItem ) {
            String val = _context.post( postItem );
            return checkClientValue( val );
        }

        /// <summary>
        /// 检查客户端 post 的数据中是否有某项 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean PostHas( String key ) {
            return _context.postHas( key );
        }

        /// <summary>
        /// 从客户端 post 的数据中获取某项的值，并转换成 decimal
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Decimal PostDecimal( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToDecimal( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// 从客户端 post 的数据中获取某项的值，并转换成 Double
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Double PostDouble( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToDouble( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// 从客户端 post 的数据中获取 id 列表，结果经过了验证，是类型安全的
        /// </summary>
        /// <param name="idname"></param>
        /// <returns></returns>
        public String PostIdList( String idname ) {
            String ids = Post( idname );
            if (!cvt.IsIdListValid( ids )) {
                return null;
            }
            return ids;
        }

        /// <summary>
        /// 从客户端 post 的数据中获取某项的值，并转换成整数
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public int PostInt( String postItem ) {
            if ((_context.post( postItem ) != null) && cvt.IsInt( _context.post( postItem ) )) {
                return int.Parse( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// 检查客户端是否已经勾选了多选框，如果勾选返回1，否则返回0
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns>如果勾选返回1，否则返回0</returns>
        public int PostIsCheck( String postItem ) {
            String target = Post( postItem );
            if (strUtil.HasText( target ) && target.Equals( "on" )) {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 从客户端 post 的数据中获取某项的值，并转换成时间类型。如果无提交值或格式错误，则返回当前时间DateTime.Now
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public DateTime PostTime( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToTime( _context.post( postItem ) );
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 获取客户端 post 的 html，结果已被过滤(管理员除外)，只有在白名单中的 tag 才被允许。
        /// <para>自定义白名单方法：修改 mvc.config 中的 tagWhiteList 项。</para>
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String PostHtml( String postItem ) {
            String val = _context.post( postItem );
            if (val != null) {

                if (this.viewer != null && this.viewer.IsAdministrator()) return val;
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.Filter( val );
            }
            return val;
        }

        /// <summary>
        /// 获取客户端 post 的 html，结果已被过滤，只允许 allowedTags 中指定的 tag
        /// </summary>
        /// <param name="postItem"></param>
        /// <param name="allowedTags"></param>
        /// <returns></returns>
        public String PostHtml( String postItem, String allowedTags ) {
            String val = _context.post( postItem );
            if (val != null) {
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.Filter( val, allowedTags );
            }
            return val;
        }

        /// <summary>
        /// 获取客户端 post 的 html，结果已被过滤，只允许 allowedTags 中指定的 tag
        /// </summary>
        /// <param name="postItem"></param>
        /// <param name="allowedTags">允许的tag，包括属性列表</param>
        /// <returns></returns>
        public String PostHtml( String postItem, Dictionary<String, String> allowedTags ) {
            String val = _context.post( postItem );
            if (val != null) {
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.Filter( val, allowedTags );
            }
            return val;
        }

        /// <summary>
        /// 在默认白名单的基础上，允许 allowedTags 中指定的tag
        /// </summary>
        /// <param name="postItem"></param>
        /// <param name="allowedTags"></param>
        /// <returns></returns>
        public String PostHtmlAppendTags( String postItem, String allowedTags ) {

            String val = _context.post( postItem );
            if (val != null) {
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.FilterAppendTags( val, allowedTags );
            }
            return val;
        }

        /// <summary>
        /// 允许接收客户端任意字符，请谨慎使用
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String PostHtmlAll( String postItem ) {
            return _context.post( postItem );
        }

        //------------------------------------------- PARAMS ------------------------------------------

        /// <summary>
        /// 获取客户端提交的数据(包括get和post)，结果已被过滤(不允许html)
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public String Params( String itemName ) {
            String val = _context.param( itemName );
            return checkClientValue( val );
        }

        /// <summary>
        /// 从客户端提交的数据中获取某项的值，并转换成整数
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public int ParamInt( String postItem ) {
            if ((_context.param( postItem ) != null) && cvt.IsInt( _context.param( postItem ) )) {
                return int.Parse( _context.param( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// 从客户端提交的数据中获取某项的值，并转换成 Decimal
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Decimal ParamDecimal( String postItem ) {
            if (_context.param( postItem ) != null) {
                return cvt.ToDecimal( _context.param( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// 从客户端提交的数据中获取某项的值，并转换成 Double
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Double ParamDouble( String postItem ) {
            if (_context.param( postItem ) != null) {
                return cvt.ToDouble( _context.param( postItem ) );
            }
            return 0;
        }

        //-------------------------------------------------------------------------------------

        /// <summary>
        /// 验证对象的各项属性是否合法
        /// </summary>
        /// <param name="target">需要被验证的对象</param>
        /// <returns>返回验证结果</returns>
        public Result Validate( IEntity target ) {
            return Validator.Validate( target );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PostObject<T>() {
            return PostObject<T>( null );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lblName">表单中对象名称。如果为空，使用属性名</param>
        /// <returns></returns>
        public T PostObject<T>( String lblName ) {
            return PostValue<T>( lblName );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Object PostObject( Object obj ) {
            return PostObject( obj, null );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lblName">表单中对象名称。如果为空，使用属性名</param>
        /// <returns></returns>
        public Object PostObject( Object obj, String lblName ) {
            return PostValue( obj, lblName );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PostValue<T>() {
            return PostValue<T>( strUtil.GetCamelCase( typeof( T ).Name ) );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lblName">表单中对象名称。如果为空，则使用对象类型的camel格式</param>
        /// <returns></returns>
        public T PostValue<T>( String lblName ) {

            EntityInfo entityInfo = Entity.GetInfo( typeof( T ) );
            Type t = typeof( T );
            T obj = (T)rft.GetInstance( t );

            setObjectProperties( lblName, entityInfo, t, obj );

            IEntity entity = obj as IEntity;
            if (entity != null) {
                Result result = Validate( entity );
                if (result.HasErrors) errors.Join( result );
            }

            return obj;
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Object PostValue( Object obj ) {
            return PostValue( obj, strUtil.GetCamelCase( obj.GetType().Name ) );
        }

        /// <summary>
        /// 获取客户端post的数据，并自动赋值到对象各属性，最后进行验证
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lblName">表单中对象名称。如果为空，则使用对象类型的camel格式</param>
        /// <returns></returns>
        public Object PostValue( Object obj, String lblName ) {

            EntityInfo entityInfo = Entity.GetInfo( obj );
            Type t = obj.GetType();
            setObjectProperties( lblName, entityInfo, t, obj );

            IEntity entity = obj as IEntity;
            if (entity != null) {
                Result result = Validate( entity );
                if (result.HasErrors) errors.Join( result );
            }

            return obj;
        }


        private void setObjectProperties( String lblName, EntityInfo entityInfo, Type t, Object obj ) {

            String prefix = "";

            if (strUtil.HasText( lblName )) {
                prefix = lblName + ".";
            }

            NameValueCollection posts = _context.postValueAll();
            foreach (String key in posts.Keys) {

                if (strUtil.HasText( prefix ) && key.StartsWith( prefix ) == false) continue;

                String propertyName = strUtil.TrimStart( key, prefix );
                PropertyInfo p = t.GetProperty( propertyName );
                if (p == null) continue;

                if (entityInfo == null)
                    setPropertyValue( obj, p, posts[key] );
                else {
                    EntityPropertyInfo ep = entityInfo.GetProperty( propertyName );
                    setEntityPropertyValue( obj, ep, posts[key] );
                }
            }
        }

        private String checkClientValue( String val ) {

            if (val != null) {
                val = val.Trim();
                val = HttpUtility.HtmlEncode( val );
            }

            return val;
        }

        private void setPropertyValue( Object obj, PropertyInfo p, String postValue ) {

            if (p.PropertyType == typeof( int )) {
                p.SetValue( obj, cvt.ToInt( postValue ), null );
            }
            else if (p.PropertyType == typeof( String )) {
                p.SetValue( obj, getAutoPostString( p, postValue ), null );
            }
            else if (p.PropertyType == typeof( Decimal )) {
                p.SetValue( obj, cvt.ToDecimal( postValue ), null );
            }
            else if (p.PropertyType == typeof( Double )) {
                p.SetValue( obj, cvt.ToDouble( postValue ), null );
            }
            else if (p.PropertyType == typeof( DateTime )) {
                p.SetValue( obj, cvt.ToTime( postValue ), null );
            }
        }

        private void setEntityPropertyValue( Object obj, EntityPropertyInfo p, String postValue ) {

            if (p.Type == typeof( int )) {
                p.SetValue( obj, cvt.ToInt( postValue ) );
            }
            else if (p.Type == typeof( String )) {

                p.SetValue( obj, getAutoPostString( p.Property, postValue ) );

            }
            else if (p.Type == typeof( Decimal )) {
                p.SetValue( obj, cvt.ToDecimal( postValue ) );
            }
            else if (p.Type == typeof( Double )) {
                p.SetValue( obj, cvt.ToDouble( postValue ) );
            }
            else if (p.Type == typeof( DateTime )) {
                p.SetValue( obj, cvt.ToTime( postValue ) );
            }
            else if (p.IsEntity) {
                IEntity objProperty = Entity.New( p.EntityInfo.FullName );
                objProperty.Id = cvt.ToInt( postValue );
                p.SetValue( obj, objProperty );

            }
        }

        private String getAutoPostString( PropertyInfo p, String postValue ) {

            Attribute htmlAttr = rft.GetAttribute( p, typeof( HtmlTextAttribute ) );
            if (htmlAttr == null) postValue = checkClientValue( postValue );
            return postValue;
        }

        private Boolean _isRunAction = true;

        internal void isRunAction( Boolean isRun ) {
            _isRunAction = isRun;
        }

        internal Boolean isRunAction() {
            return _isRunAction;
        }

        //------------------------------------------------------------------------------------

        private CtxLink _link;

        /// <summary>
        /// 获取链接对象
        /// </summary>
        /// <returns></returns>
        public CtxLink link {
            get {
                if (_link == null) _link = new CtxLink( this );
                return _link;
            }
        }

    }
}