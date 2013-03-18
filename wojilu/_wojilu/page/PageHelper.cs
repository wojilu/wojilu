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
using System.Text;
using System.Web;
using System.Collections.Generic;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Routes;

namespace wojilu {

    /// <summary>
    /// 分页查询的结果集的处理，确定当前页、生成分页栏 html 、生成链接、追加页码等 
    /// </summary>
    /// <example>
    /// 静态方法：PageHelper 提供了多个静态方法，请看方法的注释。<br/>
    /// 实例方法：只要提供三个参数，就可以获取分页bar<br/>
    /// 1）recordCount (记录总数)  <br/>
    /// 2）pageSize (每页数量)  <br/>
    /// 3）currentPage (当前页码)  <br/>
    /// 然后通过构造函数给 PageHelper 赋值，然后通过它的 PageBar 属性就可以得到分页栏了。
    /// int recordCount = 302; 
    /// int pageSize = 15;
    /// int currentPage = ctx.route.page;
    /// wojilu.PageHelper op = new wojilu.PageHelper( recordCount, pageSize, currentPage );
    /// set( "page", op.PageBar );
    /// </example>
    [Serializable]
    public class PageHelper {

        public PageHelper() {
        }

        public PageHelper( int recordCount, int pageSize, int currentPage ) {

            this.setSize( pageSize );
            this.RecordCount = recordCount;
            this.computePageCount();

            this.setCurrent( currentPage );
            this.resetCurrent();
        }

        private String _bar;
        private int _current = 0;
        private int _pagesize = 20;

        public String Condition;
        public int PageCount;
        public int RecordCount;

        public int getCurrent() {
            if (_current <= 0) return CurrentRequest.getCurrentPage();
            return _current;
        }
        public void setCurrent( int page ) { _current = page; }

        public int getSize() { return _pagesize; }
        public void setSize( int size ) { _pagesize = size; }

        public int computePageCount() {
            this.PageCount = GetPageCount( this.RecordCount, this.getSize() );
            return this.PageCount;
        }

        public void resetCurrent() {
            if (this.getCurrent() <= 1) {
                this.setCurrent( 1 );
            }
            else if (this.getCurrent() > this.PageCount) {
                this.setCurrent( this.PageCount );
            }
        }

        /// <summary>
        /// 数据列表的分页栏(包括html)，你也可以根据 PageCount/RecordCount/getCurrent()/getSize() 自定义。
        /// </summary>
        public String PageBar {
            get {
                if (strUtil.IsNullOrEmpty( _bar )) {
                    setPagerBar();
                }
                return _bar;
            }
            set { _bar = value; }
        }

        public String GetSimplePageBar() {
            return GetSimplePageBar( this.getPath(), this.getCurrent(), this.PageCount );
        }

        private static void appendLinkPrivate( String url, StringBuilder sb, int currentPage, Boolean isHtml ) {
            if (isHtml) {
                appendHtmlLink( url, sb, currentPage );
            }
            else {
                appendLink( url, sb, currentPage );
            }
        }

        private static void loopPrivate( String url, StringBuilder sb, int startNo, int endNo, Boolean isHtml ) {
            if (isHtml) {
                loopHtmlPage( url, sb, startNo, endNo );
            }
            else {
                loopPage( url, sb, startNo, endNo );
            }
        }

