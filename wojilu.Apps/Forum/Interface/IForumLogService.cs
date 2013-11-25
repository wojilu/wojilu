/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumLogService {

        void Add(User user, long appId, string msg, string ip);
        Result AddPost(User user, long appId, long postId, long actionId, string ip);
        Result AddTopic(User user, long appId, long topicId, long actionId, string reason, string ip);

        DataPage<ForumLog> FindPage(long appId);
        ForumLog GetByDeletedPostId(long postId);
        ForumLog GetByDeletedTopicId(long topicId);

        Result AddTopic(User user, long appId, long topicId, long actionId, string ip);
    }

}
