/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {


    public interface IMessageService {

        /// <summary>
        /// 以网站的名义给用户发送站内短信
        /// </summary>
        /// <param name="title">短信标题</param>
        /// <param name="body">短信内容</param>
        /// <param name="receiver">接收者</param>
        void SiteSend( String title, String body, User receiver );

        /// <summary>
        /// 以网站的名义给多个用户发送站内短信
        /// </summary>
        /// <param name="title">短信标题</param>
        /// <param name="body">短信内容</param>
        /// <param name="receiver">接收者列表</param>
        void SiteSend( String title, String body, List<User> receivers );

        /// <summary>
        /// 发送站内短信
        /// </summary>
        /// <param name="sender">发送人User</param>
        /// <param name="rawReceiver">接收人的用户名（多个用户名用逗号分开）</param>
        /// <param name="msgTitle">短信标题</param>
        /// <param name="msgBody">短信内容</param>
        /// <returns></returns>
        Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody );

        Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody, int replyId, int[] attachmentIds );
        

        void ReadMsg( Message msg );
        void CheckSiteMsg( User user );

        Message GetById( int receiverId, int id );
        MessageData GetDataById( int senderId, int id );
        MessageStats GetStats( User owner );

        Message GetPrevMsg( int receiverId, int msgId );
        Message GetNextMsg( int receiverId, int msgId );

        DataPage<Message> GetPageAll( int receiverId );
        DataPage<MessageData> GetPageSent( int senderId );
        DataPage<Message> GetPageTrash( int receiverId );

        DataPage<Message> SearchByUser( int receiverId, string senderName );
        DataPage<MessageData> SearchByReceiver( int senderId, string receiverName );

        DataPage<Message> GetNewMsg( int receiverId );
        List<Message> GetNewMsg( int receiverId, int count );

        Boolean AdminByAction( String action, User member, String choice );

        void DeleteToTrash( Message msg );
    }

}

