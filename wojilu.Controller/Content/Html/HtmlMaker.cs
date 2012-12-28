/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Apps.Content.Domain;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Htmls {

    public abstract class HtmlMaker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlMaker ) );

        public static DetailMaker GetDetail() {
            return (DetailMaker)ObjectContext.CreateObject( typeof( DetailMaker ) );
        }

        public static HomeMaker GetHome() {
            return (HomeMaker)ObjectContext.CreateObject( typeof( HomeMaker ) );
        }

        public static ListMaker GetList() {
            return (ListMaker)ObjectContext.CreateObject( typeof( ListMaker ) );
        }

        public static RecentMaker GetRecent() {
            return (RecentMaker)ObjectContext.CreateObject( typeof( RecentMaker ) );
        }

        public static SidebarMaker GetSidebar() {
            return (SidebarMaker)ObjectContext.CreateObject( typeof( SidebarMaker ) );
        }

        protected abstract String GetDir();

        protected String makeHtml( String addr ) {
            return makeHtml( addr, new List<String>() );
        }

        protected String makeHtml( String addr, List<String> nextMakeUrls ) {
            StringWriter sw = new StringWriter();
            IWebContext webContext = MockWebContext.New( addr, sw );
            MvcContext ctx = new MvcContext( webContext );
            ctx.SetItem( "_makeHtml", true );

            new CoreHandler().ProcessRequest( ctx );

            List<String> relativeUrls = ctx.GetItem( "_relativeUrls" ) as List<String>;
            if (relativeUrls != null && relativeUrls.Count > 0) {
                nextMakeUrls.AddRange( relativeUrls );
            }

            return sw.ToString();
        }


        protected String CheckDir() {

            String dir = GetDir();

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;

        }

        /// <summary>
        /// 循环生成列表页的缓存页。存档页只生成最近一页
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="appId"></param>
        /// <param name="lnkNormal"></param>
        /// <param name="lnkArchive"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected int makeHtmlLoopCache( int recordCount, int appId, String lnkNormal, String lnkArchive, int pageSize ) {

            // 缓存页数
            int pageWidth = ContentSetting.ListPageWidth;

            // 获得所有分页的url(aspx动态页面)
            List<String> lnks = PageHelper.GetPageLinks( lnkNormal, lnkArchive, recordCount, pageWidth, pageSize );

            // 所有的缓存页
            for (int i = 0; i < pageWidth; i++) {
                if (i >= lnks.Count) break;
                makeListPageSingle( appId, lnks[i] );
            }

            // 最近的存档页
            if (lnks.Count > pageWidth) {
                makeListPageSingle( appId, lnks[lnks.Count - 1] );
            }

            return lnks.Count;

        }


        /// <summary>
        /// 循环生成列表页的所有页面，包括存档页和缓存页
        /// </summary>
        /// <param name="recordCount">总数据量</param>
        /// <param name="appId">列表的Id</param>
        /// <param name="lnkNormal">缓存页</param>
        /// <param name="lnkArchive">存档页</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        protected int makeHtmlLoopAll( int recordCount, int appId, String lnkNormal, String lnkArchive, int pageSize ) {

            // 缓存页数
            int pageWidth = ContentSetting.ListPageWidth;

            // 获得所有分页的url(aspx动态页面)
            List<String> lnks = PageHelper.GetPageLinks( lnkNormal, lnkArchive, recordCount, pageWidth, pageSize );

            foreach (String url in lnks) {

                makeListPageSingle( appId, url );

            }
            return lnks.Count;
        }

        private void makeListPageSingle( int appId, String url ) {
            int pageNo = PageHelper.GetPageNoByUrl( url ); // 当前页码

            Boolean isArchive = url.IndexOf( "Archive" ) > 0;

            String addr = url;
            String html = makeHtml( addr );
            String htmlPath = GetListPath( appId, pageNo, isArchive );
            file.Write( htmlPath, html );
            logger.Info( "make html done=>" + htmlPath );
        }


        //-----------------------------------------------------------

        protected string GetListPath( int appId, int pageNo, bool isArchive ) {

            if (isArchive == false) {
                return GetListPath( appId, pageNo );
            }

            return GetListPathArchive( appId, pageNo );
        }

        protected String GetListPath( int appId, int page ) {

            if (page == 1) {
                return Path.Combine( GetDir(), appId + ".html" );
            }
            else {
                return Path.Combine( GetDir(), appId + "_" + page + ".html" );
            }

        }

        protected String GetListPathArchive( int appId, int page ) {

            if (page == 1) {
                return Path.Combine( GetDir(), appId + "_a.html" );
            }
            else {
                return Path.Combine( GetDir(), appId + "_a_" + page + ".html" );
            }

        }

    }

}
