/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Web;

using wojilu.Web.Mvc;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Msg.Domain;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Users.Admin {

    public partial class MsgController : ControllerBase {


        private void bindLayout() {
            set( "notificationUrl", to( new NotificationController().List ) );

            // 是否有阅读网站通知的权限？
            String lnkSiteNf = "";
            if (ctx.viewer.obj.RoleId == SiteRole.Administrator.Id) {
                String siteNfUrl = t2( new SiteNfController().List );
                lnkSiteNf = string.Format( "<li id=\"tabSite\"><a href=\"{0}\">网站通知</a><span></span></li>", siteNfUrl );
            }

            set( "lnkSiteNotification", lnkSiteNf );

            set( "msg.UrlNew", to( New, -1 ) );
            set( "msg.UrlUnread", to( Unread ) );
            set( "msg.UrlAll", to( Index ) );
            set( "msg.UrlSend", to( Sent ) );
            set( "msg.UrlTrash", to( Deleted ) );

            User user = ctx.owner.obj as User;

            MessageStats stats = msgService.GetStats( user );
            set( "msg.NewCount", getCountString( stats.New ) );
            set( "msg.AllCount", getCountString( stats.All ) );
            set( "msg.SendCount", getCountString( stats.Sended ) );
            set( "msg.TrashCount", getCountString( stats.Trash ) );
        }


        private void bindList( DataPage<Message> results ) {
            IBlock block = getBlock( "list" );
            foreach (Message msg in results.Results) {

                block.Set( "m.Attachments", getAttachmentIcon( msg.MessageData ) );

                block.Set( "m.Title", strUtil.CutString( msg.Title, 40 ) );
                block.Set( "m.StatusString", getStatusString( msg ) );
                block.Set( "m.IsNew", getIsNew( msg ) );
                block.Set( "m.SenderName", msg.SenderName );
                block.Set( "m.Id", msg.Id );
                block.Set( "m.ReadUrl", to( Read, msg.Id ) );
                block.Set( "m.ReceiveTime", msg.Created );
                block.Next();
            }
            set( "page", results.PageBar );
        }

        private String getAttachmentIcon( MessageData x ) {
            return x.AttachmentCount > 0 ? string.Format( "<img src=\"{0}attachment.gif\" />", sys.Path.Img ) : "";
        }

        private string getIsNew( Message msg ) {

            return msg.IsRead == 1 ? "false" : "true";

        }

        public String getStatusString( Message msg ) {

            if (msg.IsReply == 1) return string.Format( "<span class='hasReply'>{0}</span>", lang( "replyed" ) );
            if (msg.IsRead == 1) return lang( "readed" );
            return string.Format( "<span class='new'>{0}</span>", lang( "notRead" ) );
        }


        private void bindUploadInfo() {
            //附件
            set( "uploadLink", to( new UserUploadController().SaveMsgAttachment ) ); // 接受上传的网址
            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );
            set( "jsPath", sys.Path.DiskJs );
        }


        private String getCountString( int count ) {
            if (count == 0) {
                return "";
            }
            return string.Format( "({0})", count );
        }

        private String getForward( String content ) {
            return string.Format( "<br/><br/><br/><div class=\"note\">------------ " + lang( "msgForward" ) + " ------------------------<br/>{0}</div>", content );
        }

        private String getQuote( String content ) {
            return string.Format( "<br/><br/><br/><div class=\"note\">------------ " + lang( "msgQuote" ) + " ------------------------<br/>{0}</div>", content );
        }


        private void bindMsgDetail( int id, MessageData msgData, IMember dataSender ) {

            String senderUrl = "";
            String replyButton = "";
            if (dataSender.GetType() != typeof( Site )) {
                String userLink = getFullUrl( toUser( dataSender ) );
                senderUrl = string.Format( "<span class=\"senderUrl\">&lt;<a href=\"{0}\" target=\"_blank\">{0}</a>&gt;</span>&nbsp;<a href=\"{0}\" target=\"_blank\">{1}</a>", userLink, lang( "viewSender" ) );
                replyButton = string.Format( "<a href=\"{1}\" />{0}</a>", lang( "reply" ), to( Reply, id ) );
            }

            set( "m.SenderUrl", senderUrl );
            set( "m.ReplyButton", replyButton );
            set( "m.ForwardUrl", to( Forward, id ) );
            set( "m.PrevUrl", getPrevUrl( id ) );
            set( "m.NextUrl", getNextUrl( id ) );
            set( "m.DeleteUrl", to( Delete, id ) );

            set( "m.Sender", msgData.SenderName );
            set( "m.Title", msgData.Title );
            set( "m.CreateTime", msgData.Created );
            set( "m.Content", msgData.Body );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private object getNextUrl( int id ) {
            Message nextMsg = msgService.GetNextMsg( ctx.owner.Id, id );
            if (nextMsg == null) return "下一封(无)";
            return string.Format( "<a href=\"{0}\">下一封 &raquo;</a>", to( Read, nextMsg.Id ) );
        }

        private object getPrevUrl( int id ) {
            Message prevMsg = msgService.GetPrevMsg( ctx.owner.Id, id );
            if (prevMsg == null) return "上一封(无)";
            return string.Format( "<a href=\"{0}\" class=\"right10\">&laquo; 上一封</a>", to( Read, prevMsg.Id ) );
        }

        private void bindAttachmentPanel( MessageData msgData ) {
            IBlock attachmentPanel = getBlock( "attachmentPanel" );
            if (msgData.AttachmentCount > 0) {
                bindAttachments( attachmentPanel, msgData );
            }
        }

        private void bindAttachments( IBlock attachmentPanel, MessageData msgData ) {

            List<MessageAttachment> list = attachmentService.GetByMsg( msgData.Id );

            IBlock block = attachmentPanel.GetBlock( "attachments" );
            foreach (MessageAttachment obj in list) {

                block.Set( "obj.FileName", obj.FileName );
                block.Set( "obj.FileSizeKB", obj.FileSizeKB );
                block.Set( "obj.DownloadUrl", to( DownloadAttachment, obj.Id ) );
                block.Next();
            }

            attachmentPanel.Next();
        }


        private void bindSentList( DataPage<MessageData> list ) {
            IBlock block = getBlock( "list" );
            foreach (MessageData data in list.Results) {

                block.Set( "m.Attachments", getAttachmentIcon( data ) );

                block.Set( "m.Title", strUtil.CutString( data.Title, 40 ) );
                block.Set( "m.ToName", data.ToName );
                block.Set( "m.ReadUrl", to( SentMsg, data.Id ) );
                block.Set( "m.SendTime", data.Created.ToString() );
                block.Set( "m.Id", data.Id.ToString() );
                block.Next();
            }
            set( "page", list.PageBar );
        }



    }
}