        private void setPagerBar() {

            String url = this.getPath();

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat( "<div class=\"turnpage\" data-records={0} data-pages={1} data-page-size={2}>", RecordCount, PageCount, this.getSize() );
            sb.AppendFormat( lang.get( "pageStats" ), RecordCount, this.getSize() );


            sb.Append( "&nbsp;" );
            if (this.getCurrent() > 1) {
                sb.Append( "<a href=\"" );
                appendLink( url, sb, this.getCurrent() - 1 );
                sb.Append( "\" class=\"pagePrev\">&laquo;" + lang.get( "prevPage" ) + "</a>&nbsp;" );
            }
            if (this.getCurrent() <= 8) {
                loopPage( url, sb, 1, this.getCurrent() );
            }
            else {
                loopPage( url, sb, 1, 3 );
                sb.Append( "...&nbsp;" );
                if ((PageCount - this.getCurrent()) < 3) {
                    loopPage( url, sb, PageCount - 6, this.getCurrent() );
                }
                else {
                    loopPage( url, sb, this.getCurrent() - 3, this.getCurrent() );
                }
            }
            sb.Append( "<span class=\"currentPageNo\">" );
            sb.Append( this.getCurrent() );
            sb.Append( "</span>&nbsp;" );
            if ((PageCount - this.getCurrent()) <= 7) {
                loopPage( url, sb, this.getCurrent() + 1, PageCount + 1 );
            }
            else {
                if ((this.getCurrent() + 3) < 7) {
                    loopPage( url, sb, this.getCurrent() + 1, 8 );
                }
                else {
                    loopPage( url, sb, this.getCurrent() + 1, this.getCurrent() + 4 );
                }
                sb.Append( "...&nbsp;" );
                loopPage( url, sb, PageCount - 1, PageCount + 1 );
            }
            if (this.getCurrent() < PageCount) {
                sb.Append( "<a href=\"" );
                appendLink( url, sb, this.getCurrent() + 1 );
                sb.Append( "\" class=\"pageNext\">" + lang.get( "nextPage" ) + "&raquo;</a>&nbsp;" );
            }
            sb.Append( "</div>" );
            PageBar = sb.ToString();
        }

        //----------------------------------------------------------

        private static void loopPage( String url, StringBuilder sb, int startNo, int endNo ) {
            for (int i = startNo; i < endNo; i++) {
                sb.Append( "<a href=\"" );
                appendLink( url, sb, i );
                sb.Append( "\" class=\"pageNo\">" );
                sb.Append( i );
                sb.Append( "</a>&nbsp;" );
            }
        }

        private static void loopHtmlPage( String url, StringBuilder sb, int startNo, int endNo ) {
            for (int i = startNo; i < endNo; i++) {
                sb.Append( "<a href=\"" );
                appendHtmlLink( url, sb, i );
                sb.Append( "\" class=\"pageNo\">" );
                sb.Append( i );
                sb.Append( "</a>&nbsp;" );
            }
        }

        private static void appendLink( String url, StringBuilder sb, int pageNo ) {
            sb.Append( AppendNo( url, pageNo ) );
        }

        private static void appendHtmlLink( String url, StringBuilder sb, int pageNo ) {
            sb.Append( AppendHtmlNo( url, pageNo ) );
        }

        //----------------------------------------------------------

        private String getPath() {

            if (_path != null) return _path;

            String routePath = Route.getRoutePath();
            String path = (routePath == null ? CurrentRequest.getRawUrl() : routePath);
            _path = path;
            return path;
        }


        private String _path;

        public override String ToString() {
            setPagerBar();
            return PageBar;
        }

        //-----------------------------以下是静态方法------------------------------------------------------


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
                return url + MvcConfig.Instance.UrlSeparator + "p" + pageNumber + ext + query;
        }

        private static String getPageNumberLabel( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return "";

            String[] arr = url.Split( RouteTool.Separator );
            if (arr.Length < 2) return "";

            String end = arr[arr.Length - 1];

            if (end.StartsWith( "p" ) && cvt.IsInt( end.Substring( 1 ) )) {
                return MvcConfig.Instance.UrlSeparator + end;
            }

            return "";
        }

        private static String getExt( String url ) {
            return MvcConfig.Instance.UrlExt;
        }

        public static int GetPageNoByUrl( String url ) {
            if (strUtil.IsNullOrEmpty( url )) return 1;

            String[] arr = url.Split( '/' );

            String[] arrItem = arr[arr.Length - 1].Split( '.' );

            String strPage = arrItem[0];

            if (strPage.StartsWith( "p" ) == false) return 1;
            String strNo = strPage.TrimStart( 'p' );
            if (cvt.IsInt( strNo ) == false) return 1;

            return cvt.ToInt( strNo );
        }

        public static int GetPageCount( int recordCount, int pageSize ) {
            int pcount = recordCount / pageSize;
            int imod = recordCount % pageSize;
            if (imod > 0) pcount = pcount + 1;

            return pcount;
        }

