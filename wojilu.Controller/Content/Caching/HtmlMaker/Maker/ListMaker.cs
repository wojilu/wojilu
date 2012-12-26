/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Context;
using wojilu.Web.Mvc;

using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class ListMaker : HtmlMakerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ListMaker ) );

        public IContentPostService postService { get; set; }

        public ListMaker( MvcContext ctx )
            : base( ctx ) {
            postService = new ContentPostService();
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/list/" );
        }

        public int Process( ContentPost post ) {

            if (post == null) return 0;

            int totalCount = 0;

            List<ContentPostSection> psList = ContentPostSection.find( "PostId=" + post.Id ).list();
            foreach (ContentPostSection x in psList) {

                int recordCount = postService.CountBySection( x.Section.Id );

                totalCount += this.ProcessCache( x.Section.Id, recordCount );
            }

            return totalCount;
        }

        public int ProcessCache( int sectionId, int recordCount ) {

            CheckDir();

            String lnkNormal = _ctx.link.To( new SectionController().Show, sectionId );
            String lnkArchive = _ctx.link.To( new SectionController().Archive, sectionId );


            int pageSize = getPageSize();

            return makeHtmlLoopCache( recordCount, sectionId, lnkNormal, lnkArchive, pageSize );
        }

        public int ProcessAll( int sectionId, int recordCount ) {

            CheckDir();

            String lnkNormal = _ctx.link.To( new SectionController().Show, sectionId );
            String lnkArchive = _ctx.link.To( new SectionController().Archive, sectionId );


            int pageSize = getPageSize();

            return makeHtmlLoopAll( recordCount, sectionId, lnkNormal, lnkArchive, pageSize );
        }

        private int getPageSize() {

            ContentApp app = _ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            return s.ListPostPerPage;

        }



    }

}
