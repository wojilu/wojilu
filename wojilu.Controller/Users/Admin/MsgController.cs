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

        public IMessageService msgService { get; set; }
        public IBlacklistService blacklistService { get; set; }
        public IUserService userService { get; set; }
        public IMessageAttachmentService attachmentService { get; set; }

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

        public void Index() {
            set( "actionTitle", lang( "allMsg" ) );
            set( "adminAction", to( Admin ) );

            target( SearchSender );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            set( "searchTerm", "" );

            bindList( msgService.GetPageAll( ctx.owner.Id ) );
        }

        public void Sent() {
            set( "adminAction", to( Admin ) );
            set( "actionTitle", lang( "sentMsg" ) );

            target( SearchReceiver );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
            set( "searchTerm", "" );

            DataPage<MessageData> list = msgService.GetPageSent( ctx.owner.Id );
            bindSentList( list );
        }

        //---------------------------------

        public void Unread() {
            set( "actionTitle", lang( "unreadMsg" ) );
            set( "adminAction", to( Admin ) );

            DataPage<Message> newMsgs = msgService.GetNewMsg( ctx.owner.Id );
            bindList( newMsgs );
        }

        public void Deleted() {
            set( "actionTitle", lang( "msgTrash" ) );
            set( "adminAction", to( Admin ) );

            bindList( msgService.GetPageTrash( ctx.owner.Id ) );
        }

        //-------------------------------------------------------------------------------------------------

        public void SearchSender() {
            view( "Index" );

            target( SearchSender );
            set( "friendLink", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );

            String key = strUtil.SqlClean( ctx.Get( "q" ), 20 );
            set( "searchTerm", key );

            DataPage<Message> list = msgService.SearchByUser( ctx.owner.Id, key );
            bindList( list );
        }


        public void SearchReceiver() {

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
        public void New( int id ) {
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
        public void Forward( int id ) {

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
        public void Reply( int id ) {

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
        public void Create() {

            User user = ctx.owner.obj as User;

            String receiverName = ctx.Post( "ToName" );
            int replyId = ctx.PostInt( "replyId" );
            int[] ids = cvt.ToIntArray( ctx.Post( "attachmentIds" ) );

            Result result = msgService.SendMsg( user, receiverName, ctx.Post( "Title" ), ctx.PostHtml( "Content" ), replyId, ids );
            if (result.IsValid) {
                echoRedirectPart( lang( "opok" ), to( Sent ), 1 );
            }
            else {
                echoError( result );
            }
        }
        //-------------------------------------------------------------------------------------------------

        [Login]
        public void Read( int id ) {

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

        public void SentMsg( int id ) {
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

        public void DownloadAttachment( int id ) {

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
        public void Admin() {

            User user = ctx.owner.obj as User;

            if (msgService.AdminByAction( ctx.Post( "action" ), user, ctx.PostIdList( "choice" ) )) {
                echoAjaxOk();
            }
            else {
                content( lang( "exUnknowCmd" ) );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int msgId ) {

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

