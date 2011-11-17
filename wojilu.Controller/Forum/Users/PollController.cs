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


namespace wojilu.Web.Controller.Forum.Users {

    [App( typeof( ForumApp ) )]
    public partial class PollController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public ForumPollService pollService { get; set; }
        public IForumTopicService topicService { get; set; }

        public PollController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            pollService = new ForumPollService();
        }

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
                echoText( alang( "exPollItemNotFound" ) );
                return;
            }

            if (poll.CheckHasVote( ctx.viewer.Id )) {
                echoText( alang( "exVoted" ) );
                return;
            }

            String choice = ctx.Get( "pollOption" );
            ForumPollResult pollResult = new ForumPollResult();
            pollResult.User = (User)ctx.viewer.obj;
            pollResult.PollId = poll.Id;
            pollResult.Answer = choice;
            pollResult.Ip = ctx.Ip;

            String lnkPost = to( new Forum.TopicController().Show, poll.TopicId );
            pollService.CreateResult( pollResult, lnkPost );

            ForumBoard board = setCurrentBoard( poll );

            String lnkVote = to( Vote, id ) + "?boardId=" + board.Id;
            String lnkVoter = to( Voter, id ) + "?boardId=" + board.Id;

            String json = new PollViewFactory( (User)ctx.viewer.obj, poll, lnkVote, lnkVoter ).GetJsonResult();
            echoText( json );
        }

        public void Voter( int id ) {

            ForumPoll poll = pollService.GetById( id );

            if (poll == null) {
                echoText( alang( "exPollItemNotFound" ) );
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
            editor( "Question", "", "180px" );

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

