/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Polls.Service;

using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Poll.Utils;
using wojilu.Web.Controller.Content.Caching;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Interface;


namespace wojilu.Web.Controller.Content.Common {

    [App( typeof( ContentApp ) )]
    public class PollController : ControllerBase {

        public virtual ContentPollService pollService { get; set; }
        public virtual IContentPostService topicService { get; set; }

        public PollController() {
            topicService = new ContentPostService();
            pollService = new ContentPollService();
        }

        public void List( int sectionId ) {

            DataPage<ContentPost> list = topicService.GetPageBySection( sectionId );
            List<ContentPoll> polls = pollService.GetByTopicList( list.Results );
            populatePoll( list.Results, polls );

            list.Results.ForEach( x => {
                x.data.show = alink.ToAppData( x, ctx );
                x.data["Replies"] = x.Replies > 0 ? string.Format( "<span class=\"spVote\">|</span>{0}评论", x.Replies ) : "";
            } );

            bindList( "list", "x", list.Results );
            set( "page", list.PageBar );
        }

        private void populatePoll( List<ContentPost> list, List<ContentPoll> polls ) {
            foreach (ContentPost x in list) {
                ContentPoll poll = getPollByPost( polls, x );
                if (poll == null) {
                    x.data["VoteCount"] = "0";
                    x.data["Option1"] = "";
                    x.data["Option2"] = "";
                }
                else {
                    x.data["VoteCount"] = poll.VoteCount.ToString();
                    x.data["Option1"] = getOption( poll.OptionList[0], poll.Type );
                    x.data["Option2"] = getOption( poll.OptionList[1], poll.Type );
                }
            }
        }

        private string getOption( String option, int pollType ) {
            String inputType = (pollType == 0 ? "radio" : "checkbox");
            return string.Format( "<input type=\"{0}\" /> <span class=\"help-inline\">{1}</span>", inputType, option );
        }

        private ContentPoll getPollByPost( List<ContentPoll> polls, ContentPost x ) {
            foreach (ContentPoll poll in polls) {
                if (poll.TopicId == x.Id) return poll;
            }
            return null;
        }

        //---------------------------------------------------------------------------------

        public void Show( int id ) {

            ContentPoll p = pollService.GetByTopicId( id );
            ctx.SetItem( "poll", p );
            actionContent( loadHtml( Detail ) );
        }

        public void Detail() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            if (p == null) {
                set( "topicId", 0 );
                set( "pollForm", "" );
                set( "pollResult", "" );
                set( "poll.VoteLink", "" );
                set( "resultLink", "" );
                return;
            }

            set( "topicId", p.TopicId );
            set( "poll.Id", p.Id );
            set( "poll.VoteLink", to( Vote, p.Id ) );
            set( "resultLink", to( GetPollResultHtml, p.Id ) );

            String hideCss = "display:none;";
            if (p.CheckHasVote( ctx.viewer.Id ) || (p.IsClosed() && p.IsVisible == 0)) {
                set( "formStyle", hideCss );
                set( "resultStype", "" );
            }
            else {
                set( "formStyle", "" );
                set( "resultStype", hideCss );
            }

            load( "pollForm", pollForm );
            load( "pollResult", pollResult );
        }

