/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;

using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common;

namespace wojilu.Web.Controller.Users.Admin {

    public partial class MsgController : ControllerBase {

        public virtual IMessageService msgService { get; set; }
        public virtual IBlacklistService blacklistService { get; set; }
        public virtual IUserService userService { get; set; }
        public virtual IMessageAttachmentService attachmentService { get; set; }

        public MsgController() {
            msgService = new MessageService();
            blacklistService = new BlacklistService();
            userService = new UserService();
            attachmentService = new MessageAttachmentService();
        }

        public override void Layout() {
            bindLayout();
        }

        public override void CheckPermission() {

            Boolean isMessageClose = Component.IsClose( typeof( MessageApp ) );
            if (isMessageClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        //-------------------------------------------------------------------------------

        public virtual void Index() {
            set( "actionTitle", lang( "allMsg" ) );
            set( "adminAction", to( Admin ) );

            target( SearchSender );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            set( "searchTerm", "" );

            bindList( msgService.GetPageAll( ctx.owner.Id ) );
        }

        public virtual void Sent() {
            set( "adminAction", to( Admin ) );
            set( "actionTitle", lang( "sentMsg" ) );

            target( SearchReceiver );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            set( "searchTerm", "" );

            DataPage<MessageData> list = msgService.GetPageSent( ctx.owner.Id );
            bindSentList( list );
        }

        //---------------------------------

        public virtual void Unread() {
            set( "actionTitle", lang( "unreadMsg" ) );
            set( "adminAction", to( Admin ) );

            DataPage<Message> newMsgs = msgService.GetNewMsg( ctx.owner.Id );
            bindList( newMsgs );
        }

        public virtual void Deleted() {
            set( "actionTitle", lang( "msgTrash" ) );
            set( "adminAction", to( Admin ) );

            bindList( msgService.GetPageTrash( ctx.owner.Id ) );
        }

        //-------------------------------------------------------------------------------------------------

        public virtual void SearchSender() {
            view( "Index" );

            target( SearchSender );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );

            String key = strUtil.SqlClean( ctx.Get( "q" ), 20 );
            set( "searchTerm", key );

            DataPage<Message> list = msgService.SearchByUser( ctx.owner.Id, key );
            bindList( list );
        }


        public virtual void SearchReceiver() {

            view( "Sent" );
            set( "adminAction", to( Admin ) );

            target( SearchReceiver );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );

            String key = strUtil.SqlClean( ctx.Get( "q" ), 20 );
            set( "searchTerm", key );

            DataPage<MessageData> list = msgService.SearchByReceiver( ctx.owner.Id, key );
            bindSentList( list );
        }

        //-------------------------------------------------------------------------------------------------

        [Login]
        public virtual void New( long id ) {
            target( Create );

            User receiver = userService.GetById( id );

            if (receiver != null && blacklistService.IsBlack( receiver.Id, ctx.viewer.Id )) {
                echo( lang( "blackSendMsg" ) );
                return;
            }

            set( "m.ToName", receiver == null ? "" : receiver.Name );
            set( "m.Title", "" );
            set( "m.ReplyId", "" );
            set( "Content", "" );

            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            bindUploadInfo();
        }

        [Login]
        public virtual void Forward( long id ) {

            view( "New" );
            target( Create );

            Message msg = msgService.GetById( ctx.owner.Id, id );
            if (msg == null) {
                echoRedirect( lang( "exMsgNotFound" ) );
                return;
            }

            MessageData msgData = msg.MessageData;
            User sender = msgData.Sender;

            set( "m.ToName", "" );
            set( "m.Title", lang( "forwardPrefix" ) + msgData.Title );
            set( "m.ReplyId", "" );
            set( "Content", getForward( msgData.Body ) );

            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            bindUploadInfo();
        }

        [Login]
        public virtual void Reply( long id ) {

            view( "New" );
            target( Create );

            Message msg = msgService.GetById( ctx.owner.Id, id );
            if (msg == null) {
                echoRedirect( lang( "exMsgNotFound" ) );
                return;
            }

            MessageData msgData = msg.MessageData;
            IMember sender = msgData.GetSender();
            if (sender is Site) {
                echoRedirect( lang( "exMsgSysNoReply" ) );
                return;
            }

            set( "m.ToName", msg.SenderName );
            set( "m.Title", lang( "replyPrefix" ) + msgData.Title );
            set( "m.ReplyId", id );
            set( "Content", getQuote( msgData.Body ) );

            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            bindUploadInfo();
        }

        [HttpPost, DbTransaction]
        public virtual void Create() {

            Result result = createMsg();
            if (result.IsValid) {
                echoRedirectPart( lang( "opok" ), to( Sent ), 1 );
            }
            else {
                echoError( result );
            }
        }

        [HttpPost, DbTransaction]
        public virtual void CreateOk() {

            Result result = createMsg();
            if (result.IsValid) {
                echoRedirect( lang( "opok" ), ctx.Post( "returnUrl" ) );
            }
            else {
                echoError( result );
            }
        }

        private Result createMsg() {
            User user = ctx.owner.obj as User;

            String receiverName = ctx.Post( "ToName" );
            long replyId = ctx.PostLong( "replyId" );
            long[] ids = cvt.ToLongArray( ctx.Post( "attachmentIds" ) );

            Result result = msgService.SendMsg( user, receiverName, ctx.Post( "Title" ), ctx.PostHtml( "Content" ), replyId, ids );
            return result;
        }
        //-------------------------------------------------------------------------------------------------

        [Login]
        public virtual void Read( long id ) {

            Message msg = msgService.GetById( ctx.owner.Id, id );
            if (msg == null) {
                echoRedirect( lang( "exMsgNotFound" ) );
                return;
            }

            MessageData msgData = msg.MessageData;
            IMember dataSender = msgData.GetSender();

            bindAttachmentPanel( msgData );

            bindMsgDetail( id, msgData, dataSender );

            msgService.ReadMsg( msg );
        }

        public virtual void SentMsg( long id ) {
            view( "MyMsg" );
            MessageData data = msgService.GetDataById( ctx.owner.Id, id );
            if (data == null) {
                echoRedirect( lang( "exMsgNotFound" ) );
                return;
            }

            bindAttachmentPanel( data );

            set( "m.ToName", data.ToName );
            set( "m.Title", data.Title );
            set( "m.CreateTime", data.Created );
            set( "m.Content", data.Body );
        }

        public virtual void DownloadAttachment( long id ) {

            MessageAttachment att = attachmentService.GetById( id );
            if (att == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            if (attachmentService.IsReceiver( ctx.viewer.Id, att ) == false &&
                attachmentService.IsSender( ctx.viewer.Id, att ) == false) {
                echoRedirect( lang( "exNoPermission" ) );
                return;
            }

            redirectUrl( att.FileUrl );
        }

        //-------------------------------------------------------------------------------------------------

        [HttpPost, DbTransaction]
        public virtual void Admin() {

            User user = ctx.owner.obj as User;

            if (msgService.AdminByAction( ctx.Post( "action" ), user, ctx.PostIdList( "choice" ) )) {
                echoAjaxOk();
            }
            else {
                content( lang( "exUnknowCmd" ) );
            }
        }

        [HttpDelete, DbTransaction]
        public virtual void Delete( long msgId ) {

            Message msg = msgService.GetById( ctx.owner.Id, msgId );
            if (msg == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            msgService.DeleteToTrash( msg );

            redirectUrl( to( Index ) + "?frm=true" );
        }


    }
}

