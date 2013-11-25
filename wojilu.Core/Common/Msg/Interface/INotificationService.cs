/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface INotificationService {

        Notification GetById(long id);
        List<Notification> GetUnread(long receiverId, string receiverType, int count);
        DataPage<Notification> GetPage(long receiverId, string receiverType);
        int GetUnReadCount(long receiverId, string receiverType);

        void Read(long notificationId);
        void ReadAll(long receiverId, string receiverType);

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        void send(long receiverId, string msg);

        /// <summary>
        /// 向用户(User)发送通知
        /// </summary>
        /// <param name="receiverId">接收User的Id</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        void send(long receiverId, string msg, int type);

        /// <summary>
        /// 向某种IMember对象发送通知
        /// </summary>
        /// <param name="receiverId">接收用户的Id</param>
        /// <param name="receiverType">接收者的类型Type.FullName，比如Site，User等</param>
        /// <param name="msg">通知内容</param>
        /// <param name="type">NotificationType的枚举值</param>
        void send(long receiverId, string receiverType, string msg, int type);

        void sendFriendRequest(long senderId, long receiverId, string msg);

        void cancelFriendRequest(long senderId, long receiverId);


    }

}
