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

        public void SectionShow( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            List<ContentPost> posts = postService.GetBySection( sectionId, section.ListCount );
            List<ContentPoll> polls = pollService.GetByTopicList( posts );

            IBlock block = getBlock( "list" );

            foreach (ContentPost x in posts) {

                ContentPoll p = pollService.GetByTopicId( polls, x.Id );

                ctx.SetItem( "poll", p );
                block.Set( "pollHtml", loadHtml( new wojilu.Web.Controller.Content.Common.PollController().Detail ) );
                block.Next();
            }
        }

        public void List( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "section.Name", section.Title );
            set( "pollList", loadHtml( new wojilu.Web.Controller.Content.Common.PollController().List, sectionId ) );
        }

        public void Show( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            ContentPoll poll = pollService.GetByTopicId( id );

            postService.AddHits( post );

            bind( "x", post );

            ctx.SetItem( "ContentPost", post );
            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", post.SectionId );

            set( "x.Content", loadHtml( new wojilu.Web.Controller.Content.Common.PollController().Detail ) );
        }


    }

}
