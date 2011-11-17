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

namespace wojilu.ORM {

    /// <summary>
    /// 分页查询的结果集的处理，确定当前页、生成分页栏 html 等 
    /// </summary>
    /// <example>
    /// 只要提供三个参数，就可以获取分页bar<br/>
    /// 1）recordCount (记录总数)  <br/>
    /// 2）pageSize (每页数量)  <br/>
    /// 3）currentPage (当前页码)  <br/>
    /// 然后通过构造函数给 ObjectPage 赋值，然后通过它的 PageBar 属性就可以得到分页栏了。
    /// int recordCount = 302; 
    /// int pageSize = 15;
    /// int currentPage = ctx.route.page;
    /// wojilu.ORM.ObjectPage op = new wojilu.ORM.ObjectPage( recordCount, pageSize, currentPage );
    /// set( "page", op.PageBar );
    /// </example>
    [Serializable]
    public class ObjectPage {

        public ObjectPage() {
        }

        public ObjectPage( int recordCount, int pageSize, int currentPage ) {

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
            int pcount = this.RecordCount / this.getSize();
            int imod = this.RecordCount % this.getSize();
            if (imod > 0) pcount = pcount + 1;

            this.PageCount = pcount;

            return pcount;
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

        /// <summary>
        /// 获取简易形式的分页栏
        /// </summary>
        /// <returns></returns>
        public String GetSimplePageBar() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "<div class=\"turnpage\" style='text-align:center'>" );

            if (getCurrent() > 1) {
                sb.Append( "<a href=\"" );
                appendLink( sb, this.getCurrent() - 1 );
                sb.Append( "\" class=\"pagePrev\">&laquo;" + lang.get( "prevPage" ) + "</a>&nbsp;" );
            }
            if (getCurrent() <= 8) {
                loopPage( sb, 1, getCurrent() );
            }
            else {
                loopPage( sb, 1, 3 );
                sb.Append( "...&nbsp;" );
                if ((PageCount - getCurrent()) < 3) {
                    loopPage( sb, PageCount - 6, getCurrent() );
                }
                else {
                    loopPage( sb, getCurrent() - 3, getCurrent() );
                }
            }
            sb.Append( "<span class=\"currentPageNo\">" );
            sb.Append( getCurrent() );
            sb.Append( "</span>&nbsp;" );
            if ((PageCount - getCurrent()) <= 7) {
                loopPage( sb, getCurrent() + 1, PageCount + 1 );
            }
            else {
                if ((getCurrent() + 3) < 7) {
                    loopPage( sb, getCurrent() + 1, 8 );
                }
                else {
                    loopPage( sb, getCurrent() + 1, getCurrent() + 4 );
                }
                sb.Append( "...&nbsp;" );
                loopPage( sb, PageCount - 1, PageCount + 1 );
            }
            if (getCurrent() < PageCount) {
                sb.Append( "<a href=\"" );
                appendLink( sb, this.getCurrent() - 1 );
                sb.Append( "\" class=\"pageNext\">" + lang.get( "nextPage" ) + "&raquo;</a>&nbsp;" );
            }
            sb.Append( "</div>" );
            return sb.ToString();
        }


        private void setPagerBar() {

            StringBuilder sb = new StringBuilder();

            sb.Append( "<div class=\"turnpage\">" );
            sb.AppendFormat( lang.get( "pageStats" ), RecordCount, this.getSize() );


            sb.Append( "&nbsp;" );
            if (this.getCurrent() > 1) {
                sb.Append( "<a href=\"" );
                appendLink( sb, this.getCurrent() - 1 );
                sb.Append( "\" class=\"pagePrev\">&laquo;" + lang.get( "prevPage" ) + "</a>&nbsp;" );
            }
            if (this.getCurrent() <= 8) {
                loopPage( sb, 1, this.getCurrent() );
            }
            else {
                loopPage( sb, 1, 3 );
                sb.Append( "...&nbsp;" );
                if ((PageCount - this.getCurrent()) < 3) {
                    loopPage( sb, PageCount - 6, this.getCurrent() );
                }
                else {
                    loopPage( sb, this.getCurrent() - 3, this.getCurrent() );
                }
            }
            sb.Append( "<span class=\"currentPageNo\">" );
            sb.Append( this.getCurrent() );
            sb.Append( "</span>&nbsp;" );
            if ((PageCount - this.getCurrent()) <= 7) {
                loopPage( sb, this.getCurrent() + 1, PageCount + 1 );
            }
            else {
                if ((this.getCurrent() + 3) < 7) {
                    loopPage( sb, this.getCurrent() + 1, 8 );
                }
                else {
                    loopPage( sb, this.getCurrent() + 1, this.getCurrent() + 4 );
                }
                sb.Append( "...&nbsp;" );
                loopPage( sb, PageCount - 1, PageCount + 1 );
            }
            if (this.getCurrent() < PageCount) {
                sb.Append( "<a href=\"" );
                appendLink( sb, this.getCurrent() + 1 );
                sb.Append( "\" class=\"pageNext\">" + lang.get( "nextPage" ) + "&raquo;</a>&nbsp;" );
            }
            sb.Append( "</div>" );
            PageBar = sb.ToString();
        }
     

        //----------------------------------------------------------

        private void loopPage( StringBuilder sb, int startNo, int endNo ) {
            for (int i = startNo; i < endNo; i++) {
                sb.Append( "<a href=\"" );
                appendLink( sb, i );
                sb.Append( "\" class=\"pageNo\">" );
                sb.Append( i );
                sb.Append( "</a>&nbsp;" );
            }
        }

        private void appendLink( StringBuilder sb, int pageNo ) {
            String path = this.getPath();
            sb.Append( Link.AppendPage( path, pageNo ) );
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

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// 在已有的翻页链接后，增加额外的分页码。采用query形式："?cp=***"
        /// </summary>
        /// <param name="lnk">已有链接</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="currentPage">当前页</param>
        /// <returns></returns>
        public static String GetPageBarByLink( String lnk, int pageCount, int currentPage ) {
            StringBuilder sb = new StringBuilder();
            sb.Append( "<div class=\"turnpage\">" );

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


    }
}

