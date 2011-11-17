/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Context;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Polls.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Poll.Views;

namespace wojilu.Apps.Content.Views {

    public class ContentPollExtViewService : IExtViewService {

        public String GetViewById( int pollId, String typeFullName, MvcContext ctx ) {

            User viewer = ctx.viewer.obj as User;

            int sectionId = (int)ctx.GetItem( "sectionId" );

            PollBase poll = getByTopic( pollId );
            if (poll == null) throw new NullReferenceException( lang.get( "exPollNotFound" ) );
            String lnkVote = ctx.GetLink().To( new wojilu.Web.Controller.Content.Section.PollController().Vote, poll.Id ) + "?sectionId=" + sectionId;
            String lnkVoter = ctx.GetLink().To( new wojilu.Web.Controller.Content.Section.PollController().Voter, poll.Id ) + "?sectionId=" + sectionId;


            return new PollViewFactory( viewer, poll, lnkVote, lnkVoter ) .GetPollView().GetBody( false );
        }

        private PollBase getByTopic( int pollId ) {
            return db.find<ContentPoll>( "Id=" + pollId ).first();
        }
    }

}
