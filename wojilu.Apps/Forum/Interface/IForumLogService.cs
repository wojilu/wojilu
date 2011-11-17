/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumLogService {

        void Add( User user, int appId, String msg, String ip );
        Result AddPost( User user, int appId, int postId, int actionId, String ip );
        Result AddTopic( User user, int appId, int topicId, int actionId, String reason, String ip );

        DataPage<ForumLog> FindPage( int appId );
        ForumLog GetByDeletedPostId( int postId );
        ForumLog GetByDeletedTopicId( int topicId );

        Result AddTopic( User user, int appId, int topicId, int actionId, String ip );
    }

}
