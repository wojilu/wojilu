using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching {


    public abstract class HtmlMakerBase {

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


        protected String CheckDir( int appId ) {

            String dir = GetDir();

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="recordCount">总数据量</param>
        /// <param name="appId">列表的Id</param>
        /// <param name="cpLink">缓存页</param>
        /// <param name="caLink">存档页</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        protected int makeHtmlLoop( MvcContext ctx, int recordCount, int appId,
            String cpLink, String caLink, int pageSize ) {

            // 获得所有分页的url(aspx动态页面)
            List<String> lnks = PageHelper.GetPageLinks( cpLink, caLink, recordCount, 3, pageSize );

            foreach (String url in lnks) {

                int pageNo = PageHelper.GetPageNoByUrl( url ); // 当前页码

                Boolean isArchive = url.IndexOf( "Archive" ) > 0;

                String addr = strUtil.Join( ctx.url.SiteAndAppPath, url );
                String html = makeHtml( addr );
                file.Write( GetListPath( appId, pageNo, isArchive ), html );

            }
            return lnks.Count;
        }


        //-----------------------------------------------------------

        protected string GetListPath( int appId, int pageNo, bool isArchive ) {

            if (isArchive==false) {
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
