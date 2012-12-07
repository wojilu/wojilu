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

namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public class PollController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public ContentPollService pollService { get; set; }

        public PollController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            pollService = new ContentPollService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            return new List<IPageSettingLink>();
        }

        public void AdminSectionShow( int sectionId ) {
        }

        public void SectionShow( int sectionId ) {

            ContentPoll c = pollService.GetRecentPoll( ctx.app.Id, sectionId );
            if (c == null) {
                content( "" );
            }
            else {
                ctx.SetItem( "poll", c );
                load( "pollHtml", new CmsPollController().Detail );
            }
        }

        public void List( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "section.Name", section.Title );
            set( "pollList", loadHtml( new CmsPollController().List, sectionId ) );
        }

        public void Show( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            ContentPoll poll = pollService.GetByTopicId( id, typeof( ContentPost ).FullName );

            postService.AddHits( post );

            bind( "x", post );

            ctx.SetItem( "ContentPost", post );
            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", post.PageSection.Id );

            set( "x.Content", loadHtml( new CmsPollController().Detail ) );
        }

    }

}
