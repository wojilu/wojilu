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
using wojilu.Web.Controller.Forum.Users;

namespace wojilu.Apps.Forum.Views {

    public class ForumPollExtViewService : IExtViewService {

        public String GetViewById( int topicId, String typeFullName, MvcContext ctx ) {

            PollBase poll = getByTopic( topicId );
            ctx.SetItem( "poll", poll );

            return ctx.controller.loadHtml( new PollController().Detail );
        }

        private PollBase getByTopic( int topicId ) {
            return db.find<ForumPoll>( "TopicId=" + topicId ).first();
        }
    }

}
