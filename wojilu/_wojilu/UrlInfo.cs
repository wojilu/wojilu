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
using System.Text;
using wojilu.Web;
using System.Web;

namespace wojilu {

    /// <summary>
    /// 封装了 url 的一些基本信息。
    /// </summary>
    /// <example>
    /// 如下的网址
    /// <code>
    /// Uri uri = new Uri( "http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top" );
    /// UrlInfo u = new UrlInfo( uri, "/myapp/", "myPathInfo" );
    /// </code>
    /// 返回的结果是
    /// <code>
    /// Scheme=>http
    /// UserName=>zhangsan
    /// Password=>123
    /// Host=>www.abc.com
    /// Port=>80
    /// Path=>/myapp/Photo/1984
    /// PathAndQuery=>/myapp/Photo/1984?title=eee
    /// PathInfo=>myPathInfo
    /// PathAndQueryWithouApp=>/Photo/1984?title=eee
    /// Query=>?title=eee
    /// Fragment=>#top
    /// SiteUrl=>http://zhangsan:123@www.abc.com
    /// SiteAndAppPath=>http://zhangsan:123@www.abc.com/myapp/
    /// </code>
    /// </example>
    public class UrlInfo {

        public UrlInfo( String url ) {
            Uri uri = new UriBuilder( url ).Uri;
            initByUri( uri );
        }

        public UrlInfo( Uri uri, String applicationPath, String pathInfo ) {
            initByUri( uri );
            this.AppPath = applicationPath;
            this.PathInfo = pathInfo;
        }

        public UrlInfo( String url, String applicationPath, String pathInfo ) {
            Uri uri = new UriBuilder( url ).Uri;
            initByUri( uri );
            this.AppPath = applicationPath;
            this.PathInfo = pathInfo;
        }

        private void initByUri( Uri uri ) {
            this.ub = new UriBuilder( uri );
            this.Scheme = ub.Scheme;
            this.UserName = ub.UserName;
            this.Password = ub.Password;
            this.Host = ub.Host;
            this.Port = ub.Port;
            this.Path = ub.Path;
            this.Query = ub.Query;
            this.Fragment = ub.Fragment;
        }

        private String _AppPath;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 /myapp/
        /// </summary>
        public String AppPath {
            get { return _AppPath; }
            set { _AppPath = value; }
        }

        private String _Scheme;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 http
        /// </summary>
        public String Scheme {
            get { return _Scheme; }
            set { _Scheme = value; }
        }

        private String _UserName;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 zhangsan
        /// </summary>
        public String UserName {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private String _Password;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 123
        /// </summary>
        public String Password {
            get { return _Password; }
            set { _Password = value; }
        }

        private String _Host;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 www.abc.com
        /// </summary>
        public String Host {
            get { return _Host; }
            set { _Host = value; }
        }

        private int _Port;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 80
        /// </summary>
        public int Port {
            get { return _Port; }
            set { _Port = value; }
        }

        private String _Path;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 /myapp/Photo/1984
        /// </summary>
        public String Path {
            get { return _Path; }
            set { _Path = value; }
        }

        private String _Query;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 ?title=eee
        /// </summary>
        public String Query {
            get { return _Query; }
            set { _Query = value; }
        }

        private String _Fragment;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 #top
        /// </summary>
        public String Fragment {
            get { return _Fragment; }
            set { _Fragment = value; }
        }

        private String _PathInfo;

        public String PathInfo {
            get { return _PathInfo; }
            set { _PathInfo = value; }
        }

        /// <summary>
        /// 网站根目录之后的路径(如果当前应用放在虚拟目录中，则包括虚拟目录)，例如
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 /myapp/Photo/1984?title=eee
        /// </summary>
        public String PathAndQuery {
            get { return Path + Query; }
        }

        /// <summary>
        /// 不包括网站域名和虚拟目录的完整路径，前面包括斜杠"/"。例如
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 /Photo/1984?title=eee
        /// </summary>
        public String PathAndQueryWithouApp {
            get {
                String result = strUtil.TrimStart( PathAndQuery, AppPath );
                return result.StartsWith( "/" ) ? result : "/" + result;
            }
        }

        private String siteUrl;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 http://zhangsan:123@www.abc.com
        /// </summary>
        public String SiteUrl {
            get {
                if (siteUrl == null)
                    siteUrl = getSiteUrl();
                return siteUrl;
            }
        }

        private String siteAndAppPath;

        /// <summary>
        /// http://zhangsan:123@www.abc.com/myapp/Photo/1984?title=eee#top 返回 http://zhangsan:123@www.abc.com/myapp/
        /// </summary>
        public String SiteAndAppPath {
            get {
                if (siteAndAppPath == null)
                    siteAndAppPath = strUtil.Join( SiteUrl, AppPath );
                return siteAndAppPath;
            }
        }

        private String getSiteUrl() {
            StringBuilder sb = new StringBuilder();
            sb.Append( Scheme );
            sb.Append( "://" );
            append_username_password( sb );
            sb.Append( Host );
            append_port( sb );
            return sb.ToString();
        }

        private void append_username_password( StringBuilder sb ) {
            if (strUtil.IsNullOrEmpty( UserName ) && strUtil.IsNullOrEmpty( Password ))
                return;

            if (strUtil.HasText( UserName ) && strUtil.HasText( Password )) {
                sb.Append( UserName );
                sb.Append( ":" );
                sb.Append( Password );
                sb.Append( "@" );
                return;
            }

            String up = null;
            if (strUtil.HasText( UserName ))
                up = UserName;
            else if (strUtil.HasText( Password ))
                up = Password;

            sb.Append( up );
            sb.Append( ":" );
            sb.Append( up );
            sb.Append( "@" );
            return;
        }

        private void append_port( StringBuilder sb ) {
            if (Port == 80) return;

            sb.Append( ":" );
            sb.Append( Port );
            sb.Append( "/" );
        }

        private UriBuilder ub;

        /// <summary>
        /// 获取 UriBuilder 对象
        /// </summary>
        public UriBuilder UriBuilder {
            get {
                if (ub == null) {
                    ub = new UriBuilder();
                    ub.Scheme = this.Scheme;
                    ub.UserName = this.UserName;
                    ub.Password = this.Password;
                    ub.Host = this.Host;
                    ub.Port = this.Port;
                    ub.Path = this.Path;
                    ub.Query = this.Query;
                    ub.Fragment = this.Fragment;
                }
                return ub;
            }
        }

        /// <summary>
        /// 完整的网址路径，包括http前缀以及query string等所有信息；相当于直接拷贝浏览器地址栏的网址。
        /// </summary>
        /// <returns></returns>
        public override String ToString() {
            return UriBuilder.Uri.ToString();
        }

        /// <summary>
        /// 返回编码过(Server.UrlEncode)的完整 url 
        /// </summary>
        /// <returns></returns>
        public String EncodeUrl {
            get {
                return HttpContext.Current.Server.UrlEncode( this.ToString() );
            }
        }


    }
}



