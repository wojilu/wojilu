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
using System.Web;
using System.Text.RegularExpressions;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web;

namespace wojilu {

    /// <summary>
    /// 封装了 ORM 分页查询的结果集
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    [Serializable]
    public class DataPage<T> : IPageList {

        public DataPage() {
        }

        public DataPage( IPageList list ) {
            this.Current = list.Current;
            this.Size = list.Size;
            this.PageCount = list.PageCount;
            this.PageBar = list.PageBar;
            this.RecordCount = list.RecordCount;
            this.Results = db.getResults<T>( list.Results );
        }

        public void CopyStats( IPageList list ) {
            this.Current = list.Current;
            this.Size = list.Size;
            this.PageCount = list.PageCount;
            this.PageBar = list.PageBar;
            this.RecordCount = list.RecordCount;
        }

        private int _current;
        private String _pageBar;
        private int _pageCount;
        private int _recordCount;
        private List<T> _results;
        private int _size;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Current {
            get { return _current; }
            set { _current = value; }
        }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int Size {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// 风转过的 html 分页栏(也可以自定义)
        /// </summary>
        public String PageBar {
            get { return _pageBar; }
            set { _pageBar = value; }
        }

        /// <summary>
        /// 总页码
        /// </summary>
        public int PageCount {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// 查询结果：对象的列表
        /// </summary>
        public List<T> Results {
            get { return _results; }
            set { _results = value; }
        }

        /// <summary>
        /// 返回空的分页结果集
        /// </summary>
        /// <returns></returns>
        public static DataPage<T> GetEmpty() {
            DataPage<T> p = new DataPage<T>();
            p.Results = new List<T>();
            p.Current = 1;
            p.RecordCount = 0;
            return p;
        }

        public DataPage<TData> Convert<TData>( List<TData> list ) {
            DataPage<TData> p = new DataPage<TData>();
            p.CopyStats( this );
            p.Results = list;
            return p;
        }

        System.Collections.IList IPageList.Results {
            get { return new System.Collections.ArrayList(); }
            set {
            }
        }

        //public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount, Boolean isHtml ) {
        //    return GetRecentPage( recentLink, archiveLink, recentPageCount, recentPageCount, isHtml );
        //}

        ///// <summary>
        ///// 最近数据列表的分页栏
        ///// </summary>
        ///// <param name="recentLink">最近数据列表网址(不带页码)</param>
        ///// <param name="archiveLink">存档数据列表网址(不带页码)</param>
        ///// <param name="recentPageCount">最近数据列表需要展示的页数</param>
        ///// <returns></returns>
        //public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount ) {
        //    return GetRecentPage( recentLink, archiveLink, recentPageCount, recentPageCount );
        //}

        ///// <summary>
        ///// 最近数据列表的分页栏
        ///// </summary>
        ///// <param name="recentLink">最近数据列表网址(不带页码)</param>
        ///// <param name="archiveLink">存档数据列表网址(不带页码)</param>
        ///// <param name="recentPageCount">最近数据列表需要展示的页数</param>
        ///// <param name="pageWidth">分页栏宽度</param>
        ///// <returns></returns>
        //public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount, int pageWidth ) {
        //    return GetRecentPage( recentLink, archiveLink, recentPageCount, pageWidth, false );
        //}


        ///// <summary>
        ///// 最近数据列表的分页栏
        ///// </summary>
        ///// <param name="recentLink">最近数据列表网址(不带页码)</param>
        ///// <param name="archiveLink">存档数据列表网址(不带页码)</param>
        ///// <param name="recentPageCount">最近数据列表需要展示的页数</param>
        ///// <param name="pageWidth">分页栏宽度</param>
        ///// <param name="isHtml">是否html制作请求</param>
        ///// <returns></returns>
        //public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount, int pageWidth, Boolean isHtml ) {

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append( "<div class=\"turnpage\">" );

        //    // prev
        //    if (this.Current > 1) {
        //        sb.AppendFormat( "<a href=\"{0}\">&lt;{1}</a>&nbsp;", appendPage( recentLink, this.Current - 1, isHtml ), lang.get( "prevPage" ) );
        //    }

        //    // page number
        //    int startNo;
        //    int pp = this.Current / pageWidth;
        //    if (this.Current % pageWidth == 0) {
        //        startNo = (pp - 1) * pageWidth + 1;
        //    }
        //    else {
        //        startNo = pp * pageWidth + 1;
        //    }

        //    if (startNo > this.PageCount) startNo = 1;

        //    int endNo = startNo + pageWidth - 1;
        //    if (endNo >= this.PageCount) endNo = this.PageCount;
        //    if (endNo > recentPageCount) endNo = recentPageCount;

        //    for (int i = startNo; i <= endNo; i++) {
        //        String pstyle = this.Current == i ? "currentPageNo" : "pageNo";
        //        sb.AppendFormat( "<a href=\"{0}\" class=\"{1}\">{2}</a>&nbsp;", appendPage( recentLink, i, isHtml ), pstyle, i );
        //    }

        //    // next
        //    if (this.Current + 1 > this.PageCount) {
        //    }
        //    else if (this.Current + 1 > recentPageCount) {
        //        int nextPage = this.PageCount - recentPageCount;
        //        sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>&nbsp;", appendPage( archiveLink, nextPage, isHtml ), lang.get( "nextPage" ) );
        //    }
        //    else {
        //        sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>&nbsp;", appendPage( recentLink, this.Current + 1, isHtml ), lang.get( "nextPage" ) );
        //    }

        //    sb.AppendFormat( "<span class=\"pageCount\">" + lang.get( "pageCount" ) + "</span>", this.PageCount );

        //    sb.Append( "</div>" );

        //    return sb.ToString();
        //}

        private String appendPage( String url, int pageNumber, Boolean isHtml ) {

            if (isHtml) {
                return PageHelper.AppendHtmlNo( url, pageNumber );
            }
            else {
                return PageHelper.AppendNo( url, pageNumber );
            }

        }

        public String GetPageBar( string cpLink, bool isMakeHtml ) {

            if (isMakeHtml == false) return this.PageBar;

            return PageHelper.GetSimplePageBar( cpLink, this.Current, this.PageCount, isMakeHtml );
        }


        public static DataPage<T> GetPage( List<T> list, int pageSize ) {

            PageHelper op = new PageHelper();
            if (pageSize <= 0) pageSize = 20;
            op.setSize( pageSize );
            op.RecordCount = list.Count;
            // 计算页数
            op.computePageCount();

            // 矫正当前页码
            int currentPageNumber = CurrentRequest.getCurrentPage();
            op.setCurrent( currentPageNumber );
            op.resetCurrent();
            currentPageNumber = op.getCurrent();

            // 得到结果集
            List<T> results = new List<T>();
            int start = (currentPageNumber - 1) * pageSize;
            int count = 1;
            for (int i = start; i < list.Count; i++) {
                if (count > pageSize) break;
                results.Add( list[i] );
                count++;
            }

            // 填充分页数据
            DataPage<T> page = new DataPage<T>();
            page.Results = results;
            page.Current = currentPageNumber;
            page.Size = pageSize;
            page.RecordCount = list.Count;

            page.PageCount = op.PageCount;
            page.PageBar = op.PageBar;

            return page;
        }




    }


}
