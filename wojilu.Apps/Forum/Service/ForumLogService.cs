/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ForumLogService : IForumLogService {

        public virtual void Add(User user, long appId, string msg, string ip) {

            ForumLog log = new ForumLog();
            log.UserId = user.Id;
            log.UserName = user.Name;
            log.AppId = appId;
            log.Msg = msg;
            log.Ip = ip;

            db.insert( log );
        }

        public virtual Result AddPost(User user, long appId, long postId, long actionId, string ip) {

            ForumLog log = new ForumLog();
            log.UserId = user.Id;
            log.UserName = user.Name;
            log.UserUrl = user.Url;
            log.AppId = appId;
            log.PostId = postId;
            log.ActionId = actionId;
            log.Ip = ip;

            return db.insert( log );
        }

        public virtual Result AddTopic(User user, long appId, long topicId, long actionId, string reason, string ip) {
            ForumLog log = createLog( user, appId, topicId, actionId, ip );
            log.Msg = reason;
            return db.insert( log );
        }

        public virtual Result AddTopic(User user, long appId, long topicId, long actionId, string ip) {
            ForumLog log = createLog( user, appId, topicId, actionId, ip );
            return db.insert( log );
        }

        private static ForumLog createLog(User user, long appId, long topicId, long actionId, string ip) {
            ForumLog log = new ForumLog();
            log.UserId = user.Id;
            log.UserName = user.Name;
            log.UserUrl = user.Url;
            log.AppId = appId;
            log.TopicId = topicId;
            log.ActionId = actionId;
            log.Ip = ip;
            return log;
        }

        public virtual DataPage<ForumLog> FindPage(long appId) {
            return db.findPage<ForumLog>( "AppId=" + appId );
        }

        public virtual ForumLog GetByDeletedTopicId(long topicId) {
            return db.find<ForumLog>( "TopicId=" + topicId + " and ActionId=" + ForumLogAction.Delete ).first();
        }

        public virtual ForumLog GetByDeletedPostId(long postId) {
            return db.find<ForumLog>( "PostId="+postId+" and ActionId=" + ForumLogAction.Delete ).first();
        }

    }
}

