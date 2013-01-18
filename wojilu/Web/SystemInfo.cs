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
using System.Web;
using wojilu.Web.Mvc;

namespace wojilu.Web {

    /// <summary>
    /// 系统的固定数据，比如网站根路径、app路径、主机名称等。
    /// 1) 可以在非web请求时候使用，比如定时器的新线程中使用;
    /// 2) 获取不同域名级别下的的统一主机值
    /// </summary>
    public class SystemInfo {

        public static SystemInfo Instance = loadSystemInfo();

        private SystemInfo() { }

        /// <summary>
        /// 比 ApplicationPath 多一个斜杠
        /// </summary>
        public static String RootPath { get { return SystemInfo.Instance.rootPath; } }

        /// <summary>
        /// 比如 /myapp 或者 /
        /// </summary>
        public static String ApplicationPath { get { return SystemInfo.Instance.applicationPath; } }

        /// <summary>
        /// 主机名 www.wojilu.com 或 localhost 或 127.0.0.1
        /// </summary>
        public static String Host { get { return SystemInfo.Instance.host; } }

        /// <summary>
        /// 网站首页，普通模式是"/"，二级域名下是http加主机名
        /// </summary>
        public static String SiteRoot { get { return SystemInfo.Instance.siteRoot; } }

        /// <summary>
        /// 不带二级域名的 Host 名称，比如 abc.com 或 baidu.com
        /// </summary>
        public static String HostNoSubdomain { get { return SystemInfo.Instance.hostNoSubdomain; } }

        /// <summary>
        /// Host 是否是ip地址
        /// </summary>
        public static Boolean HostIsIp {
            get { return SystemInfo.Instance.hostIsIp; }
        }

        /// <summary>
        /// Host 是否等于 localhost
        /// </summary>
        public static Boolean HostIsLocalhost {
            get { return SystemInfo.Instance.hostIsLocalhost; }
        }

        /// <summary>
        /// 主机名(或ip地址)+端口号
        /// </summary>
        public static String Authority { get { return SystemInfo.Instance.authority; } }

        private String rootPath;
        private String applicationPath;
        private String host;
        private String authority;

        private Boolean hostIsIp;
        private Boolean hostIsLocalhost;
        private String hostNoSubdomain;
        private String siteRoot;

        public String Scheme { get; set; }

        /// <summary>
        /// 比如 http://www.wojilu.com/app
        /// </summary>
        public String SiteUrl { get; set; }

        //-------------------------------------------------------------------------------------------------

        private static Boolean _initialized = false;
        private static Object _objLock = new Object();

        /// <summary>
        /// 网站初始化时候启动
        /// </summary>
        public static void Init() {


            if (!_initialized) {

                lock (_objLock) {

                    if (!_initialized) {

                        SystemInfo obj = SystemInfo.Instance;

                        loadSubdomainInfo( obj );

                        _initialized = true;

                    }
                }

            }
        }

        private static SystemInfo loadSystemInfo() {

            SystemInfo obj = new SystemInfo();

            if (IsWeb) {

                obj.applicationPath = HttpContext.Current.Request.ApplicationPath;
                obj.rootPath = addEndSlash( obj.applicationPath );
                obj.authority = HttpContext.Current.Request.Url.Authority;

                obj.host = HttpContext.Current.Request.Url.Host;

                obj.hostIsLocalhost = strUtil.EqualsIgnoreCase( obj.host, "localhost" );
                obj.hostIsIp = cvt.IsInt( obj.host.Split( '.' )[0] );
                obj.hostNoSubdomain = getHostNoSubdomain( obj );

                obj.Scheme = HttpContext.Current.Request.Url.Scheme;

                obj.SiteUrl = obj.Scheme + "://" + strUtil.Join( obj.authority, obj.applicationPath );
            }
            else {
                obj.applicationPath = "/";
                obj.rootPath = "/";
                obj.host = "localhost";
            }

            return obj;
        }

        private static void loadSubdomainInfo( SystemInfo obj ) {

            if (MvcConfig.Instance.IsDomainMap) {

                if (strUtil.IsNullOrEmpty( config.Instance.Site.SiteUrl ) || config.Instance.Site.SiteUrl.StartsWith( "www." )) {
                    obj.host = "www." + obj.hostNoSubdomain;
                }
                else {
                    obj.host = obj.hostNoSubdomain;
                }

                obj.siteRoot = obj.Scheme + "://" + SystemInfo.Host;

            }
            else {
                obj.siteRoot = "/";
            }
        }

        private static String getHostNoSubdomain( SystemInfo obj ) {

            if (obj.hostIsIp || obj.hostIsLocalhost) {
                return obj.host;
            }
            else {
                return getHostNoSubdomain( obj.host );
            }

        }

        private static String getHostNoSubdomain( String host ) {
            int firstDotIndex = host.IndexOf( '.' );
            String result = host.Substring( firstDotIndex + 1, host.Length - firstDotIndex - 1 );
            if (result.IndexOf( '.' ) < 0) return host;
            return result;
        }

        private static String addEndSlash( String appPath ) {
            if (!appPath.EndsWith( "/" )) return appPath + "/";
            return appPath;
        }


        public static Boolean IsWeb { get { return HttpContext.Current != null; } }
        public static Boolean IsWindows {
            get { return Environment.OSVersion.VersionString.ToLower().IndexOf( "windows" ) >= 0; }
        }

        //-------------------------------------------------------------------------------------------------

        public static readonly String clientSessionID = "clientSessionID";

        public static void UpdateSessionId() {
            String sessionId = getClientSessionId();
            if (sessionId != null) updateCookie( sessionId );
        }

        private static String getClientSessionId() {
            HttpRequest req = HttpContext.Current.Request;
            if (req.Form[clientSessionID] != null) return req.Form[clientSessionID];
            if (req.QueryString[clientSessionID] != null) return req.QueryString[clientSessionID];
            return null;
        }

        private static void updateCookie( String sessionId ) {

            String aspSessionCookieName = "ASP.NET_SESSIONID";

            HttpRequest req = HttpContext.Current.Request;
            HttpResponse res = HttpContext.Current.Response;

            HttpCookie cookie = req.Cookies.Get( aspSessionCookieName );

            if (cookie == null) {
                res.Cookies.Add( new HttpCookie( aspSessionCookieName, sessionId ) );
            }
            else {
                cookie.Value = sessionId;
                req.Cookies.Set( cookie );
            }
        }


    }

}
