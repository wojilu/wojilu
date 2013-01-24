/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Poll.Service;
using wojilu.Apps.Poll.Domain;
using wojilu.Apps.Poll.Views;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Poll.Admin;
using wojilu.Web.Controller.Common;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Poll {

    [App( typeof( PollApp ) )]
    public class PollController : ControllerBase {

        public PollDataService pollService { get; set; }

        public PollController() {
            pollService = new PollDataService();
        }

        public override void Layout() {
            IBlock block = getBlock( "list" );
            List<PollData> list = pollService.GetHots( ctx.app.Id, 10 );
            foreach (PollData p in list) {
                block.Set( "p.Title", p.Title );
                block.Set( "p.Link", to( Show, p.Id ) );
                block.Next();
            }
        }

        public void Index() {

            ctx.Page.Title = lang( "poll" );

            DataPage<PollData> polls = pollService.GetPageByApp( ctx.app.Id );
            bindPollList( polls );
        }

        public void Show( int id ) {
            PollData poll = pollService.GetById( id );

            pollService.AddHits( poll );

            bindDetail( poll );
            bindComment( poll );
        }


        private void bindPollList( DataPage<PollData> polls ) {

            User user = (User)ctx.viewer.obj;

            IBlock block = getBlock( "list" );
            for (int i = 0; i < polls.Results.Count; i++) {

                PollData poll = polls.Results[i];
                ctx.SetItem( "poll", poll );

                block.Set( "post.UserPic", poll.Creator.PicSmall );
                Boolean hasVote = poll.CheckHasVote( user.Id );
                String html = hasVote ? loadHtml( sectionPollResult ) : loadHtml( sectionPoll );

                block.Set( "post.Html", html );


                block.Next();
            }

            set( "page", polls.PageBar );
        }

        private void sectionPoll() {

            PollData p = ctx.GetItem( "poll" ) as PollData;

            bindPollItem( p );

            set( "poll.VoteLink", to( Vote, p.Id ) );

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

        private void bindPollItem( PollData p ) {
            set( "poll.Id", p.Id );
            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );

            set( "poll.Created", p.Created );
            String replies = p.Replies > 0 ? " " + lang( "comment" ) + ":" + p.Replies : "";
            set( "poll.Replies", replies );
            set( "poll.ShowLink", to( Show, p.Id ) );
        }

        public void sectionPollResult() {

            PollData p = ctx.GetItem( "poll" ) as PollData;

            bindPollItem( p );
            set( "poll.ResultLink", to( Voter, p.Id ) );

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

        private String getControl( PollData poll, int optionIndex ) {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            else {
                html.Radio( "pollOption", Convert.ToString( (optionIndex + 1) ), "" );
            }
            return html.ToString();
        }


        public void Vote( int pollId ) {

            PollData poll = pollService.GetById( pollId );
            if (poll == null) {
                content( lang( "exPollNotFound" ) );
                return;
            }

            if (poll.CheckHasVote( ctx.viewer.Id )) {
                content( alang( "exVoted" ) );
                return;
            }

            String choice = ctx.Get( "pollOption" );
            PollDataResult pr = new PollDataResult();
            pr.User = (User)ctx.viewer.obj;
            pr.PollId = poll.Id;
            pr.Answer = choice;
            pr.Ip = ctx.Ip;

            String lnkPoll = to( Show, poll.Id );
            pollService.CreateResult( pr, lnkPoll );

            String url = to( Voter, poll.Id );

            echoRedirect( lang( "pollDone" ), url );
        }

        public void Voter( int pollId ) {

            PollData poll = pollService.GetById( pollId );
            if (poll == null) {
                content( lang( "exPollNotFound" ) );
                return;
            }

            bindVoterList( poll );

        }


        private void bindDetail( PollData poll ) {
            set( "post.Title", poll.Title );
            set( "post.CreateTime", poll.Created );
            set( "post.ReplyCount", poll.Replies );
            set( "post.Hits", poll.Hits );

            User user = (User)ctx.viewer.obj;
            Boolean hasVote = poll.CheckHasVote( user.Id );

            ctx.SetItem( "poll", poll );

            if (hasVote)
                set( "post.Content", loadHtml( sectionPollResult ) );
            else
                set( "post.Content", loadHtml( sectionPoll ) );
        }

        private void bindComment( PollData post ) {
            set( "commentUrl", getCommentUrl( post ) );
        }

        private string getCommentUrl( PollData post ) {

            return t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + alink.ToAppData( post, ctx )
                + "&dataType=" + typeof( PollData ).FullName
                + "&dataTitle=" + post.Title
                + "&dataUserId=" + post.Creator.Id
                + "&dataId=" + post.Id;
        }



        private void bindVoterList( PollData poll ) {
            ctx.SetItem( "poll", poll );

            load( "voteResult", sectionPollResult );

            DataPage<PollDataResult> voterList = pollService.GetVoterList( poll.Id, 10 );
            IBlock block = getBlock( "list" );
            foreach (PollDataResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }
            set( "page", voterList.PageBar );
        }

    }

}
