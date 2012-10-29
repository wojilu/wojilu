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

namespace wojilu {

    public class PageHelper {


        /// <summary>
        /// 计算分页的页码
        /// </summary>
        /// <param name="count">数据量</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>总计多少页</returns>
        public static int GetPageIndex( int count, int pageSize ) {

            if (count == 0) return 1;

            int mod = count % pageSize;
            if (mod == 0) return count / pageSize;

            return count / pageSize + 1;
        }

        /// <summary>
        /// "/html/2010/11/22/195.html" => "/html/2010/11/22/195_2.html"
        /// </summary>
        /// <param name="srcUrl"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static String AppendHtmlNo( String srcUrl, int pageNumber ) {

            if (strUtil.IsNullOrEmpty( srcUrl )) return srcUrl;
            if (pageNumber <= 1) return srcUrl;

            int lastDot = srcUrl.LastIndexOf( '.' );
            if (lastDot <= 0 || lastDot >= srcUrl.Length - 1) return srcUrl;

            String path = srcUrl.Substring( 0, lastDot );
            String ext = srcUrl.Substring( lastDot + 1, srcUrl.Length - lastDot - 1 );

            return path + "_" + pageNumber + "." + ext;
        }

        /// <summary>
        /// 在已有网址url后加上页码 Post/List.aspx=>Post/List/p6.aspx
        /// </summary>
        /// <param name="srcUrl">原始网址</param>
        /// <param name="pageNumber">页码</param>
        /// <returns></returns>
        public static String AppendNo( String srcUrl, int pageNumber ) {

            if (strUtil.IsNullOrEmpty( srcUrl )) return srcUrl;

            String url = srcUrl;

            // 查询字符串
            int qIndex = url.IndexOf( "?" );
            String query = "";
            if (qIndex > 0) {
                url = srcUrl.Substring( 0, qIndex );
                query = srcUrl.Substring( qIndex, (srcUrl.Length - qIndex) );
            }

            String ext = getExt( url );

            // 有扩展名
            if (ext.Length > 0) {
                url = strUtil.TrimEnd( url, ext );
            }

            String originalPage = getPageNumberLabel( url );
            if (originalPage.Length > 0) url = strUtil.TrimEnd( url, originalPage );

            if (pageNumber <= 1)
                return url + ext + query;
            else
                return url + "/p" + pageNumber + ext + query;
        }

        private static String getPageNumberLabel( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return "";

            char separator = '/';
            String[] arr = url.Split( separator );
            if (arr.Length < 2) return "";

            String end = arr[arr.Length - 1];

            if (end.StartsWith( "p" ) && cvt.IsInt( end.Substring( 1 ) )) {
                return separator + end;
            }

            return "";
        }

        private static String getExt( String url ) {
            int dotIndex = url.LastIndexOf( "." );
            int slashIndex = url.LastIndexOf( "/" );
            if (dotIndex < 0) return "";
            if (dotIndex < slashIndex) return "";
            return url.Substring( dotIndex, (url.Length - dotIndex) );
        }



    }

}
