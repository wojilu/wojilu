/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public partial class PollController : ControllerBase, IPageSection {

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

            set( "pollSectionLink", to( SectionDetail, sectionId ) );

            //ContentPoll c = pollService.GetRecentPoll( ctx.app.Id, sectionId );
            //if (c == null) {
            //    actionContent( "" );
            //    return;
            //}

            //bindPollDetail( sectionId, c );
        }

        public void Vote( int pollId ) {

            int sectionId = ctx.GetInt( "sectionId" );

            ContentPoll poll = pollService.GetById( pollId );
            if (poll == null) {
                actionContent( lang( "exPollNotFound" ) );
                return;
            }

            ContentPost post = postService.GetById( poll.TopicId, ctx.owner.Id );
            if (post == null || post.PageSection.Id != sectionId) {
                actionContent( lang( "exPollNotFound" ) );
                return;
            }

            if (poll.CheckHasVote( ctx.viewer.Id )) {
                actionContent( alang( "exVoted" ) );
                return;
            }

            String choice = ctx.Get( "pollOption" );
            ContentPollResult pr = new ContentPollResult();
            pr.User = (User)ctx.viewer.obj;
            pr.PollId = poll.Id;
            pr.Answer = choice;
            pr.Ip = ctx.Ip;

            //String lnkPoll = to( Show, poll.TopicId );
            String lnkPoll = alink.ToAppData( post );

            pollService.CreateResult( pr, lnkPoll );

            String url = to( VoteResult, poll.Id ) + "?sectionId=" + sectionId;

            echoRedirect( alang( "pollDone" ), url );
        }

        public void VoteResult( int pollId ) {

            int sectionId = ctx.GetInt( "sectionId" );

            ContentPoll poll = pollService.GetById( pollId );
            if (poll == null) {
                actionContent( lang( "exPollNotFound" ) );
                return;
            }

            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", sectionId );

            load( "voteResult", sectionPollResult );

        }

        public void Voter( int pollId ) {

            int sectionId = ctx.GetInt( "sectionId" );

            ContentPoll poll = pollService.GetById( pollId );
            if (poll == null) {
                actionContent( lang( "exPollNotFound" ) );
                return;
            }

            ContentPost post = postService.GetById( poll.TopicId, ctx.owner.Id );
            if( post == null ) {
            //if (post == null || post.PageSection.Id != sectionId) {
                actionContent( lang( "exPollNotFound" ) );
                return;
            }

            bindVoterList( poll, sectionId );
        }

        public void List( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            Page.Title = section.Title;
            ctx.SetItem( "PageTitle", Page.Title );

            DataPage<ContentPost> list = postService.GetPageBySection( sectionId, 10 );

            List<ContentPoll> polls = pollService.GetByTopicList( list.Results );
            ctx.SetItem( "sectionId", sectionId );

            bindList( section, list, polls );
        }

        public void Show( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );
            ContentPoll poll = pollService.GetByTopicId( id, typeof( ContentPost ).FullName );

            postService.AddHits( post );

            Page.Title = post.Title;

            bindDetail( post, poll );

        }

    }

}
