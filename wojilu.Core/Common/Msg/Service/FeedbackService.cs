/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Msg.Service {

    public class FeedbackService : IFeedbackService {

        public virtual IFeedService feedService { get; set; }
        public virtual INotificationService nfService { get; set; }

        public FeedbackService() {
            feedService = new FeedService();
            nfService = new NotificationService();
        }

        public virtual Feedback GetById( int id ) {
            return db.findById<Feedback>( id );
        }

        public virtual List<Feedback> GetRecent( int count, int userId ) {
            if (count == 0) count = 10;
            return db.find<Feedback>( "Target.Id=" + userId + "" ).list( count );
        }

        public virtual void Insert( Feedback f ) {
            Result result = db.insert( f );
            if (result.IsValid) {
                addFeedInfo( f );
                addNotification( f );
            }
        }

        public virtual void Reply( Feedback parent, Feedback feedback ) {

            this.Insert( feedback );
            addNotificationToParentCreator( parent, feedback );
        }

        private void addNotification( Feedback f ) {

            int receiverId = f.Target.Id;
            if (f.Creator.Id == receiverId) return;

            String feedbackInfo = lang.get( "feedbackInfo" );
            String spaceLink = "<a href=\"" + Link.ToMember( f.Target ) + "\">" + lang.get( "yourSpace" ) + "</a>";
            String msg = string.Format( feedbackInfo, f.Creator.Name, spaceLink );
            //String msg = f.Creator.Name + " 给 <a href=\"" + Link.ToMember( f.Target ) + "\">" + lang.get( "yourSpace" ) + "</a> 留了言";

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
        }

        private void addNotificationToParentCreator( Feedback parent, Feedback f ) {

            int receiverId = parent.Creator.Id;
            if (f.Creator.Id == receiverId) return;

            // 不给空间主人发两遍
            if (receiverId == f.Target.Id) return;

            String feedbackReplyInfo = lang.get( "feedbackReplyInfo" );
            String spaceLink = "<a href=\"" + Link.ToMember( f.Target ) + "\">" + f.Target.Name + "</a>";
            String msg = string.Format( feedbackReplyInfo, f.Creator.Name, spaceLink );
            //String msg = f.Creator.Name + "回复了你在 <a href=\"" + Link.ToMember( f.Target ) + "\">" + f.Target.Name + "的空间</a> 的留言";

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
        }

        private void addFeedInfo( Feedback f ) {
            Feed feed = new Feed();
            feed.Creator = f.Creator;
            feed.DataType = typeof( Feedback ).FullName;

            String feedbackFeed = lang.get( "feedbackFeed" );
            String tt = string.Format( feedbackFeed, "{*actor*}", "{*user*}" );
            feed.TitleTemplate = tt;
            //feed.TitleTemplate = "{*actor*} 在 {*user*} 的空间留了言";

            feed.TitleData = "{user:\"<a href='" + Link.ToMember( f.Target ) + "'>" + f.Target.Name + "</a>\"}";
            feed.BodyGeneral = strUtil.CutString( f.Content, 100 );

            feed.Ip = f.Ip;

            feedService.publishUserAction( feed );
        }

        public virtual DataPage<Feedback> GetPageList( int userId ) {
            return db.findPage<Feedback>( "Target.Id=" + userId + "" );
        }

        public virtual void Delete( Feedback f ) {
            db.delete( f );
        }




    }

}