        public void pollForm() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "topicId", p.TopicId );

            set( "resultLink", to( GetPollResultHtml, p.Id ) );

            set( "poll.Id", p.Id );
            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );
            set( "poll.VoteLink", to( Vote, p.Id ) );

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                opblock.Set( "op.SelectControl", getControl( p, i, p.OptionList[i] ) );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Next();
            }

            IBlock cmdBlock = getBlock( "cmdVote" ); // 投票命令
            IBlock tipBlock = getBlock( "plsVote" ); // 请先登录

            if (p.IsClosed()) {
            }
            else if (ctx.viewer.IsLogin) {
                cmdBlock.Next();
            }
            else {
                tipBlock.Next();
            }

            bindViewLink( p );

            set( "poll.ExpiryInfo", p.GetRealExpiryDate() );
        }

        private void bindViewLink( ContentPoll p ) {
            IBlock lnkView = getBlock( "lnkView" );
            IBlock lblView = getBlock( "lblView" );
            if (p.IsVisible == 0 || isAuthor( p ) || ctx.viewer.IsAdministrator() || ctx.viewer.IsOwnerAdministrator( ctx.owner.obj )) {
                lnkView.Set( "topicId", p.TopicId );
                lnkView.Next();
            }
            else {
                lblView.Next();
            }
        }

        private bool isAuthor( ContentPoll p ) {
            return ctx.viewer.Id == p.Creator.Id;
        }

        public void GetPollResultHtml( int pollId ) {
            ContentPoll p = pollService.GetById( pollId );
            ctx.SetItem( "poll", p );

            echo( loadHtml( pollResult ) );
        }

        public void pollResult() {

            ContentPoll p = ctx.GetItem( "poll" ) as ContentPoll;

            set( "topicId", p.TopicId );

            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );

            set( "lnkVoter", getVoterLink( p ) );

            int colorCount = 6;
            int iColor = 1;

            IBlock opblock = getBlock( "options" );
            for (int i = 0; i < p.OptionList.Length; i++) {

                PollHelper or = new PollHelper( p, p.OptionList.Length, i );

                opblock.Set( "op.Text", p.OptionList[i] );
                opblock.Set( "op.Id", (i + 1) );
                opblock.Set( "op.ImgWidth", or.ImgWidth * 1 );
                opblock.Set( "op.Percent", or.VotesAndPercent );
                opblock.Set( "op.ColorIndex", iColor );
                opblock.Next();

                iColor++;
                if (iColor > colorCount) iColor = 1;
            }

            set( "poll.ExpiryInfo", p.GetRealExpiryDate() );

            IBlock btnVote = getBlock( "btnVote" );
            IBlock lblVoted = getBlock( "lblVoted" );
            if (p.CheckHasVote( ctx.viewer.Id )) {
                lblVoted.Next();
            }
            else if (p.IsClosed() == false) {
                btnVote.Next();
            }
        }

        private String getVoterLink( ContentPoll p ) {

            if (p.IsOpenVoter == 0) {
                String url = to( Voter, p.Id );
                return string.Format( "<a href=\"{0}\" class=\"frmBox left10 right10\" target=\"_blank\">投票人数: {1}</a>", url, p.VoteCount );
            }

            return "<span class=\"poll-voter-count\">投票人数: " + p.VoteCount + "</span>";
        }

        private String getControl( ContentPoll poll, int optionIndex, String optionText ) {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), optionText );
            }
            else {
                html.Radio( "pollOption", Convert.ToString( (optionIndex + 1) ), optionText );
            }
            return html.ToString();
        }

        //----------------------------------------------------------------------------

        [Login, HttpPost, DbTransaction]
        public void Vote( int id ) {

            ContentPoll poll = pollService.GetById( id );
            if (poll == null) {
                echoError( lang( "exPollItemNotFound" ) );
                return;
            }

            if (poll.CheckHasVote( ctx.viewer.Id )) {
                echoError( lang( "exVoted" ) );
                return;
            }

            String choice = ctx.Post( "pollOption" );
            if (strUtil.IsNullOrEmpty( choice )) {
                echoError( lang( "pollSelectRequire" ) );
                return;
            }

            ContentPollResult pollResult = new ContentPollResult();
            pollResult.User = (User)ctx.viewer.obj;
            pollResult.PollId = poll.Id;
            pollResult.Answer = choice;
            pollResult.Ip = ctx.Ip;

            String lnkPost = to( new Content.PostController().Show, poll.TopicId );
            pollService.CreateResult( pollResult, lnkPost );

            echoAjaxOk();
        }

        public void Voter( int id ) {

            ContentPoll poll = pollService.GetById( id );

            if (poll == null) {
                echoText( lang( "exPollItemNotFound" ) );
                return;
            }

            DataPage<ContentPollResult> voterList = pollService.GetVoterList( id );

            bindVoter( poll, voterList );
        }

        private void bindVoter( ContentPoll poll, DataPage<ContentPollResult> voterList ) {

            IBlock block = getBlock( "list" );
            foreach (ContentPollResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }

            set( "page", voterList.PageBar );
        }


    }
}

