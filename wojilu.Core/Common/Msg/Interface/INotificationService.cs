/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface INotificationService {

        Notification GetById( int id );
        List<Notification> GetUnread( int receiverId, String receiverType, int count );
        DataPage<Notification> GetPage( int receiverId, String receiverType );
        int GetUnReadCount( int receiverId, String receiverType );

        void Read( int notificationId );
        void ReadAll( int receiverId, String receiverType );

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        void send( int receiverId, String msg );

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        void send( int receiverId, String msg, int type );

        /// <summary>
        /// 向某种IMember对象发送通知
        /// </summary>
        /// <param name="receiverId">接收用户的Id</param>
        /// <param name="receiverType">接收者的类型Type.FullName，比如Site，User等</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        void send( int receiverId, String receiverType, String msg, int type );

        void sendFriendRequest( int senderId, int receiverId, String msg );

        void cancelFriendRequest( int senderId, int receiverId );


    }

}
