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
using System.IO;
using System.Net;
using System.Text;
using wojilu.Web;
using wojilu.DI;
using System.Reflection;

namespace wojilu.Net {

    public class HttpClient {

        public static HttpClient New() {
            return ObjectContext.Create<HttpClient>();
        }

        public static HttpClient Init( String apiUrl ) {
            return Init( apiUrl, "GET" );
        }

        /// <summary>
        /// 创建一个 Rest 类型的调用客户端
        /// </summary>
        /// <param name="apiUrl">需要调用的 api 的网址</param>
        /// <param name="httpMethod">方法类型</param>
        /// <returns></returns>
        public static HttpClient Init( String apiUrl, String httpMethod ) {

            if (strUtil.IsNullOrEmpty( apiUrl )) throw new ArgumentNullException( "api url" );

            HttpClient x = New();
            x.SetUrl( apiUrl );
            x.SetMethod( httpMethod );

            return x;
        }

        protected String _apiUrl;
        protected String _httpMethod = "GET";
        protected String _userAgent = PageLoader.AgentIE8;
        protected String _encoding;

        protected Dictionary<String, String> _parameters = new Dictionary<String, String>();
        protected Dictionary<String, String> _headers = new Dictionary<String, String>();
        protected Dictionary<String, String> _queryItems = new Dictionary<String, String>();
        protected List<HttpFile> _absPathFiles = new List<HttpFile>();

        public IHttpClientHelper restHelper { get; set; }

        private String _response;

        public HttpClient() {
            restHelper = new HttpClientHelper();
        }

        /// <summary>
        /// 需要调用的 api 的网址
        /// </summary>
        /// <param name="apiUrl"></param>
        public virtual HttpClient SetUrl( String apiUrl ) {
            _apiUrl = apiUrl.Trim();
            return this;
        }

        /// <summary>
        /// 调用的方法类型，比如 GET/POST 等
        /// </summary>
        /// <param name="httpMethod"></param>
        public virtual HttpClient SetMethod( String httpMethod ) {
            if (strUtil.IsNullOrEmpty( httpMethod )) return this;
            _httpMethod = httpMethod;
            return this;
        }

        /// <summary>
        /// 设置客户端信息
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public virtual HttpClient SetUserAgent( String userAgent ) {
            _userAgent = userAgent;
            return this;
        }

        /// <summary>
        /// 设置 Encoding
        /// </summary>
        /// <param name="strEncoding"></param>
        /// <returns></returns>
        public virtual HttpClient SetEncoding( String strEncoding ) {
            _encoding = strEncoding;
            return this;
        }

        /// <summary>
        /// 添加调用的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual HttpClient AddParam( String key, Object val ) {
            if (val == null) val = "";
            _parameters.Add( key, val.ToString() );
            return this;
        }

        /// <summary>
        /// 添加调用的参数
        /// </summary>
        /// <param name="parameters"></param>
        public virtual HttpClient AddParam( Dictionary<String, String> parameters ) {
            foreach (KeyValuePair<String, String> kv in parameters) {
                _parameters.Add( kv.Key, kv.Value );
            }
            return this;
        }

