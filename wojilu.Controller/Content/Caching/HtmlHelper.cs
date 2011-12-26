using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.ORM;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlHelper ) );

        public static void SetCurrentPost( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }

        public static Boolean IsMakeHtml( MvcContext ctx ) {
            if (ctx.GetItem( "_makeHtml" ) == null) return false;
            Boolean isMakeHtml = (Boolean)ctx.GetItem( "_makeHtml" );
            return isMakeHtml;
        }

        /// <summary>
        /// 生成详细页的html
        /// </summary>
        /// <param name="ctx"></param>
        public static void MakeDetailHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            MakeDetailHtml( ctx, post );
        }

        public static void MakeDetailHtml( MvcContext ctx, ContentPost post ) {
            HtmlHelper.CheckPostDir( post );

            List<String> pagedUrls = new List<String>(); // 翻页的链接
            String addr = strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( post ) );
            String html = makeHtml( addr, pagedUrls );
            file.Write( HtmlHelper.GetPostPath( post ), html );

            if (pagedUrls.Count > 0) {
                makeDetailPages( ctx, post, pagedUrls );
            }
        }

        // 处理需要翻页的详细页
        private static void makeDetailPages( MvcContext ctx, ContentPost post, List<String> pagedUrls ) {
            foreach (String url in pagedUrls) {
                String addrPaged = strUtil.Join( ctx.url.SiteAndAppPath, url );
                String htmlPaged = makeHtml( addrPaged );
                file.Write( HtmlHelper.GetPostPath( post, ObjectPage.GetPageByUrl( url ) ), htmlPaged );
            }
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// 插入或删除文章后，根据此文章更新列表页
        /// </summary>
        /// <param name="ctx"></param>
        public static int MakeListHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return 0;

            int sectionId = post.PageSection.Id;
            ContentApp app = ctx.app.obj as ContentApp;
            int recordCount = 0;

            return MakeListHtml( ctx, app, sectionId, recordCount );
        }

        /// <summary>
        /// 根据 sectionId 生成此区块的列表页
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sectionId"></param>
        public static int MakeListHtml( MvcContext ctx, ContentApp app, int sectionId, int recordCount ) {

            HtmlHelper.CheckListDir( app.Id );

            ContentSetting s = app.GetSettingsObj();

            Link lnk = new Link( ctx );

            String cpLink = lnk.To( new SectionController().Show, sectionId );
            String caLink = lnk.To( new SectionController().Archive, sectionId );

            List<String> lnks = ObjectPage.GetPageLinks( cpLink, caLink, recordCount, 3, s.ListPostPerPage );

            foreach (String url in lnks) {

                int page = ObjectPage.GetPageByUrl( url );

                Boolean isArchive = url.IndexOf( "/Archive" ) > 0;

                String addr = strUtil.Join( ctx.url.SiteAndAppPath, url );
                String html = makeHtml( addr );
                file.Write( HtmlHelper.GetListPath( app.Id, sectionId, page, isArchive ), html );

            }

            return lnks.Count;
        }

        //------------------------------------------------------------------------------------------

        public static void MakeSidebarHtml( MvcContext ctx ) {

            HtmlHelper.CheckSidebarDir();

            Link lnk = new Link( ctx );
            String addr = strUtil.Join( ctx.url.SiteAndAppPath, lnk.To( new SidebarController().Index ) ) + "?ajax=true";

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetSidebarPath( ctx.app.Id ), html );
        }

        //------------------------------------------------------------------------------------------

        public static void MakeAppHtml( MvcContext ctx ) {

            MakeAppHtmlm( ctx, ctx.app.Id );

        }

        public static void MakeAppHtmlm( MvcContext ctx, int appId ) {

            HtmlHelper.CheckAppDir( appId );

            Link lnk = new Link( ctx );
            String addr = strUtil.Join( ctx.url.SiteAndAppPath, lnk.To( new ContentController().Index ) );

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetAppPath( appId ), html );
        }
        //------------------------------------------------------------------------------------------

        public static void DeleteDetailHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            DeleteDetailHtml( post );
        }

        public static void DeleteDetailHtml( ContentPost post ) {
            String filePath = HtmlHelper.GetPostPath( post );
            if (file.Exists( filePath )) file.Delete( filePath );
        }

        //----------------------------------------------------------------------------

        public static String CheckPostDir( ContentPost data ) {

            String dir = GetPostDir( data );

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetPostDir( ContentPost data ) {
            DateTime n = data.Created;
            return PathHelper.Map( string.Format( "/html/{0}/{1}/{2}/", n.Year, n.Month, n.Day ) );
        }

        public static String GetPostPath( ContentPost post ) {
            return Path.Combine( GetPostDir( post ), post.Id + ".html" );
        }

        private static string GetPostPath( ContentPost post, int page ) {
            return Path.Combine( GetPostDir( post ), post.Id + "_" + page + ".html" );
        }

        //----------------------------------------------------------------------------

        public static String CheckListDir( int appId ) {

            String dir = GetListDir();

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetListDir() {
            return PathHelper.Map( "/html/list/" );
        }

        // 如果是普通的 /list/38.html, /list/38_5.html
        // 如果是存档的 /list/38_a_1.html, /list/38_a_5.html
        public static String GetListPath( int appId, int sectionId, int page, Boolean isArchive ) {

            if (isArchive) {

                if (page == 1) {
                    return Path.Combine( GetListDir(), sectionId + "_a.html" );
                }
                else {
                    return Path.Combine( GetListDir(), sectionId + "_a_" + page + ".html" );
                }
            }
            else {
                if (page == 1) {
                    return Path.Combine( GetListDir(), sectionId + ".html" );
                }
                else {
                    return Path.Combine( GetListDir(), sectionId + "_" + page + ".html" );
                }
            }
        }

        //public static String GetListLink( int sectionId ) {
        //    return string.Format( "/html/list/{0}.html", sectionId );
        //}

        //----------------------------------------------------------------------------

        public static String CheckSidebarDir() {
            String dir = GetSidebarDir();

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static string GetSidebarDir() {
            return PathHelper.Map( "/html/sidebar/" );
        }

        public static string GetSidebarPath( int appId ) {
            return Path.Combine( GetSidebarDir(), appId + ".html" );
        }

        //----------------------------------------------------------------------------

        public static String CheckAppDir( int appId ) {

            String dir = GetAppDir( appId );

            if (dir == null) return null;

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        private static string GetAppDir( int appId ) {

            ContentApp app = ContentApp.findById( appId );
            if (app == null) throw new Exception( "app not found: Content.AppId=" + appId );

            String staticDir = app.GetSettingsObj().StaticDir;
            if (strUtil.IsNullOrEmpty( staticDir )) {

                logger.Error( "app(id=" + appId + ")尚未配置静态目录，使用默认目录：cms+" + app.Id );

                staticDir = "cms" + app.Id;
            }

            return PathHelper.Map( "/" + staticDir + "/" );
        }

        public static string GetAppPath( int appId ) {

            return Path.Combine( GetAppDir( appId ), "default.html" );

        }

        //----------------------------------------------------------------------------

        private static String makeHtml( String addr ) {
            return makeHtml( addr, new List<String>() );
        }

        private static String makeHtml( String addr, List<String> nextMakeUrls ) {
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


    }

}
