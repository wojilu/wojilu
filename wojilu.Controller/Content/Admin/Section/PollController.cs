/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Admin.Common;


namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class PollController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public ContentPollService pollService { get; set; }

        public PollController() {
            postService = new ContentPostService();
            pollService = new ContentPollService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {

            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().Edit, sectionId );
            links.Add( lnk );

            return links;
        }

        public void AdminSectionShow( int sectionId ) {

            set( "section.Id", sectionId );
            set( "addLink", to( new Admin.Common.PollController().Add, sectionId ) );
            set( "listLink", to( new Admin.Common.PollController().AdminList, sectionId ) );

            ContentPoll c = pollService.GetRecentPoll( ctx.app.Id, sectionId );

            if (c == null) {
                set( "pollHtml", "" );
            }
            else {
                ctx.SetItem( "poll", c );
                load( "pollHtml", new wojilu.Web.Controller.Content.Common.PollController().Detail );
            }
        }

        public void SectionShow( int sectionId ) {
        }
    }
}
