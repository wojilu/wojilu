/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Apps.Poll.Domain;
using wojilu.Apps.Poll.Views;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Poll.Admin {

    public partial class PollController : ControllerBase {

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

            set( "poll.VoteLink", to( new wojilu.Web.Controller.Poll.PollController().Vote, p.Id ) );

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
            String replies = p.Replies > 0 ? " "+lang("comment")+":" + p.Replies : "";
            set( "poll.Replies", replies );
            set( "poll.ShowLink", to( Show, p.Id ) );
        }

        public void sectionPollResult() {

            PollData p = ctx.GetItem( "poll" ) as PollData;

            bindPollItem( p );
            set( "poll.ResultLink", to( new wojilu.Web.Controller.Poll.PollController().Voter, p.Id ) );

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
    }

}
