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

        public virtual INotificationService nfService { get; set; }

        public FeedbackService() {
            nfService = new NotificationService();
        }

        public virtual Feedback GetById( int id ) {
            return db.findById<Feedback>( id );
        }

        public virtual List<Feedback> GetRecent( int count, int userId ) {
            if (count == 0) count = 10;
            return db.find<Feedback>( "Target.Id=" + userId + "" ).list( count );
        }

        public virtual void Insert( Feedback f, String lnkReplyList ) {
            Result result = db.insert( f );
            if (result.IsValid) {
                addNotification( f, lnkReplyList );
            }
        }

        public virtual void Reply( Feedback parent, Feedback feedback, String lnkReplyList ) {

            this.Insert( feedback, lnkReplyList );
            addNotificationToParentCreator( parent, feedback, lnkReplyList );
        }

        private void addNotification( Feedback f, String lnkReplyList ) {

            int receiverId = f.Target.Id;
            if (f.Creator.Id == receiverId) return;

            String feedbackInfo = lang.get( "feedbackInfo" );
            String spaceLink = "<a href=\"" + lnkReplyList + "\">" + lang.get( "yourSpace" ) + "</a>";
            String msg = string.Format( feedbackInfo, f.Creator.Name, spaceLink );

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
        }

        private void addNotificationToParentCreator( Feedback parent, Feedback f, String lnkReplyList ) {

            int receiverId = parent.Creator.Id;
            if (f.Creator.Id == receiverId) return;

            // 不给空间主人发两遍
            if (receiverId == f.Target.Id) return;

            String feedbackReplyInfo = lang.get( "feedbackReplyInfo" );
            String spaceLink = "<a href=\"" + lnkReplyList + "\">" + f.Target.Name + "</a>";
            String msg = string.Format( feedbackReplyInfo, f.Creator.Name, spaceLink );

            nfService.send( receiverId, typeof( User ).FullName, msg, NotificationType.Comment );
        }

        public virtual DataPage<Feedback> GetPageList( int userId ) {
            return db.findPage<Feedback>( "Target.Id=" + userId + "" );
        }

        public virtual void Delete( Feedback f ) {
            db.delete( f );
        }




    }

}
