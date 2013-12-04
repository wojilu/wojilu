/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Comments;
using wojilu.Members.Interface;

namespace wojilu.Common.Msg.Service {

    public class NotificationService : INotificationService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( NotificationService ) );

        public virtual void sendFriendRequest(long senderId, long receiverId, string msg) {
            send( senderId, receiverId, typeof( User ).FullName, msg, NotificationType.Friend );
        }

        //private void send( String receiverIds, String msg, int type ) {
        //    int[] ids = cvt.ToIntArray( receiverIds );
        //    foreach (int receiverId in ids) {
        //        send( receiverId, msg, type );
        //    }
        //}

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        public virtual void send(long receiverId, string msg) {
            send( 0, receiverId, typeof( User ).FullName, msg, NotificationType.Normal );
        }

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        public virtual void send(long receiverId, string msg, int type) {
            send( 0, receiverId, typeof( User ).FullName, msg, type );
        }

        /// <summary>
        /// 向某种IMember对象发送通知
        /// </summary>
        /// <param name="receiverId">接收用户的Id</param>
        /// <param name="receiverType">接收者的类型Type.FullName，比如Site，User等</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        public virtual void send(long receiverId, string receiverType, string msg, int type) {
            send( 0, receiverId, receiverType, msg, type );
        }

        private void send(long senderId, long receiverId, string receiverType, string msg, int type) {

            User receiver = null;
            if (receiverType == typeof( User ).FullName) {
                receiver = User.findById( receiverId );
                if (receiver == null) {
                    logger.Error( "receiver not found: receiverId=" + receiverId );
                    return;
                }
            }

            Notification nf = new Notification();
            nf.Creator = new User( senderId );
            nf.ReceiverId = receiverId;
            nf.ReceiverType = receiverType;

            nf.Msg = msg;
            nf.IsRead = 0;
            nf.Type = type;

            Result result = db.insert( nf );
            if (result.IsValid) {
                addNotificationCount( receiver );
            }
        }

        // User 的最新通知数是缓存的
        private void addNotificationCount( User receiver ) {

            if (receiver == null) return;

            receiver.NewNotificationCount++;
            receiver.update( "NewNotificationCount" );
        }


        public virtual void cancelFriendRequest(long senderId, long receiverId) {
            Notification f = db.find<Notification>( "Creator.Id=" + senderId + " and ReceiverId=" + receiverId ).first();
            if (f == null) return;

            db.delete( f );

            User receiver = db.findById<User>( receiverId );
            if (receiver == null) return;

            receiver.NewNotificationCount--;
            db.update( receiver, "NewNotificationCount" );
        }

        //----------------------------------------------------------------------------------------------------------------

        public virtual Notification GetById(long id) {
            return db.findById<Notification>( id );
        }

        public virtual List<Notification> GetUnread(long receiverId, string receiverType, int count) {
            return db.find<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" )
                .list( count );
        }

        public virtual DataPage<Notification> GetPage(long receiverId, string receiverType) {
            return db.findPage<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "'" );
        }

        public virtual int GetUnReadCount(long receiverId, string receiverType) {
            return db.count<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" );
        }

        //----------------------------------------------------------------------------------------------------------------

        public virtual void Read(long notificationId) {
            Notification nf = GetById( notificationId );

            if (nf == null) throw new Exception( lang.get( "exDataNotFound" ) );

            nf.IsRead = 1;
            nf.update( "IsRead" );

            subtractNotificationCount( nf );
        }

        private void subtractNotificationCount( Notification nf ) {

            if (nf.ReceiverType != typeof( User ).FullName) return;

            User user = User.findById( nf.ReceiverId );
            if (user.NewNotificationCount > 0) {
                user.NewNotificationCount--;
                user.update( "NewNotificationCount" );
            }

        }

        private void refuseFriend( Notification nf ) {
            throw new NotImplementedException();
        }

        public virtual void ReadAll(long receiverId, string receiverType) {
            db.updateBatch<Notification>( "set IsRead=1", "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" );

            if (receiverType != typeof( User ).FullName) return;

            User user = User.findById( receiverId );
            if (user != null) {
                user.NewNotificationCount = 0;
                user.update( "NewNotificationCount" );
            }

        }

    }

}
