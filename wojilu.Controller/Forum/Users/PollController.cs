/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;

using wojilu.Web.Controller.Poll.Utils;
using wojilu.Apps.Poll.Views;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Apps.Forum.Views;
using wojilu.Common.Polls.Service;

namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public partial class PollController : ControllerBase {

        public virtual IForumBoardService boardService { get; set; }
        public virtual ForumPollService pollService { get; set; }
        public virtual IForumTopicService topicService { get; set; }

        public PollController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            pollService = new ForumPollService();
        }

        public void Detail() {

            ForumPoll p = ctx.GetItem( "poll" ) as ForumPoll;

            if (p == null) {
                set( "pollForm", "" );
                set( "pollResult", "" );
                return;
            }

            ForumBoard board = setCurrentBoard( p );

            set( "topicId", p.TopicId );
            set( "resultLink", to( GetPollResultHtml, p.Id ) + "?boardId=" + board.Id );
            set( "poll.VoteLink", to( Vote, p.Id ) + "?boardId=" + board.Id );

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

            ForumPoll p = ctx.GetItem( "poll" ) as ForumPoll;
            ForumBoard board = setCurrentBoard( p );

            set( "topicId", p.TopicId );

            set( "resultLink", to( GetPollResultHtml, p.Id ) + "?boardId=" + board.Id );

            set( "poll.Id", p.Id );
            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );
            set( "poll.VoteLink", to( Vote, p.Id ) + "?boardId=" + board.Id );

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

        private void bindViewLink( ForumPoll p ) {
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

        private bool isAuthor( ForumPoll p ) {
            return ctx.viewer.Id == p.Creator.Id;
        }

        public void GetPollResultHtml( int pollId ) {
            ForumPoll p = pollService.GetById( pollId );
            ctx.SetItem( "poll", p );

            echo( loadHtml( pollResult ) );
        }

        public void pollResult() {

            ForumPoll p = ctx.GetItem( "poll" ) as ForumPoll;
            ForumBoard board = setCurrentBoard( p );

            set( "topicId", p.TopicId );

            set( "poll.Title", p.Title );
            set( "poll.Question", p.Question );
            set( "poll.Voters", p.VoteCount );

            set( "lnkVoter", getVoterLink( p, board ) );

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

        private String getVoterLink( ForumPoll p, ForumBoard board ) {

            if (p.IsOpenVoter == 0) {
                String url = to( Voter, p.Id ) + "?boardId=" + board.Id;
                return string.Format( "<a href=\"{0}\" class=\"frmBox left10 right10\" target=\"_blank\">投票人数: {1}</a>", url, p.VoteCount );
            }

            return "<span class=\"poll-voter-count\">投票人数: " + p.VoteCount + "</span>";
        }

        private String getControl( ForumPoll poll, int optionIndex, String optionText ) {
            Html html = new Html();
            if (poll.Type == 1) {
                html.CheckBox( "pollOption", Convert.ToString( (optionIndex + 1) ), optionText );
            }
            else {
                String __name = "pollOption";
                String __val = Convert.ToString( (optionIndex + 1) );
                String __txt = optionText;

                html.Code( string.Format( "<label class=\"radio\"><input type=\"radio\" name=\"{0}\" id=\"{3}\" value=\"{1}\" /> {2}</label> ", __name, __val, __txt, __name + __val ) );
            }
            return html.ToString();
        }

        //----------------------------------------------------------------------------

        public void Add() {

            int id = ctx.GetInt( "boardId" );
            set( "ActionLink", to( Create ) + "?boardId=" + id );
            List<ForumBoard> pathboards = getTree().GetPath( id );
            bindAddForm( id, pathboards );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            int id = ctx.GetInt( "boardId" );
            ForumBoard board = boardService.GetById( id, ctx.owner.obj );

            ForumPoll poll = new PollValidator<ForumPoll>().Validate( ctx );
            if (errors.HasErrors) {
                echoError();
                return;
            }

            pollService.CreatePoll( poll, id, ctx.owner.obj, (IApp)ctx.app.obj );

            echoRedirect( lang( "opok" ), alink.ToAppData( board ) );
        }

        [HttpPost, DbTransaction]
        public void Vote( int id ) {

            ForumPoll poll = pollService.GetById( id );
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

            ForumPollResult pollResult = new ForumPollResult();
            pollResult.User = (User)ctx.viewer.obj;
            pollResult.PollId = poll.Id;
            pollResult.Answer = choice;
            pollResult.Ip = ctx.Ip;

            String lnkPost = to( new Forum.TopicController().Show, poll.TopicId );
            pollService.CreateResult( pollResult, lnkPost );

            echoAjaxOk();
        }

        public void Voter( int id ) {

            ForumPoll poll = pollService.GetById( id );

            if (poll == null) {
                echoText( lang( "exPollItemNotFound" ) );
                return;
            }

            DataPage<ForumPollResult> voterList = pollService.GetVoterList( id );

            bindVoter( poll, voterList );
        }

        //----------------------------------------------------------------------------------------

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        private ForumBoard setCurrentBoard( ForumPoll poll ) {

            ForumTopic topic = topicService.GetById( poll.TopicId, ctx.owner.obj );
            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );
            ctx.SetItem( "forumBoard", board );
            return board;
        }

        private void bindAddForm( int id, List<ForumBoard> pathboards ) {

            set( "location", ForumLocationUtil.GetPollAdd( pathboards, ctx ) );
            set( "optionCount", 5 );
            set( "forumId", id );

            if ((ctx.Post( "PollType" ) == null) || (ctx.Post( "PollType" ) == "0")) {
                set( "singleCheck", " checked=\"checked\"" );
                set( "multiCheck", "" );
            }
            else if (ctx.Post( "PollType" ) == "1") {
                set( "singleCheck", "" );
                set( "multiCheck", " checked=\"checked\"" );
            }
        }

        private void bindVoter( ForumPoll poll, DataPage<ForumPollResult> voterList ) {

            setCurrentBoard( poll );

            IBlock block = getBlock( "list" );
            foreach (ForumPollResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }

            set( "page", voterList.PageBar );
        }


    }
}