        /// <summary>
        /// 获取简易形式的分页栏
        /// </summary>
        /// <param name="url"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static String GetSimplePageBar( String url, int currentPage, int pageCount ) {
            return GetSimplePageBar( url, currentPage, pageCount, false );
        }

        /// <summary>
        /// 获取简易形式的分页栏
        /// </summary>
        /// <returns></returns>
        public static String GetSimplePageBar( String url, int currentPage, int pageCount, Boolean isHtml ) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "<div class=\"turnpage\" data-pages={0} style=\"text-align:center\">", pageCount );

            if (currentPage > 1) {
                sb.Append( "<a href=\"" );
                appendLinkPrivate( url, sb, currentPage - 1, isHtml );
                sb.Append( "\" class=\"pagePrev\">&laquo;" + lang.get( "prevPage" ) + "</a>&nbsp;" );
            }
            if (currentPage <= 8) {
                loopPrivate( url, sb, 1, currentPage, isHtml );
            }
            else {
                loopPrivate( url, sb, 1, 3, isHtml );
                sb.Append( "...&nbsp;" );
                if ((pageCount - currentPage) < 3) {
                    loopPrivate( url, sb, pageCount - 6, currentPage, isHtml );
                }
                else {
                    loopPrivate( url, sb, currentPage - 3, currentPage, isHtml );
                }
            }
            sb.Append( "<span class=\"currentPageNo\">" );
            sb.Append( currentPage );
            sb.Append( "</span>&nbsp;" );
            if ((pageCount - currentPage) <= 7) {
                loopPrivate( url, sb, currentPage + 1, pageCount + 1, isHtml );
            }
            else {
                if ((currentPage + 3) < 7) {
                    loopPrivate( url, sb, currentPage + 1, 8, isHtml );
                }
                else {
                    loopPrivate( url, sb, currentPage + 1, currentPage + 4, isHtml );
                }
                sb.Append( "...&nbsp;" );
                loopPrivate( url, sb, pageCount - 1, pageCount + 1, isHtml );
            }
            if (currentPage < pageCount) {
                sb.Append( "<a href=\"" );
                appendLinkPrivate( url, sb, currentPage + 1, isHtml );
                sb.Append( "\" class=\"pageNext\">" + lang.get( "nextPage" ) + "&raquo;</a>&nbsp;" );
            }
            sb.Append( "</div>" );
            return sb.ToString();
        }


        /// <summary>
        /// 在已有的翻页链接后，增加额外的分页码。采用query形式："?cp=***"
        /// </summary>
        /// <param name="lnk">已有链接</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="currentPage">当前页</param>
        /// <returns></returns>
        public static String GetPageBarByLink( String lnk, int pageCount, int currentPage ) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "<div class=\"turnpage\" data-pages={0}>", pageCount );

            if (currentPage > 1) {
                sb.Append( "<a href=\"" );
                sb.Append( lnk );
                int pindex = currentPage - 1;
                if (pindex > 1) {
                    sb.Append( "?cp=" );
                    sb.Append( pindex );
                }

                sb.Append( "\"  class=\"pagePrev\">&laquo;" );
                sb.Append( wojilu.lang.get( "prevPage" ) );
                sb.Append( "</a> " );
            }

            for (int i = 1; i < pageCount + 1; i++) {

                if (i == currentPage) {
                    sb.Append( "<span class=\"currentPageNo\">" );
                    sb.Append( i );
                    sb.Append( "</span> " );
                }
                else {

                    sb.Append( "<a href=\"" );

                    sb.Append( lnk );
                    if (i > 1) {
                        sb.Append( "?cp=" );
                        sb.Append( i );
                    }

                    sb.Append( "\"  class=\"pageNo\">" );
                    sb.Append( i );
                    sb.Append( "</a> " );

                }
            }

            if (currentPage < pageCount) {

                sb.Append( "<a href=\"" );

                sb.Append( lnk );
                sb.Append( "?cp=" );
                sb.Append( currentPage + 1 );

                sb.Append( "\"  class=\"pageNext\">" );
                sb.Append( wojilu.lang.get( "nextPage" ) );
                sb.Append( "&raquo;</a> " );
            }

            sb.Append( "</div>" );

            return sb.ToString();
        }

        public static List<String> GetPageLinks( String listLink, int recordCount, int pageSize ) {

            List<String> list = new List<String>();

            int totalPages = PageHelper.GetPageCount( recordCount, pageSize );
            for (int i = 1; i < totalPages + 1; i++) {
                String url = AppendNo( listLink, i );
                list.Add( url );
            }

            return list;
        }



    }
}

