using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.ORM;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlHelper ) );

        public static void SetCurrentPost( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }

        public static ContentPost GetCurrentPost( MvcContext ctx ) {
            return ctx.GetItem( "_currentContentPost" ) as ContentPost;
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
                file.Write( HtmlHelper.GetPostPath( post, PageHelper.GetPageNoByUrl( url ) ), htmlPaged );
            }
        }

        //------------------------------------------------------------------------------------------

        public static void MakeSidebarHtml( MvcContext ctx ) {

            HtmlHelper.CheckSidebarDir();

            String addr = strUtil.Join( ctx.url.SiteAndAppPath, ctx.link.To( new SidebarController().Index ) ) + "?ajax=true";

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetSidebarPath( ctx.app.Id ), html );
        }

        //------------------------------------------------------------------------------------------

        public static void MakeAppHtml( MvcContext ctx ) {

            MakeAppHtmlm( ctx, ctx.app.Id );

        }

        public static void MakeAppHtmlm( MvcContext ctx, int appId ) {

            HtmlHelper.CheckAppDir( appId );

            String addr = strUtil.Join( ctx.url.SiteAndAppPath, ctx.link.To( new ContentController().Index ) );

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetAppStaticHomePageAbs( appId ), html );
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

            String dir = GetAppDirAbs( appId );

            if (dir == null) return null;

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        private static string GetAppDirAbs( int appId ) {

            String staticDir = GetlAppDirName( appId );

            return PathHelper.Map( "/" + staticDir + "/" );
        }

        public static String GetlAppDirName( int appId ) {

            ContentApp app = ContentApp.findById( appId );
            if (app == null) throw new Exception( "app not found: Content.AppId=" + appId );

            return HtmlLink.GetStaticDir( app );
        }

        public static string GetAppStaticHomePageAbs( int appId ) {

            return Path.Combine( GetAppDirAbs( appId ), "default.html" );

        }

        //----------------------------------------------------------------------------

        public static bool IsHtmlDirError( String htmlDir, Result errors ) {

            if (strUtil.HasText( htmlDir )) {


                if (htmlDir.Length > 50) {
                    errors.Add( "目录名称不能超过50个字符" );
                    return true;
                }

                if (isReservedKeyContains( htmlDir )) {
                    errors.Add( "目录名称是保留词，请换一个" );
                    return true;
                }

                if (isHtmlDirUsed( htmlDir )) {
                    errors.Add( "目录名称已被使用，请换一个" );
                    return true;
                }

            }

            return false;
        }

        private static bool isHtmlDirUsed( string dirName ) {

            List<ContentApp> appList = ContentApp.find( "OwnerType=:otype" )
                .set( "otype", typeof( Site ).FullName )
                .list();

            foreach (ContentApp app in appList) {

                if (dirName.Equals( app.GetSettingsObj().StaticDir )) return true;
            }

            return false;
        }

        private static bool isReservedKeyContains( string dirName ) {

            if (dirName == null) return false;

            String[] arrKeys = new String[] { "framework", "bin", "html", "static" };

            return new List<String>( arrKeys ).Contains( dirName.ToLower() );
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
