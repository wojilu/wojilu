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
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.IO;
using System.Collections.Generic;

namespace wojilu {

    /// <summary>
    /// 封装了 web 场合下常用路径和 url 的操作
    /// </summary>
    public class PathHelper {

        /// <summary>
        /// 将相对路径转换为绝对路径
        /// </summary>
        /// <param name="path">必须是相对路径</param>
        /// <returns>返回绝对路径</returns>
        public static String Map( String relativePath ) {
            return PathTool.getInstance().Map( relativePath );
        }

        /// <summary>
        /// 将几个路径拼接为绝对路径(第一个路径必须是绝对路径)
        /// </summary>
        /// <param name="arrPath"></param>
        /// <returns></returns>
        public static String CombineAbs( String[] arrPath ) {
            return PathTool.getInstance().CombineAbs( arrPath );
        }

        private static Boolean IsFirstDomainEqual( String url1, String url2 ) {
            String host1 = new UriBuilder( url1 ).Host;
            String host2 = new UriBuilder( url2 ).Host;
            return host1.Equals( host2 );
        }

        /// <summary>
        /// 从指定的path中去除掉rootPath部分，
        /// </summary>
        /// <param name="rootPath">需要剔除的根路径</param>
        /// <param name="pathFull">被处理的path</param>
        /// <returns>返回多个路径列表(从子命名空间依次到跟命名空间)</returns>
        public static IList GetPathList( String rootPath, String pathFull ) {

            String mypath = strUtil.TrimStart( pathFull, rootPath );

            String[] arrPath;
            if (strUtil.HasText( mypath )) {
                mypath = mypath.TrimStart( '.' );
                arrPath = mypath.Split( '.' );
            }
            else
                arrPath = new String[] { };

            IList list = new ArrayList();
            list.Add( "" );
            String result = "";
            for (int i = 0; i < arrPath.Length; i++) {
                result = result + "." + arrPath[i];
                result = result.TrimStart( '.' );
                result = result.Replace( ".", "/" );
                list.Add( result );
            }

            IList results = new ArrayList();
            for (int i = list.Count - 1; i >= 0; i--) {
                results.Add( list[i].ToString() );
            }

            return results;
        }

        // ------------------------------ Url ---------------------------------------

        /// <summary>
        /// 检查url是否完整(是否以http开头或者以域名开头)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsFullUrl( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return false;
            if (url.Trim().StartsWith( "/" )) return false;
            if (url.Trim().StartsWith( "http://" ) ||
                url.Trim().StartsWith( "https://" )
                ) return true;

            String[] arrItem = url.Split( '/' );
            if (arrItem.Length < 1) return false;

            int dotIndex = arrItem[0].IndexOf( "." );
            if (dotIndex <= 0) return false;


            return hasCommonExt( arrItem[0] ) == false;

        }

        private static readonly List<String> extList = getExtList();

        private static List<String> getExtList() {
            String[] exts = { "htm", "html", "xhtml", "txt",
                                "jpg", "gif", "png", "jpg", "jpeg", "bmp", 
                                "doc", "docx", "ppt", "pptx", "xls", "xlsx", "chm", "pdf",
                                "zip", "7z", "rar", "exe", "dll", 
                                "mov", "wav", "mp3", "rm", "rmvb", "mkv", "avi",
                                "asp", "aspx", "php", "jsp"
                            };

            return new List<String>( exts );
        }

        /// <summary>
        /// 判断网址是否包含常见后缀名，比如 .htm/.html/.aspx/.jpg/.doc/.avi 等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool hasCommonExt( string str ) {

            int dotIndex = str.LastIndexOf( "." );
            String ext = str.Substring( dotIndex + 1, str.Length - dotIndex - 1 );
            return extList.Contains( ext );
        }

        /// <summary>
        /// 判断网址是否包含后缀名，比如 xyzz/ab.htm 包含，my/xyz/dfae3 则不包含
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlHasExt( String url ) {


            if (strUtil.IsNullOrEmpty( url )) return false;
            String[] arrItem = url.Split( '/' );

            String lastPart = arrItem[arrItem.Length - 1];

            return lastPart.IndexOf( "." ) >= 0;
        }

        /// <summary>
        /// 是否是外部链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsOutUrl( String url ) {

            Boolean isFull = IsFullUrl( url );
            if (!isFull) return false;

            String targetHost = new UriBuilder( url ).Host;

            if (targetHost.Equals( SystemInfo.HostNoSubdomain )) return false;
            if (targetHost.IndexOf( SystemInfo.HostNoSubdomain ) >= 0) return false;
            return true;
        }

        private static Boolean IsHostSame( String url1, String url2 ) {
            String host1 = new UriBuilder( url1 ).Host;
            String host2 = new UriBuilder( url2 ).Host;
            return host1.Equals( host2 );
        }

        /// <summary>
        /// 剔除掉 url 的后缀名
        /// </summary>
        /// <param name="rawUrl">原始url</param>
        /// <returns>返回被剔除掉后缀名的 url</returns>
        public static String TrimUrlExt( String rawUrl ) {
            if (strUtil.IsNullOrEmpty( rawUrl )) return rawUrl;
            int dotIndex = rawUrl.IndexOf( "." );
            if (dotIndex < 0) return rawUrl;

            String[] arrItem = rawUrl.Split( '.' );
            String ext = arrItem[arrItem.Length - 1];
            if (ext.IndexOf( '/' ) > 0) return rawUrl;
            return strUtil.TrimEnd( rawUrl, ext ).TrimEnd( '.' );
        }

        /// <summary>
        /// 在不考虑后缀名的情况下，比较两个网址是否相同
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public static Boolean CompareUrlWithoutExt( String url1, String url2 ) {
            if (strUtil.IsNullOrEmpty( url1 ) && strUtil.IsNullOrEmpty( url2 )) return true;
            if (strUtil.IsNullOrEmpty( url1 ) || strUtil.IsNullOrEmpty( url2 )) return false;
            return TrimUrlExt( url1 ) == TrimUrlExt( url2 );
        }

        /// <summary>
        /// bin 的绝对路径
        /// </summary>
        /// <returns></returns>
        public static String GetBinDirectory() {
            if (SystemInfo.IsWeb) {
                return HttpRuntime.BinDirectory;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }


    }
}

