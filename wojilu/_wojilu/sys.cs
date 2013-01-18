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
using wojilu.Log;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Mvc;
using wojilu.Caching;
using wojilu.Web;
using wojilu.ORM.Caching;
using wojilu.Data;
using wojilu.Web.Mock;

namespace wojilu {

    /// <summary>
    /// 系统常用对象的快捷方式，比如 SysPath(系统路径)
    /// </summary>
    public class sys {

        /// <summary>
        /// 系统路径
        /// </summary>
        public static SysPath Path {
            get { return SysPath.Instance; }
        }

        /// <summary>
        /// 清理缓存，重新加载配置文件
        /// </summary>
        public static CacheClear Clear {
            get { return CacheClear.Instance; }
        }

        /// <summary>
        /// 网址信息，包括网站固定网址，也包括当前请求网址
        /// </summary>
        public static SysRequest Url {
            get { return new SysRequest(); }
        }

    }

    public class SysRequest {

        /// <summary>
        /// 返回 http 或者 https，如果要返回 http:// ，请属性 SchemeStr
        /// </summary>
        public String Scheme {
            get { return SystemInfo.Instance.Scheme; }
        }

        /// <summary>
        /// 返回 http:// 或者 https://，如果要返回 http ，请属性 Scheme
        /// </summary>
        public String SchemeStr {
            get { return this.Scheme + "://"; }
        }

        /// <summary>
        /// 返回 http://www.abc.com/app 这种格式，包括端口号
        /// </summary>
        public String SiteUrl {
            get { return SystemInfo.Instance.SiteUrl; }
        }

        /// <summary>
        /// 返回 www.abc.com 或 localhost 等，带端口号
        /// </summary>
        public String SiteHost {
            get { return SystemInfo.Authority; }
        }

        /// <summary>
        /// 返回 http 或者 https
        /// </summary>
        public String SiteScheme {
            get { return SystemInfo.Instance.Scheme; }
        }

        /// <summary>
        /// 获取当前请求的原始 url
        /// </summary>
        public String RawUrl {
            get { return CurrentRequest.getRawUrl(); }
        }

        public String[] UserLanguages {
            get { return CurrentRequest.getUserLanguages(); }
        }

        public String HttpMethod {
            get { return CurrentRequest.getHttpMethod(); }
        }

        public String CookieKey {
            get { return CurrentRequest.getCookieKey(); }
        }

        public String LangCookie {
            get { return CurrentRequest.getLangCookie(); }
        }

        /// <summary>
        /// 获取当前请求的翻页数
        /// </summary>
        public int PageNumber {
            get { return CurrentRequest.getCurrentPage(); }
        }


    }


    public class SysContext {

        public String GetForm( String key ) {
            return CurrentRequest.getForm( key );
        }

        public object GetItem( string key ) {
            return CurrentRequest.getItem( key );
        }

        public void SetItem( String key, object obj ) {
            CurrentRequest.setItem( key, obj );
        }

        public void SetRequest( MvcRequest req ) {
            CurrentRequest.setRequest( req );
        }

        public void SetCurrentPage( int current ) {
            CurrentRequest.setCurrentPage( current );
        }

    }

    public class CacheClear {

        public static readonly CacheClear Instance = new CacheClear();

        public void ClearAll() {

            // 单行配置
            this.ClearLogConfig();
            this.ClearMvcConfig();
            this.ClearDbConfig();
            this.ClearRouteConfig();
            this.ClearSiteConfig();

            // 内存数据库
            this.ClearMemoryDB();

            // 模板配置
            this.ClearTemplateCache();

            // ORM数据缓存，和其他系统缓存
            this.ClearOrmCache();
        }


        //--------------------------------------------------------

        public void ClearLogConfig() {
            LogConfig.Reset();
        }

        public void ClearRouteConfig() {
            RouteTable.Reset();
        }

        public void ClearMvcConfig() {
            MvcConfig.Reset();
        }

        public void ClearDbConfig() {
            DbConfig.Reset();
        }

        public void ClearSiteConfig() {
            config.Reset();
        }

        public void ClearMemoryDB() {
            MemoryDB.Clear();
        }

        public void ClearOrmCache() {
            ApplicationPool.Instance.Clear();
        }

        public void ClearTemplateCache() {
            Template.Reset();
        }

    }

}