        /// <summary>
        /// 添加 header 参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual HttpClient AddHeader( String key, Object val ) {
            if (val == null) val = "";
            _headers.Add( key, val.ToString() );
            return this;
        }

        /// <summary>
        /// 添加 header 参数
        /// </summary>
        /// <param name="headers"></param>
        public virtual HttpClient AddHeader( Dictionary<String, String> headers ) {
            foreach (KeyValuePair<String, String> kv in headers) {
                _headers.Add( kv.Key, kv.Value );
            }
            return this;
        }

        /// <summary>
        /// 添加 url 后面的查询字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual HttpClient AddQuery( String key, Object val ) {
            if (val == null) val = "";
            _queryItems.Add( key, val.ToString() );
            return this;
        }

        /// <summary>
        /// 添加 url 后面的查询字符串
        /// </summary>
        /// <param name="queryItems"></param>
        public virtual HttpClient AddQuery( Dictionary<String, String> queryItems ) {
            foreach (KeyValuePair<String, String> kv in queryItems) {
                _queryItems.Add( kv.Key, kv.Value );
            }
            return this;
        }

        /// <summary>
        /// 将对象的属性和字段的值加入参数(param)中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual HttpClient AddObject( Object obj ) {
            return AddObject( obj, null );
        }

        /// <summary>
        /// 将对象的属性和字段的值加入参数(param)中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prefix">参数的前缀</param>
        /// <returns></returns>
        public virtual HttpClient AddObject( Object obj, String prefix ) {
            if (obj == null) throw new ArgumentNullException( "obj" );
            PropertyInfo[] ps = obj.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );
            if (strUtil.HasText( prefix )) {
                prefix = prefix + ".";
            }
            foreach (PropertyInfo x in ps) {
                this.AddParam( prefix + x.Name, x.GetValue( obj, null ) );
            }
            FieldInfo[] fs = obj.GetType().GetFields( BindingFlags.Public | BindingFlags.Instance );
            foreach (FieldInfo x in fs) {
                this.AddParam( prefix + x.Name, x.GetValue( obj ) );
            }
            return this;
        }

        /// <summary>
        /// 添加需要上传的文件
        /// </summary>
        /// <param name="absFilePath">文件在磁盘上的绝对路径</param>
        public virtual HttpClient AddFile( String absFilePath ) {

            if (strUtil.IsNullOrEmpty( absFilePath )) throw new ArgumentNullException( "absFilePath" );

            _absPathFiles.Add( new HttpFile( absFilePath ) );
            _httpMethod = HttpMethod.Post; // 在上传文件的时候，总是使用POST
            return this;
        }

        /// <summary>
        /// 调用远程 api，返回原始字符串(没有解析过)
        /// </summary>
        /// <returns></returns>
        public virtual String Run() {

            if (_absPathFiles.Count > 0) {
                _response = restHelper.Upload( GetRequestUrl(), _parameters, _headers, _absPathFiles, _userAgent );
            }
            else {
                _response = restHelper.InvokeApi( GetRequestUrl(), _httpMethod, restHelper.ConstructQueryString( _parameters ), _headers, "", "", _userAgent, _encoding );
            }

            return _response;
        }

        /// <summary>
        /// 调用远程 api, 返回已经解析过的 JsonObject
        /// </summary>
        /// <returns></returns>
        public virtual JsonObject RunJson() {
            return this.Run<JsonObject>();
        }

        /// <summary>
        /// 调用远程 api, 返回结果已经解析成强类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Run<T>() {
            String response = this.Run();
            return Json.Deserialize<T>( response );
        }

        /// <summary>
        /// 调用远程 api, 返回结果已经解析成强类型对象的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual List<T> RunList<T>() {
            String response = this.Run();
            return Json.DeserializeList<T>( response );
        }

        /// <summary>
        /// 在调用远程api之后(Run之后)，返回字符串结果。本方法不会再次执行远程调用。
        /// </summary>
        /// <returns></returns>
        public virtual String GetResponse() {
            return _response;
        }

        /// <summary>
        /// 获取远程 api 的网址，已经将参数和查询字符串都拼接上去。
        /// </summary>
        public virtual String GetRequestUrl() {

            String strQuery = getQueryString();
            if (strUtil.IsNullOrEmpty( strQuery )) return _apiUrl;
            if (_apiUrl.IndexOf( "?" ) > 0) {
                return _apiUrl + "&" + strQuery;
            }
            else {
                return _apiUrl + "?" + strQuery;
            }

        }

        private String getQueryString() {

            if (_httpMethod == HttpMethod.Post) {
                return restHelper.ConstructQueryString( _queryItems );
            }
            else {
                String strQuery = restHelper.ConstructQueryString( _queryItems );
                String strParams = restHelper.ConstructQueryString( _parameters );
                return strUtil.Join( strQuery, strParams, "&" ).TrimStart( '&' ).TrimEnd( '&' );
            }
        }



    }

}
