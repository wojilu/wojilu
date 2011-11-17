/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Poll.Views;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class PollController : ControllerBase, IPageSection {


        private void bindPollDetail( int sectionId, ContentPoll poll ) {

            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", sectionId );

            User user = (User)ctx.viewer.obj;
            Boolean hasVote = poll.CheckHasVote( user.Id );

            if (hasVote)
                actionContent( loadHtml( sectionPollResult ) );
            else
                actionContent( loadHtml( sectionPoll ) );

        }

        public void SectionDetail( int sectionId ) {

            ContentPoll c = pollService.GetRecentPoll( ctx.app.Id, sectionId );
            if (c == null) {
                actionContent( "" );
                return;
            }

            bindPollDetail( sectionId, c );


        }

        private void sectionPoll() {

            int sectionId = (int)ctx.GetItem( "sectionId" );
            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "section.Id", sectionId );
            set( "poll.Id", p.Id );
            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );
            set( "poll.VoteLink", to( Vote, p.Id ) + "?sectionId=" + sectionId );

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                OptionResult or = new OptionResult( p, p.OptionList.Length, i );

                opblock.Set( "op.SelectControl", getControl( p, i ) );
                opblock.Set( "op.Text", p.OptionList[i] );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Next();
            }

            IBlock cmdBlock = getBlock( "cmdVote" );
            IBlock tipBlock = getBlock( "plsVote" );

            if (PollUtil.IsClosed( p )) {
            }
            else if (ctx.viewer.IsLogin) {
                cmdBlock.Next();
            }
            else {
                tipBlock.Next();
            }

            set( "poll.ExpiryInfo", PollUtil.GetRealExpiryDate( p ) );
        }

        public void sectionPollResult() {

            int sectionId = (int)ctx.GetItem( "sectionId" );
            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );
            set( "poll.ResultLink", to( Voter, p.Id ) + "?sectionId=" + sectionId );

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                OptionResult or = new OptionResult( p, p.OptionList.Length, i );

                opblock.Set( "op.Text", p.OptionList[i] );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Set( "op.ImgWidth", or.ImgWidth * 0.5 );
                opblock.Set( "op.Percent", or.VotesAndPercent );
                opblock.Next();
            }
            set( "poll.ExpiryInfo", PollUtil.GetRealExpiryDate( p ) );
        }

        private void bindVoterList( ContentPoll poll, int sectionId ) {

            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", sectionId );

            load( "voteResult", sectionPollResult );

            DataPage<ContentPollResult> voterList = pollService.GetVoterList( poll.Id, 10 );
            IBlock block = getBlock( "list" );
            foreach (ContentPollResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }
            set( "page", voterList.PageBar );
        }

        private String getControl( ContentPoll poll, int optionIndex ) {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            else {
                html.Radio( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            return html.ToString();
        }

        private void bindList( ContentSection section, DataPage<ContentPost> list, List<ContentPoll> polls ) {
            User user = (User)ctx.viewer.obj;

            set( "section.Name", section.Title );

            IBlock block = getBlock( "list" );
            for (int i = 0; i < polls.Count; i++) {

                ContentPoll poll = polls[i];
                ContentPost post = list.Results[i];

                block.Set( "post.Created", poll.Created );
                ctx.SetItem( "poll", poll );

                Boolean hasVote = poll.CheckHasVote( user.Id );

                String html = hasVote ? loadHtml( sectionPollResult ) : loadHtml( sectionPoll );

                block.Set( "post.Html", html );
                //block.Set( "post.ShowLink", to( Show, post.Id ) );
                block.Set( "post.ShowLink", alink.ToAppData( post ) );


                String replies = post.Replies > 0 ? lang("comment") + " :" + post.Replies : "";
                block.Set( "post.Replies", replies );

                block.Next();
            }

            set( "page", list.PageBar );
        }

        private void bindDetail( ContentPost post, ContentPoll poll ) {
            set( "post.Title", post.Title );
            set( "post.CreateTime", post.Created );
            set( "post.ReplyCount", post.Replies );
            set( "post.Hits", post.Hits );
            set( "post.Source", post.SourceLink );

            ctx.SetItem( "ContentPost", post );
            ctx.SetItem( "poll", poll );
            ctx.SetItem( "sectionId", post.PageSection.Id );

            User user = (User)ctx.viewer.obj;
            Boolean hasVote = poll.CheckHasVote( user.Id );

            if (hasVote)
                set( "post.Content", loadHtml( sectionPollResult ) );
            else
                set( "post.Content", loadHtml( sectionPoll ) );

        }



    }

}
