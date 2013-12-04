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
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class ListMaker : HtmlMaker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ListMaker ) );

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public ListMaker(  ) {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/list/" );
        }

        public virtual int Process( ContentPost post ) {

            if (post == null) return 0;

            int totalCount = 0;

            List<ContentPostSection> psList = ContentPostSection.find( "PostId=" + post.Id ).list();
            foreach (ContentPostSection x in psList) {

                int recordCount = postService.CountBySection( x.Section.Id );

                totalCount += this.ProcessAll( x.Section.Id, recordCount );
            }

            return totalCount;
        }

        public virtual int ProcessSection( long sectionId ) {
            int recordCount = postService.CountBySection( sectionId );
            return ProcessAll( sectionId, recordCount );
        }

        public virtual int ProcessAll( long sectionId, int recordCount ) {

            CheckDir();

            ContentSection section = sectionService.GetById( sectionId );

            String lnkNormal = Link.To( Site.Instance, new SectionController().Show, sectionId, section.AppId );

            int pageSize = getPageSize( section.AppId );

            return makeHtmlLoopAll( sectionId, lnkNormal, recordCount, pageSize );
        }

        private int getPageSize( long appId ) {

            ContentApp app = ContentApp.findById( appId );
            ContentSetting s = app.GetSettingsObj();

            return s.ListPostPerPage;

        }



    }

}
