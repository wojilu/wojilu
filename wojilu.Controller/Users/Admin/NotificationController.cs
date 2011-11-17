/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Domain;
using System.Threading;

namespace wojilu.Web.Controller.Users.Admin {

    public class NotificationController : ControllerBase {

        public INotificationService notificationService { get; set; }
        public IFriendService friendService { get; set; }

        public NotificationController() {
            notificationService = new NotificationService();
            friendService = new FriendService();

            this.LayoutControllerType = typeof( MsgController );
        }

        public void NewList() {

            List<Notification> notifications = notificationService.GetUnread( ctx.owner.Id, ctx.owner.obj.GetType().FullName, 10 );

            String displayClass = notifications.Count == 0 ? "hide" : "";
            set( "displayClass", displayClass );

            IBlock nblock = getBlock( "notifications" );
            foreach (Notification nf in notifications) {
                nblock.Set( "notification.Msg", nf.Msg );

                nblock.Set( "notification.Id", nf.Id );
                nblock.Set( "notification.ReadLink", to( Read, nf.Id ) );

                IBlock fblock = nblock.GetBlock( "friendCmd" );
                if (nf.Type == NotificationType.Friend) {
                    fblock.Set( "notification.ApproveFriendLink", to( ApproveFriend, nf.Id ) );
                    fblock.Set( "notification.RefuseFriendLink", to( RefuseFriend, nf.Id ) );
                    fblock.Next();
                }

                nblock.Next();
            }

        }

        public void List() {

            set( "myHomeLink", to( new FeedController().My, 0 ) );
            set( "lnkReadAll", to( ReadAll ) );

            User owner = ctx.owner.obj as User;
            String readAllClass = owner.NewNotificationCount == 0 ? "hide" : "";
            set( "readAllClass", readAllClass );

            DataPage<Notification> list = notificationService.GetPage( ctx.owner.Id, ctx.owner.obj.GetType().FullName );

            IBlock nblock = getBlock( "notifications" );
            foreach (Notification nf in list.Results) {
                nblock.Set( "notification.Created", nf.Created );
                nblock.Set( "notification.Msg", nf.Msg );

                nblock.Set( "notification.Id", nf.Id );
                nblock.Set( "notification.ReadLink", to( Read, nf.Id ) );


                String readClass = nf.IsRead == 1 ? "nfread" : "";
                nblock.Set( "notification.ReadClass", readClass );

                IBlock fblock = nblock.GetBlock( "friendCmd" );
                if (nf.Type == NotificationType.Friend && nf.IsRead == 0) {
                    if (nf.Creator != null && nf.Creator.Id > 0) {
                        fblock.Set( "notification.ApproveFriendLink", to( ApproveFriend, nf.Id ) );
                        fblock.Set( "notification.RefuseFriendLink", to( RefuseFriend, nf.Id ) );
                        fblock.Next();
                    }
                }

                nblock.Next();
            }

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void ReadAll() {
            notificationService.ReadAll( ctx.owner.Id, ctx.owner.obj.GetType().FullName );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Read( int id ) {

            Notification nf = notificationService.GetById( id );

            // 好友邀请是无法直接标记为已读的，必须通过下面的接受或拒绝明确操作
            if (nf.Type == NotificationType.Friend && (nf.Creator != null && nf.Creator.Id > 0)) {
                echoText( "请选择批准或拒绝" );
            }
            else {
                notificationService.Read( id );
                echoAjaxOk();
            }

        }

        [HttpPut, DbTransaction]
        public void ApproveFriend( int id ) {

            Notification nf = notificationService.GetById( id );

            friendService.Approve( ctx.owner.Id, nf.Creator.Id );
            notificationService.Read( id );

            echoRedirect( lang( "opok" ) );
        }

        [HttpPut, DbTransaction]
        public void RefuseFriend( int id ) {
            Notification nf = notificationService.GetById( id );
            friendService.Refuse( ctx.owner.Id, nf.Creator.Id );
            notificationService.Read( id );
            echoRedirect( lang( "opok" ) );
        }

    }

}
