/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.AppBase.Interface;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Polls.Service;

namespace wojilu.Apps.Forum.Service {

    public class ForumPollService : PollBaseService<ForumPoll, ForumPollResult> {

        public virtual ForumTopicService topicService { get; set; }

        public ForumPollService() {
            topicService = new ForumTopicService();
        }

        public Result CreatePoll( ForumPoll poll, int boardId, IMember owner, IApp app ) {

            Result result = topicService.CreateTopicOther( boardId, poll.Title, poll.Question, typeof( ForumPoll ), poll.Creator, owner, app );
            if (result.IsValid == false) return result;

            poll.TopicId = ((ForumTopic)result.Info).Id;
            return base.Insert( poll );
        }


    }


}

