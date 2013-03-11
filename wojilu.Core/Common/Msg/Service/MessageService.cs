/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.ORM.Caching;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Interface;

namespace wojilu.Common.Msg.Service {

    public class MessageService : IMessageService {

        public IBlacklistService blacklistService { get; set; }

        public MessageService() {
            blacklistService = new BlacklistService();
        }


        private static readonly char[] separator = new char[] { ',', '，', '、', '/', '|' };


        public virtual Message GetPrevMsg( int receiverId, int msgId ) {
            Message msg = db.find<Message>( "Id<" + msgId + " and Receiver.Id=" + receiverId + " and IsDelete=0 order by Id desc" ).first();
            return msg;
        }

        public virtual Message GetNextMsg( int receiverId, int msgId ) {
            Message msg = db.find<Message>( "Id>" + msgId + " and Receiver.Id=" + receiverId + " and IsDelete=0 order by Id asc" ).first();
            return msg;
        }

        public virtual void DeleteToTrash( Message msg ) {

            msg.IsDelete = 1;
            msg.update();
            updateAllMsgCount( msg.Receiver );

        }




        public virtual Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody ) {

            return SendMsg( sender, rawReceiver, msgTitle, msgBody, 0, new int[] { } );
        }


        public virtual Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody, int replyId, int[] attachmentIds ) {

            Result result = this.isValid( rawReceiver, msgTitle, msgBody );
            if (result.HasErrors) return result;

            result = this.getReceivers( rawReceiver, sender );
            if (result.HasErrors) return result;

            List<User> info = result.Info as List<User>;
            result = this.saveMessageData( sender, rawReceiver, msgTitle, msgBody );
            if (result.HasErrors) return result;

            MessageData messageData = result.Info as MessageData;

            saveAttachments( messageData, attachmentIds );

            updateReplyStatus( replyId );

            return this.saveEveryMsg( info, messageData );
        }

        private static void updateReplyStatus( int replyId ) {

            if (replyId <= 0) return;

            Message replyTarget = Message.findById( replyId );
            if (replyTarget == null) return;

            replyTarget.IsReply = 1;
            replyTarget.update();
        }


        private void saveAttachments( MessageData messageData, int[] attachmentIds ) {

            if (attachmentIds == null || attachmentIds.Length == 0) return;

            int count = 0;
            foreach (int id in attachmentIds) {

                MessageAttachment att = MessageAttachment.findById( id );
                if (att == null) continue;

                att.MessageData = messageData;
                att.update();

                count++;
            }

            if (count > 0) {
                messageData.AttachmentCount = count;
                messageData.update();
            }
        }

        public virtual void SiteSend( String title, String body, User receiver ) {

            sendSiteMsg( title, body, receiver, 0 );
            updateAllMsgCount( receiver );
            updateNewMsgCount( receiver );
        }

        private Message sendSiteMsg( String title, String body, User receiver, int siteMsgId ) {
            MessageData data = new MessageData();
            data.SenderName = lang.get( "site" );
            data.Title = title;
            data.Body = body;
            data.ToName = receiver.Name;

            db.insert( data );

            Message message = new Message();
            message.MessageData = data;
            message.Receiver = receiver;
            message.SiteMsgId = siteMsgId;
            db.insert( message );

            return message;
        }

        public virtual Boolean AdminByAction( String action, User member, String choice ) {

            String condition = string.Format( "Id in ({0}) and ReceiverId={1}", choice, member.Id );
            Message message = new Message();
            if (action.Equals( "delete" )) {
                db.updateBatch<Message>( "set IsDelete=1", condition );
                updateAllMsgCount( member );
                updateNewMsgCount( member );
            }
            else if (action.Equals( "undelete" )) {
                db.updateBatch<Message>( "set IsDelete=0", condition );
                updateAllMsgCount( member );
                updateNewMsgCount( member );
            }
            else if (action.Equals( "deletetrue" )) {
                db.deleteBatch<Message>( condition );
                updateAllMsgCount( member );
                updateNewMsgCount( member );
            }
            else if (action.Equals( "deletesendedmsg" )) {
                String condition2 = "Id in (" + choice + ") and SenderId=" + member.Id;
                db.updateBatch<MessageData>( "set IsDelete=1", condition2 );
            }
            else if (action.Equals( "deletedraftmsg" )) {
                String condition3 = "Id in (" + choice + ") and SenderId=" + member.Id;
                db.deleteBatch<MessageDraft>( condition3 );
            }
            return true;
        }

        private static void deleteMsgCount( User receiver ) {
            receiver.MsgNewCount--;
            db.update( receiver, "MsgNewCount" );
        }

        public virtual DataPage<Message> GetPageAll( int receiverId ) {
            return db.findPage<Message>( "Receiver.Id=" + receiverId + " and IsDelete=0" );
        }

        // 搜索: 发信人是key的所有我的邮件
        public virtual DataPage<Message> SearchByUser( int receiverId, string senderName ) {

            User sender = User.find( "Name=:name" ).set( "name", senderName ).first();
            if (sender == null) return DataPage<Message>.GetEmpty();

            return Message.findPage( "ReceiverId=" + receiverId + " and MessageData.SenderId=" + sender.Id + " and IsDelete=0" );
        }


        // 搜索: 收信人是key的所有已发邮件
        public virtual DataPage<MessageData> SearchByReceiver( int senderId, string receiverName ) {

            User receiver = User.find( "Name=:name" ).set( "name", receiverName ).first();
            if (receiver == null) return DataPage<MessageData>.GetEmpty();

            String condition = string.Format( "(ToName='{0}' or ToName like '%,{0},%' or ToName like '{0},%' or ToName like '%,{0}'  )", receiver.Name );

            return db.findPage<MessageData>( condition + " and SenderId=" + senderId );
        }

        public virtual DataPage<MessageData> GetPageSent( int senderId ) {
            return db.findPage<MessageData>( "Sender.Id=" + senderId + " and IsDelete=0" );
        }

        public virtual DataPage<Message> GetPageTrash( int receiverId ) {
            return db.findPage<Message>( "Receiver.Id=" + receiverId + " and IsDelete=1" );
        }

        public virtual Message GetById( int receiverId, int id ) {
            Message msg = db.find<Message>( "Id=" + id + " and Receiver.Id=" + receiverId + " and IsDelete=0" ).first();
            return msg;
        }

        public virtual MessageData GetDataById( int senderId, int id ) {
            return db.find<MessageData>( "Sender.Id=" + senderId + " and Id=" + id ).first();
        }


        public virtual DataPage<Message> GetNewMsg( int receiverId ) {
            return db.findPage<Message>( "Receiver.Id=" + receiverId + " and IsRead=0 and IsDelete=0" );
        }

        public virtual List<Message> GetNewMsg( int receiverId, int count ) {
            return db.find<Message>( "Receiver.Id=" + receiverId + " and IsRead=0 and IsDelete=0" ).list( count );
        }

        private Result getReceivers( String rawReceiver, User sender ) {
            Result result = new Result();
            List<User> list = new List<User>();
            List<int> ids = new List<int>();
            string[] strArray = rawReceiver.Trim().Split( separator );
            for (int i = 0; i < strArray.Length; i++) {

                User receiver = new UserService().IsExist( strArray[i] );
                if (receiver == null) {
                    result.Add( lang.get( "exReceiverNotFound" ) + ": \"" + strArray[i] + "\" " );
                    return result;
                }

                if (ids.Contains( receiver.Id )) continue;

                if (blacklistService.IsBlack( receiver.Id, sender.Id )) {
                    result.Add( lang.get( "blackSendMsg" ) + ":" + receiver.Name );
                    return result;
                }

                list.Add( receiver );
                ids.Add( receiver.Id );
            }
            result.Info = list;
            return result;
        }

        public virtual MessageStats GetStats( User owner ) {
            MessageStats stats = new MessageStats();
            stats.New = owner.MsgNewCount;
            stats.All = owner.MsgCount;
            stats.Sended = db.find<MessageData>( "IsDelete=0 and Sender.Id=" + owner.Id ).count();
            stats.Trash = db.find<Message>( "IsDelete=1 and Receiver.Id=" + owner.Id ).count();
            return stats;
        }


        public virtual void ReadMsg( Message msg ) {
            if (msg.IsRead != 1) {
                msg.IsRead = 1;
                db.update( msg, "IsRead" );
                deleteMsgCount( msg.Receiver );
            }
        }

        private Result saveEveryMsg( List<User> receiverList, MessageData messageData ) {

            foreach (User user in receiverList) {

                Message msg = new Message();
                msg.MessageData = messageData;
                msg.Receiver = user;
                msg.IsDelete = 0;
                msg.IsRead = 0;
                msg.IsReply = 0;
                Result result = db.insert( msg );
                if (result.HasErrors) return result;

                updateAllMsgCount( user );
                updateNewMsgCount( user );
            }
            return new Result();
        }

        private Result saveMessageData( User sender, String rawReceiver, String msgTitle, String msgBody ) {
            MessageData data = new MessageData();
            data.Sender = sender;
            data.SenderName = sender.Name;
            data.ToName = rawReceiver.Trim();
            data.Title = msgTitle;
            data.Body = msgBody;

            encodeBody( data );

            return db.insert( data );
        }

        private void encodeBody( MessageData data ) {
            data.Body = MessageEncoder.Encode( data.Body );
        }

        public virtual void CheckSiteMsg( User user ) {

            String sql = "select * from {0} where (ReceiverRoleId=-1 or ReceiverRoleId={1}) and Id not in( select SiteMsgId from {2} where receiverId={3} and SiteMsgId>0 )";
            String tbls = Entity.GetInfo( typeof( MessageSite ) ).TableName;
            String tblm = Entity.GetInfo( typeof( Message ) ).TableName;
            sql = string.Format( sql, tbls, user.RoleId, tblm, user.Id );

            List<MessageSite> list = db.findBySql<MessageSite>( sql );

            if (list.Count > 0) {

                foreach (MessageSite sm in list) {
                    sendSiteMsg( sm.Title, sm.Body, user, sm.Id );
                }

                updateAllMsgCount( user );
                updateNewMsgCount( user );

                // 移除缓存：上面的sql查询在缓存系统中只和 MessageSite 相关，
                // 所以 Message 增加并不会影响 MessageSite 的缓存，所以需要移除缓存
                CacheTime.updateTable( typeof( MessageSite ) );
            }
        }

        public virtual void SiteSend( String title, String body, List<User> receivers ) {
            foreach (User receiver in receivers) SiteSend( title, body, receiver );
        }

        private static void updateAllMsgCount( User member ) {
            int count = db.find<Message>( "Receiver.Id=" + member.Id + " and IsDelete=0" ).count();
            if (member.MsgCount != count) {
                member.MsgCount = count;
                db.update( member, "MsgCount" );
            }
        }

        private static void updateNewMsgCount( User member ) {
            int count = db.find<Message>( "Receiver.Id=" + member.Id + " and IsRead=0 and IsDelete=0" ).count();
            if (member.MsgNewCount != count) {
                member.MsgNewCount = count;
                db.update( member, "MsgNewCount" );
            }
        }


        private Result isValid( String rawReceiver, String msgTitle, String msgBody ) {

            Result result = new Result();
            if (strUtil.IsNullOrEmpty( rawReceiver )) result.Add( lang.get( "exRequireReceiver" ) );
            if (strUtil.IsNullOrEmpty( msgTitle )) result.Add( lang.get( "exRequireTitle" ) );
            if (strUtil.IsNullOrEmpty( msgBody )) result.Add( lang.get( "exRequireContent" ) );

            return result;
        }

    }
}

