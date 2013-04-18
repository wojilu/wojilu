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
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using wojilu.Drawing;


namespace wojilu.Net {

    /// <summary>
    /// 下载某个网址的页面内容(默认编码方式为 WebResponse 返回的 ContentType 属性)
    /// </summary>
    /// <remarks>更复杂的操作，请使用 .net 自带的 System.Net.HttpWebRequest 对象</remarks>
    /// <example>
    /// 用默认编码方式(即 WebResponse 返回的 ContentType 属性)抓取页面内容
    /// <code>
    /// String pageContent = PageLoader.DownloadPage( "http://www.google.com" );
    /// </code>
    /// 指定编码
    /// <code>
    /// String pageContent = PageLoader.DownloadPage( "http://www.google.com", "utf-8" );
    /// </code>
    /// 指定编码和客户端信息
    /// <code>
    /// String pageContent = PageLoader.DownloadPage( "http://www.google.com", "wojilu spider", "utf-8" );
    /// </code>
    /// </example>
    public class PageLoader {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PageLoader ) );

        public static readonly String AgentIE6 = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
        public static readonly String AgentIE8 = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";

        private String _url;
        private static Encoding _encoding = Encoding.Default;
        private String _pageEncoding = _encoding.BodyName;
        private Boolean _isSetEncoding = false;

        private String _agentInfo = AgentIE6;

        /// <summary>
        /// 客户端信息，默认是Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)
        /// </summary>
        public String AgentInfo {
            get { return _agentInfo; }
            set { _agentInfo = value; }
        }

        /// <summary>
        /// 页面编码(默认是Encoding.Default)，不区分大小写，比如 utf-8, gb2312, Unicode
        /// </summary>
        public String PageEncoding {
            get { return _pageEncoding; }
            set { _isSetEncoding = true; _pageEncoding = value; }
        }

        /// <summary>
        /// 需要下载内容的网址
        /// </summary>
        public String Url {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 开始下载页面内容
        /// </summary>
        /// <returns></returns>
        public String Download() {
            if (_isSetEncoding) {
                return Download( _url, _agentInfo, _pageEncoding );
            }
            return Download( _url, _agentInfo );
        }

        /// <summary>
        /// 下载某个网址的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns>返回页面内容</returns>
        public static String Download( String url ) {
            return Download( url, AgentIE6 );
        }

        /// <summary>
        /// 下载某个网址的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="agentInfo">客户端信息</param>
        /// <returns>返回页面内容</returns>
        public static String Download( String url, String agentInfo ) {
            String str;
            try {

                WebResponse res = getResponse( url, agentInfo );

                StreamReader reader = new StreamReader( res.GetResponseStream(), getEncoding( res ) );
                str = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception exception) {
                logger.Info( "DownloadPage : " + url + ", error : " + exception.Message + "：" );
                throw exception;
            }
            return str;
        }

        /// <summary>
        /// 下载某个网址的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="agentInfo">客户端信息</param>
        /// <param name="encoding">页面编码，不区分大小写，比如 utf-8, gb2312, Unicode</param>
        /// <returns>返回页面内容</returns>
        public static String Download( String url, String agentInfo, String encoding ) {
            String str;
            try {
                WebResponse res = getResponse( url, agentInfo );

                Encoding ec;
                if (strUtil.HasText( encoding ))
                    ec = Encoding.GetEncoding( encoding );
                else
                    ec = getEncoding( res );

                StreamReader reader = new StreamReader( res.GetResponseStream(), ec );
                str = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception exception) {
                logger.Info( "DownloadPage : " + url + ", error : " + exception.Message + "：" );
                throw exception;
            }
            return str;
        }

        private static WebResponse getResponse( String url, String agentInfo ) {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create( url );
            request.UserAgent = agentInfo;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request.GetResponse();
        }

        /// <summary>
        /// 抓取某网址的页面内容，然后使用正则表达式过滤，返回匹配的部分
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="bodyPattern">正则表达式</param>
        /// <returns>返回匹配的部分</returns>
        public static String GetBody( String url, String bodyPattern ) {
            String target = Download( url );
            if (strUtil.HasText( target )) {
                Match match = Regex.Match( target, bodyPattern, RegexOptions.Singleline );
                if (match != null) {
                    target = match.Value;
                }
            }
            return target.Trim();
        }

        private static Encoding getEncoding( WebResponse response ) {
            try {
                String contentType = response.ContentType;
                if (contentType == null) {
                    return _encoding;
                }
                String[] strArray = contentType.ToLower( CultureInfo.InvariantCulture ).Split( new char[] { ';', '=', ' ' } );
                Boolean isFind = false;
                foreach (String item in strArray) {
                    if (item == "charset") {
                        isFind = true;
                    }
                    else if (isFind) {
                        return Encoding.GetEncoding( item );
                    }
                }
            }
            catch (Exception exception) {
                if (((exception is ThreadAbortException) || (exception is StackOverflowException)) || (exception is OutOfMemoryException)) {
                    throw;
                }
            }
            return _encoding;
        }


        /// <summary>
        /// 将页面中的图片下载到本地，返回经过替换的页面内容。图片存储路径 /static/upload/wimg/2009/9/18/20552283166069276.jpg
        /// </summary>
        /// <param name="pageBody">网页内容</param>
        /// <param name="siteUrl">如果图片是相对路径，则需要提供url。如果没有，请传入null</param>
        /// <returns>返回经过替换的页面内容</returns>
        public static String ProcessPic( string pageBody, string siteUrl ) {

            if (strUtil.IsNullOrEmpty( pageBody )) return "";

            MatchCollection matchs = Regex.Matches( pageBody, RegPattern.Img, RegexOptions.Singleline );
            for (int i = 0; i < matchs.Count; i++) {

                string picUrl = matchs[i].Groups[1].Value;

                string newPicUrl = downPic( picUrl.Trim(), siteUrl );
                pageBody = pageBody.Replace( picUrl, newPicUrl );

            }

            return pageBody;
        }


        private static string downPic( string picUrl, string siteUrl ) {


            // 1、修改图片网址为可下载网址
            if (picUrl.ToLower().StartsWith( "http" ) == false) {
                picUrl = strUtil.Join( siteUrl, picUrl );
            }

            return DownloadPic( picUrl );
        }

        /// <summary>
        /// 抓取远程图片，保存到服务器
        /// </summary>
        /// <param name="picUrl">图片网址，必须http开头</param>
        /// <returns>返回从根目录/开始的图片路径</returns>
        public static string DownloadPic( string picUrl ) {
            // 2、图片保存的路径
            string picExt = Path.GetExtension( picUrl ).ToLower();
            if (string.IsNullOrEmpty( picExt )) picExt = ".jpg";

            string absPathDisk = PathHelper.Map( strUtil.Join( sys.Path.DiskUpload, picDirName ) );
            String picName = Img.GetPhotoName( absPathDisk, picExt );
            logger.Info( "picSavePath=" + picName );

            // 3、下载图片
            WebClient client = new WebClient();
            client.Headers.Add( "user-agent", AgentIE6 );
            try {
                String savePath = Path.Combine( absPathDisk, picName );
                client.DownloadFile( picUrl, savePath );
            }
            catch {
                return picUrl;
            }

            // 4、图片的网址
            picName = picName.Replace( "\\", "/" );
            return strUtil.Join( strUtil.Join( sys.Path.Upload, picDirName ), picName );
        }

        private static readonly string picDirName = "wimg";

    }
}

