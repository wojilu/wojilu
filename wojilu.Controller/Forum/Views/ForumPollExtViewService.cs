/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Context;
using wojilu.Common.Polls.Domain;
using wojilu.Common.AppBase.Interface;

using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Poll.Views;

namespace wojilu.Apps.Forum.Views {

    public class ForumPollExtViewService : IExtViewService {

        public String GetViewById( int topicId, String typeFullName, MvcContext ctx ) {

            User viewer = ctx.viewer.obj as User;

            PollBase poll = getByTopic( topicId );
            int boardId = getBoard( topicId );

            String lnkVote = ctx.GetLink().To( new wojilu.Web.Controller.Forum.Users.PollController().Vote, poll.Id ) + "?boardId=" + boardId;
            String lnkVoter = ctx.GetLink().To( new wojilu.Web.Controller.Forum.Users.PollController().Voter, poll.Id ) + "?boardId=" + boardId;


            return new PollViewFactory( viewer, poll, lnkVote, lnkVoter ) .GetPollView().GetBody( false );
        }

        private int getBoard( int topicId ) {
            ForumTopic topic = ForumTopic.findById( topicId );
            if (topic != null) return topic.ForumBoard.Id;
            return 0;
        }

        private PollBase getByTopic( int topicId ) {
            return db.find<ForumPoll>( "TopicId=" + topicId ).first();
        }
    }

}
