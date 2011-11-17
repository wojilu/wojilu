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
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public class SiteNfController : ControllerBase {

        public INotificationService notificationService { get; set; }
        public IFriendService friendService { get; set; }

        public SiteNfController() {
            notificationService = new NotificationService();
            friendService = new FriendService();

            this.LayoutControllerType = typeof( MsgController );
        }

        public override void CheckPermission() {
            if (ctx.viewer.obj.RoleId != SiteRole.Administrator.Id) {
                echoRedirect( lang( "exNoPermission" ) );
                ctx.web.CompleteRequest();
            }
        }

        private int ownerId() { return Site.Instance.Id; }
        private String ownerType() { return typeof( Site ).FullName; }

        public void List() {

            set( "lnkReadAll", to( ReadAll ) );

            int newCount = notificationService.GetUnReadCount( ownerId(), ownerType() );

            String readAllClass = newCount == 0 ? "hide" : "";
            set( "readAllClass", readAllClass );

            DataPage<Notification> list = notificationService.GetPage( ownerId(), ownerType() );

            IBlock nblock = getBlock( "notifications" );
            foreach (Notification nf in list.Results) {
                nblock.Set( "notification.Created", nf.Created );
                nblock.Set( "notification.Msg", nf.Msg );

                nblock.Set( "notification.Id", nf.Id );
                nblock.Set( "notification.ReadLink", to( Read, nf.Id ) );


                String readClass = nf.IsRead == 1 ? "nfread" : "";
                nblock.Set( "notification.ReadClass", readClass );

                IBlock fblock = nblock.GetBlock( "friendCmd" );

                nblock.Next();
            }

            set( "page", list.PageBar );
        }

        [HttpPost, DbTransaction]
        public void ReadAll() {

            notificationService.ReadAll( ownerId(), ownerType() );
            echoAjaxOk();
        }

        [HttpPost, DbTransaction]
        public void Read( int id ) {

            Notification nf = notificationService.GetById( id );
            notificationService.Read( id );
            echoAjaxOk();

        }


    }

}
