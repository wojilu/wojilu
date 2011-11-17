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


namespace wojilu.Web.Controller.Forum {

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

        public void Add( int id ) {

            ForumBoard board = getTree().GetById( id );
            if (board == null) {
                echo( alang( "exBoardNotFound" ) );
                return;
            }

            target( Create, id );
            List<ForumBoard> pathboards = getTree().GetPath( board.Id );
            bindAddForm( id, pathboards );
        }

        [HttpPost, DbTransaction]
        public void Create( int id ) {

            ForumBoard board = boardService.GetById( id, ctx.owner.obj );
            if (board == null) {
                echoRedirect( alang( "exBoardNotFound" ) );
                return;
            }

            ForumPoll poll = new PollValidator<ForumPoll>().Validate( ctx );
            if (errors.HasErrors) {
                run( Add, id );
                return;
            }

            pollService.CreatePoll( poll, id, ctx.owner.obj, (IApp)ctx.app.obj );

            new ForumCacheRemove( boardService, this ).CreateTopic( board );

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

            String lnkPost = to( new TopicController().Show, poll.TopicId );
            pollService.CreateResult( pollResult, lnkPost );

            setCurrentBoard( poll );

            String lnkVote = to( Vote, id );
            String lnkVoter = to( Voter, id );
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

        private void setCurrentBoard( ForumPoll poll ) {

            ForumTopic topic = topicService.GetById( poll.TopicId, ctx.owner.obj );
            ForumBoard board = boardService.GetById( topic.ForumBoard.Id, ctx.owner.obj );
            ctx.SetItem( "forumBoard", board );
        }

    }
}

