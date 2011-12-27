using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Caching;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class HtmlController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlController ) );

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public HtmlController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public void Index() {

            set( "lnkMakeAll", to( MakeAll ) );
            set( "lnkMakeDetailAll", to( MakeDetailAll ) );
            set( "lnkMakeSectionAll", to( MakeSectionAll ) );
            set( "lnkMakeHome", to( MakeHomePage ) );
            set( "lnkMakeSidebar", to( MakeSidebar ) );

            IBlock sectionBlock = getBlock( "sections" );
            List<ContentSection> sections = sectionService.GetByApp( ctx.app.Id );
            foreach (ContentSection section in sections) {
                sectionBlock.Set( "section.Name", section.Title );
                sectionBlock.Set( "lnkMakeSection", to( MakeSection, section.Id ) );
                sectionBlock.Set( "lnkMakeDetail", to( MakeDetailBySection, section.Id ) );
                sectionBlock.Next();
            }

        }

        private int htmlCount = 0;

        [HttpPost]
        public void MakeAll() {
            view( "MakeDone" );

            MakeSectionAll();
            MakeDetailAll();
            MakeHomePage();
            MakeSidebar();

            set( "htmlCount", htmlCount );
        }

        public void MakeSidebar() {
            view( "MakeDone" );
            HtmlHelper.MakeSidebarHtml( ctx );
            set( "htmlCount", 1 );
            htmlCount += 1;
        }

        public void MakeHomePage() {
            view( "MakeDone" );
            HtmlHelper.MakeAppHtml( ctx );
            set( "htmlCount", 1 );
            htmlCount += 1;
        }

        [HttpPost]
        public void MakeSectionAll() {

            view( "MakeDone" );

            ContentApp app = ctx.app.obj as ContentApp;

            int count = 0;
            List<ContentSection> sections = sectionService.GetByApp( ctx.app.Id );
            foreach (ContentSection section in sections) {

                int recordCount = postService.GetCountBySection( section.Id );

                count += HtmlHelper.MakeListHtml( ctx, app, section.Id, recordCount );
                logger.Info( "make section html, sectionId=" + section.Id );
            }

            htmlCount += count;
            set( "htmlCount", htmlCount );
        }

        [HttpPost]
        public void MakeDetailAll() {

            view( "MakeDone" );


            List<ContentPost> list = postService.GetByApp( ctx.app.Id );
            makeDetail( list );

            htmlCount += list.Count;
        }


        [HttpPost]
        public void MakeSection( int sectionId ) {

            view( "MakeDone" );

            ContentApp app = ctx.app.obj as ContentApp;
            int recordCount = postService.GetCountBySection( sectionId );

            int listCount = HtmlHelper.MakeListHtml( ctx, app, sectionId, recordCount );
            set( "htmlCount", listCount );
        }

        [HttpPost]
        public void MakeDetailBySection( int sectionId ) {

            view( "MakeDone" );

            List<ContentPost> list = postService.GetBySection( sectionId );
            makeDetail( list );
        }


        //-------------------------------------------------------------------------------------------

        private void makeDetail( List<ContentPost> list ) {
            foreach (ContentPost post in list) {
                ctx.SetItem( "_currentContentPost", post );
                HtmlHelper.MakeDetailHtml( ctx );
                logger.Info( "make detail html, postId=" + post.Id );
            }

            set( "htmlCount", list.Count );
        }


    }

}
