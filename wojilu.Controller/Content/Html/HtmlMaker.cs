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
            return ObjectContext.Create<DetailMaker>();
        }

        public static HomeMaker GetHome() {
            return ObjectContext.Create<HomeMaker>();
        }

        public static ListMaker GetList() {
            return ObjectContext.Create<ListMaker>();
        }

        public static RecentMaker GetRecent() {
            return ObjectContext.Create<RecentMaker>();
        }

        public static SidebarMaker GetSidebar() {
            return ObjectContext.Create<SidebarMaker>();
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
        /// 循环生成列表页的所有页面
        /// </summary>
        /// <param name="sectionIdOrAppId">列表Id或AppId</param>
        /// <param name="lnkNormal">列表首页</param>
        /// <param name="recordCount">总数据量</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        protected int makeHtmlLoopAll( int sectionIdOrAppId, String lnkNormal, int recordCount, int pageSize ) {

            // 获得所有分页的url(aspx动态页面)
            List<String> lnks = PageHelper.GetPageLinks( lnkNormal, recordCount, pageSize );

            foreach (String url in lnks) {

                makeListPageSingle( sectionIdOrAppId, url );

            }
            return lnks.Count;
        }

        private void makeListPageSingle( int sectionIdOrAppId, String url ) {
            int pageNo = PageHelper.GetPageNoByUrl( url ); // 当前页码
            String addr = url;
            String html = makeHtml( addr );
            String htmlPath = GetListPath( sectionIdOrAppId, pageNo );
            file.Write( htmlPath, html );
            logger.Info( "make html done=>" + htmlPath );
        }


        //-----------------------------------------------------------

        protected string GetListPath( int sectionIdOrAppId, int pageNo ) {

            if (pageNo == 1) {
                return Path.Combine( GetDir(), sectionIdOrAppId + ".html" );
            }
            else {
                return Path.Combine( GetDir(), sectionIdOrAppId + "_" + pageNo + ".html" );
            }

        }


    }

}
