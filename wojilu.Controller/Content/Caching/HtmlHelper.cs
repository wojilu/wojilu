using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlHelper ) );

        public static void SetCurrentPost( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }


        // 生成静态 html 页面
        public static void MakeDetailHtml( wojilu.Web.Context.MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            HtmlHelper.CheckPostDir( post );

            String addr = strUtil.Join( ctx.url.SiteAndAppPath, alink.ToAppData( post ) );
            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetPostPath( post ), html );
        }

        public static void MakeListHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            HtmlHelper.CheckListDir( post );

            Link lnk = new Link( ctx );
            String addr = strUtil.Join( ctx.url.SiteAndAppPath, lnk.To( new SectionController().Show, post.PageSection.Id ) );

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetListPath( post ), html );
        }


        public static void MakeAppHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            HtmlHelper.CheckAppDir( post );

            Link lnk = new Link( ctx );
            String addr = strUtil.Join( ctx.url.SiteAndAppPath, lnk.To( new ContentController().Index ) );

            String html = makeHtml( addr );
            file.Write( HtmlHelper.GetAppPath( post ), html );

        }

        public static void DeleteDetailHtml( MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            String filePath = HtmlHelper.GetPostPath( post );
            if (file.Exists( filePath )) file.Delete( filePath );
        }


        private static String makeHtml( String addr ) {
            StringWriter sw = new StringWriter();
            IWebContext webContext = MockWebContext.New( addr, sw );
            MvcContext ctx = new MvcContext( webContext );
            ctx.SetItem( "_makeHtml", true );

            new CoreHandler().ProcessRequest( ctx );
            return sw.ToString();
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

        //----------------------------------------------------------------------------

        public static String CheckListDir( ContentPost data ) {

            String dir = GetListDir( data );

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        public static String GetListDir( ContentPost data ) {
            return PathHelper.Map( "/html/list/" );
        }

        public static String GetListPath( ContentPost post ) {
            return Path.Combine( GetListDir( post ), post.PageSection.Id + ".html" );
        }

        public static String GetListLink( int sectionId ) {
            return string.Format( "/html/list/{0}.html", sectionId );
        }

        //----------------------------------------------------------------------------

        public static String CheckAppDir( ContentPost data ) {

            String dir = GetAppDir( data );

            if (dir == null) return null;

            if (Directory.Exists( dir )) return dir;
            Directory.CreateDirectory( dir );

            return dir;
        }

        private static string GetAppDir( ContentPost data ) {

            ContentApp app = ContentApp.findById( data.AppId );
            if (app == null) throw new Exception( "app not found: ContentPost.Id=" + data.Id + ", Content.AppId=" + data.AppId );

            String staticDir = app.GetSettingsObj().StaticDir;
            if (strUtil.IsNullOrEmpty( staticDir )) {

                logger.Error( "app(id="+app.Id+")尚未配置静态目录，使用默认目录：cms+" + app.Id );

                staticDir = "cms" + app.Id;
            }

            return PathHelper.Map( "/" + staticDir + "/" );
        }

        public static string GetAppPath( ContentPost post ) {

            return Path.Combine( GetAppDir( post ), "default.html" );

        }

    }

}
