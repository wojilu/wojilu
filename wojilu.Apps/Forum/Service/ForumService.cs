/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Onlines;
using wojilu.Members.Interface;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {

    public class ForumService : IForumService {

        public virtual IForumBoardService fbService { get; set; }
        public virtual IModeratorService moderatorService { get; set; }

        public ForumService() {
            this.moderatorService = new ModeratorService();
        }

        public virtual void SetOnlineUser( ForumApp forum ) {
            List<OnlineUser> results = new List<OnlineUser>();
            List<OnlineUser> onlineUsers = OnlineService.GetAll();// Online.Instance.OnlineUsers;
            int memberCount = 0;
            foreach (OnlineUser info in onlineUsers) {
                if (this.isInCurrentForum( info, forum )) {
                    results.Add( info );
                    if (info.UserId > 0) memberCount++;
                }
            }

            if (results.Count > forum.OnlinePeakCount) {
                forum.OnlinePeakCount = results.Count;
                forum.OnlinePeakTime = DateTime.Now;
                db.update( forum, new string[] { "OnlinePeakCount", "OnlinePeakTime" } );
            }
            forum.OnlineUser = results;
            forum.OnlineMemberCount = memberCount;
        }

        // TODO 判断是否在当前论坛
        private Boolean isInCurrentForum( OnlineUser user, ForumApp forum ) {
            return true;
        }

        public virtual void Update( ForumApp forum ) {
            db.update( forum );
        }

        public void UpdateSecurity( ForumApp forum, String str ) {
            forum.Security = str;
            db.update( forum, "Security" );
        }

        public virtual ForumApp GetById( int id ) {
            return db.findById<ForumApp>( id ) as ForumApp;
        }

        //-----------------------------------------------------------------------------------------------------------------------

        public virtual List<ForumTopic> GetStickyTopics( ForumApp forum ) {
            return StickyTopic.GetForumTopic( forum.StickyTopic, forum );
        }

        public virtual void StickyMoveUp( ForumApp forum, int topicId ) {
            String json = StickyTopic.MoveUp( forum.StickyTopic, topicId );
            forum.StickyTopic = json;
            db.update( forum, "StickyTopic" );
        }

        public virtual void StickyMoveDown( ForumApp forum, int topicId ) {
            String json = StickyTopic.MoveDown( forum.StickyTopic, topicId );
            forum.StickyTopic = json;
            db.update( forum, "StickyTopic" );
        }


    }
}

