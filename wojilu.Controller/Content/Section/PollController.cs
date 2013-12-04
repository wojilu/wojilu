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

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }
        public virtual ContentPollService pollService { get; set; }

        public PollController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            pollService = new ContentPollService();
        }

        public virtual void SectionShow( long sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            List<ContentPost> posts = postService.GetBySection( sectionId, section.ListCount );
            List<ContentPoll> polls = pollService.GetByTopicList( posts );

            IBlock block = getBlock( "list" );

            foreach (ContentPost x in posts) {

                block.Set( "postId", x.Id );
                block.Set( "lnkPoll", to( new wojilu.Web.Controller.Content.Common.PollController().Show, x.Id ) );
                block.Next();
            }
        }

        public virtual void List( long sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "section.Name", section.Title );
            set( "pollList", loadHtml( new wojilu.Web.Controller.Content.Common.PollController().List, sectionId ) );
        }

        public virtual void Show( long id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            ContentPoll poll = pollService.GetByTopicId( id );

            set( "lnkPoll", to( new wojilu.Web.Controller.Content.Common.PollController().Show, post.Id ) );
        }


    }

}
