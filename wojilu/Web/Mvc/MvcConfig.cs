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
using System.Collections.Generic;
using wojilu.Config;
using wojilu.ORM;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// mvc 配置
    /// </summary>
    public class MvcConfig {

        private static MvcConfig _instance = new MvcConfig();

        public static MvcConfig Instance {
            get { return _instance; }
        }

        /// <summary>
        /// 路由文件的绝对路径。默认是根目录绝对路径加上 /framework/config/route.config
        /// </summary>
        public String RouteConfigPath { get { return _routeConfigPath; } }

        /// <summary>
        /// 所有控制器的根命名空间。搜索 controller 时，会从这个根命名空间开始
        /// </summary>
        public List<String> RootNamespace { get { return _rootNamespace; } }

        /// <summary>
        /// 网址后缀名。默认是.aspx，如果不为空，则带前缀.点号
        /// </summary>
        public String UrlExt { get { return _urlExt; } }

        /// <summary>
        /// 网址分隔符，默认是斜杠 "/"，可以配置成横杠 "-"
        /// </summary>
        public String UrlSeparator { get { return _urlSeparator; } }

        /// <summary>
        /// 网址是否小写。默认是false
        /// </summary>
        public Boolean IsUrlToLower { get { return _isUrlToLower; } }

        /// <summary>
        /// 视图文件的后缀名。默认是.html，如果不为空，则带前缀.点号
        /// </summary>
        public String ViewExt { get { return _viewExt; } }

        /// <summary>
        /// 视图文件所在目录。默认值是 /framework/views/ ，也可以在 mvc.config 中添加 viewsDir 项自定义
        /// </summary>
        public String ViewDir { get { return _viewDir; } }

        /// <summary>
        /// mvc框架的版本号
        /// </summary>
        public String Version { get { return _version; } }

        /// <summary>
        /// 是否解析appId，默认解析，即将 Mycontroller16/List.aspx 中的16解析成appId
        /// </summary>
        public Boolean IsParseAppId { get { return _isParseAppId; } }

        /// <summary>
        /// 是否缓存视图模板，默认不缓存。
        /// </summary>
        public Boolean IsCacheView { get { return _isCacheView; } }

        /// <summary>
        /// 是否启用action缓存(如果关闭，所有action缓存都会失效)
        /// </summary>
        public Boolean IsActionCache { get { return _isActionCache; } }

        /// <summary>
        /// 是否启用页面缓存(如果关闭，所有页面缓存都会失效)
        /// </summary>
        public Boolean IsPageCache { get { return _isPageCache; } }

        /// <summary>
        /// 是否启用域名映射支持(比如二级域名映射)
        /// </summary>
        public Boolean IsDomainMap { get { return _isDomainMap; } }

        /// <summary>
        /// 是否启用域名映射支持(比如二级域名映射)，除了检查本配置文件，还要检查是否是ip/localhost等情况
        /// </summary>
        /// <returns></returns>
        public Boolean CheckDomainMap() {
            if (this.IsDomainMap == false) return false;
            if (SystemInfo.HostIsIp) return false;
            if (SystemInfo.HostIsLocalhost) return false;
            return true;
        }


        /// <summary>
        /// 二级域名通配符映射的类型
        /// </summary>
        public String SubdomainWildcardType { get { return _subdomainWildcardType; } }

        /// <summary>
        /// 全站所有 js 的版本号
        /// </summary>
        public String JsVersion { get { return _jsVersion; } }

        /// <summary>
        /// 全站所有 css 的版本号
        /// </summary>
        public String CssVersion { get { return _cssVersion; } }

        /// <summary>
        /// 静态文件所在的二级域名，比如 static 
        /// </summary>
        public String StaticDomain { get { return _staticDomain; } }

        /// <summary>
        /// 自定义的过滤器类型，比如 wojilu.Web.Controller.RenderHelper
        /// </summary>
        public List<String> Filter { get { return _filterList; } }

        /// <summary>
        /// 允许客户端提交的 html tag 白名单
        /// </summary>
        public List<String> TagWhitelist { get { return _tagWhiteList; } }


        //------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 获取网站发生错误时报错的模板路径，默认是 /framework/views/error.html
        /// </summary>
        /// <returns></returns>
        public String GetErrorTemplatePath() {
            return strUtil.Join( this.ViewDir, "error" ) + this.ViewExt;
        }

        /// <summary>
        /// 获取反馈信息(通常使用echo方法时使用)的模板路径，默认是 /framework/views/msg.html
        /// </summary>
        /// <returns></returns>
        public String GetMsgTemplatePath() {
            return strUtil.Join( this.ViewDir, "msg" ) + this.ViewExt;
        }

        /// <summary>
        /// 获取弹窗的模板路径，默认是 /framework/views/msgbox.html
        /// </summary>
        /// <returns></returns>
        public String GetMsgBoxTemplatePath() {
            return strUtil.Join( this.ViewDir, "msgbox" ) + this.ViewExt;
        }

        /// <summary>
        /// 获取页面跳转的模板路径，默认是 /framework/views/forward.html
        /// </summary>
        /// <returns></returns>
        public String GetForwardTemplatePath() {
            return strUtil.Join( this.ViewDir, "forward" ) + this.ViewExt;
        }

        /// <summary>
        /// 当网站发生异常时，不用记入错误日志的 HttpStatus
        /// </summary>
        /// <returns></returns>
        public List<int> getNoLogStatusCode() {

            List<int> results = new List<int>();
            if (strUtil.IsNullOrEmpty( _noLogError )) return results;

            int[] arrInt = cvt.ToIntArray( _noLogError );
            foreach (int code in arrInt) results.Add( code );
            return results;
        }

        //------------------------------------------------------------------------------------------------------------------------------

        private String _routeConfigPath;

        private List<String> _rootNamespace;
        private List<String> _filterList;
        private List<String> _tagWhiteList;

        private String _urlExt;
        private String _viewExt;
        private String _viewDir;
        private String _version;

        private Boolean _isParseAppId;
        private Boolean _isCacheView;
        private Boolean _isActionCache;
        private Boolean _isPageCache;
        private Boolean _isDomainMap;
        private String _subdomainWildcardType;

        private String _jsVersion;
        private String _cssVersion;
        private String _staticDomain;

        private String _urlSeparator;
        private Boolean _isUrlToLower;

        private String _noLogError;

        private MvcConfig() {

            Dictionary<String, String> dic = cfgHelper.Read( PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "mvc.config" ) ) );

            _routeConfigPath = PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "route.config" ) );

            _rootNamespace = getValueList( dic, "rootNamespace" );
            _isParseAppId = getIsParseAppId( dic );
            _isCacheView = getIsCacheView( dic );

            _isActionCache = getIsActionCache( dic );
            _isPageCache = getIsPageCache( dic );

            _isDomainMap = getIsDomainMap( dic );

            _urlExt = getUrlExt( dic );
            _viewExt = getViewExt( dic );
            _viewDir = getViewDir( dic );
            _version = getVersionDir( dic );

            _filterList = getValueList( dic, "filter" );
            _tagWhiteList = getValueList( dic, "tagWhiteList" );

            _urlSeparator = getUrlSeparator( dic );
            _isUrlToLower = getIsUrlToLower( dic );

            dic.TryGetValue( "jsVersion", out _jsVersion );
            dic.TryGetValue( "cssVersion", out _cssVersion );
            dic.TryGetValue( "staticDomain", out _staticDomain );
            dic.TryGetValue( "noLogError", out _noLogError );

            dic.TryGetValue( "subdomainWildcardType", out _subdomainWildcardType );

        }

        private List<String> getValueList( Dictionary<String, String> dic, String keyName ) {

            List<String> result = new List<String>();

            String rawStr;
            dic.TryGetValue( keyName, out rawStr );
            if (strUtil.IsNullOrEmpty( rawStr )) return result;

            String[] arr = rawStr.Split( ',' );
            foreach (String item in arr) {
                if (strUtil.IsNullOrEmpty( item )) continue;
                result.Add( item.Trim() );
            }

            return result;
        }

        private Boolean getIsParseAppId( Dictionary<String, String> dic ) {
            String isParseAppIdStr;
            dic.TryGetValue( "isParseAppId", out isParseAppIdStr );
            return cvt.ToBool( isParseAppIdStr );
        }

        private Boolean getIsCacheView( Dictionary<String, String> dic ) {
            String isCacheView;
            dic.TryGetValue( "isCacheView", out isCacheView );
            return cvt.ToBool( isCacheView );
        }


        private bool getIsActionCache( Dictionary<String, String> dic ) {
            String isActionCache;
            dic.TryGetValue( "isActionCache", out isActionCache );
            return cvt.ToBool( isActionCache );
        }

        private bool getIsPageCache( Dictionary<String, String> dic ) {
            String isPageCache;
            dic.TryGetValue( "isPageCache", out isPageCache );
            return cvt.ToBool( isPageCache );
        }

        private bool getIsDomainMap( Dictionary<String, String> dic ) {
            String isDomainMap;
            dic.TryGetValue( "isDomainMap", out isDomainMap );
            return cvt.ToBool( isDomainMap );
        }

        private String getViewDir( Dictionary<String, String> dic ) {

            String viewsDir;
            dic.TryGetValue( "viewsDir", out viewsDir );
            if (strUtil.IsNullOrEmpty( viewsDir )) viewsDir = "/framework/views";

            viewsDir = strUtil.Append( viewsDir, "/" );
            if (viewsDir.StartsWith( cfgHelper.FrameworkRoot ) == false)
                viewsDir = strUtil.Join( cfgHelper.FrameworkRoot, viewsDir );
            return viewsDir;
        }

        private String getViewExt( Dictionary<String, String> dic ) {

            String ext;
            dic.TryGetValue( "viewsExt", out ext );
            if (strUtil.IsNullOrEmpty( ext )) return "";

            if (!ext.StartsWith( "." )) return "." + ext;
            return ext;
        }

        private string getVersionDir( Dictionary<String, String> dic ) {
            String version;
            dic.TryGetValue( "version", out version );
            if (strUtil.IsNullOrEmpty( version )) return "1.9";
            return version;
        }

        private String getUrlExt( Dictionary<String, String> dic ) {

            String ext;
            dic.TryGetValue( "urlExt", out ext );
            if (strUtil.IsNullOrEmpty( ext )) return "";

            if (!ext.StartsWith( "." )) return "." + ext;
            return ext;
        }

        private String getUrlSeparator( Dictionary<String, String> dic ) {

            String defaultValue = "/";

            String urlSp;
            dic.TryGetValue( "urlSeparator", out urlSp );
            if (strUtil.IsNullOrEmpty( urlSp )) return defaultValue;

            urlSp = urlSp.Trim();
            if (urlSp.Length > 1) return defaultValue;

            return urlSp;
        }

        private bool getIsUrlToLower( Dictionary<String, String> dic ) {
            String isLower;
            dic.TryGetValue( "isUrlToLower", out isLower );
            if (strUtil.IsNullOrEmpty( isLower )) return false;
            return cvt.ToBool( isLower );
        }

        public static void Reset() {
            _instance = new MvcConfig();
        }

    }
}

