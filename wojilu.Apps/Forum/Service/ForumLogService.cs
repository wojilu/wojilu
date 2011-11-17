/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ForumLogService : IForumLogService {

        public virtual void Add( User user, int appId, String msg, String ip ) {

            ForumLog log = new ForumLog();
            log.UserId = user.Id;
            log.UserName = user.Name;
            log.AppId = appId;
            log.Msg = msg;
            log.Ip = ip;

            db.insert( log );
        }

        public virtual Result AddPost( User user, int appId, int postId, int actionId, String ip ) {

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

        public virtual Result AddTopic( User user, int appId, int topicId, int actionId, String reason, String ip ) {
            ForumLog log = createLog( user, appId, topicId, actionId, ip );
            log.Msg = reason;
            return db.insert( log );
        }

        public Result AddTopic( User user, int appId, int topicId, int actionId, String ip ) {
            ForumLog log = createLog( user, appId, topicId, actionId, ip );
            return db.insert( log );
        }

        private static ForumLog createLog( User user, int appId, int topicId, int actionId, String ip ) {
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

        public virtual DataPage<ForumLog> FindPage( int appId ) {
            return db.findPage<ForumLog>( "AppId=" + appId );
        }

        public virtual ForumLog GetByDeletedTopicId( int topicId ) {
            return db.find<ForumLog>( "TopicId=" + topicId + " and ActionId=" + ForumLogAction.Delete ).first();
        }

        public virtual ForumLog GetByDeletedPostId( int postId ) {
            return db.find<ForumLog>( "PostId="+postId+" and ActionId=" + ForumLogAction.Delete ).first();
        }

    }
}

