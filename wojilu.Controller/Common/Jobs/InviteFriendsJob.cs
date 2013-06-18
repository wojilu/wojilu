/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Threading;

using wojilu.Net;
using wojilu.Data;
using wojilu.Web.Jobs;
using wojilu.Web.Utils;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Common {

    public class InviteFriendsJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ConfirmEmailJob ) );
        private static Random rd = new Random();

        public IUserService userService { get; set; }
        public IInviteService inviteService { get; set; }

        public InviteFriendsJob() {
            userService = new UserService();
            inviteService = new InviteService();
        }

        public void Execute() {

            if (config.Instance.Site.EnableEmail == false) return;

            List<UserInvite> unsendInvites = inviteService.GetUnSendEmail();
            if (unsendInvites.Count > 0)
                logger.Info( "UnSend Invites Count：" + unsendInvites.Count );

            foreach (UserInvite invite in unsendInvites) {
                sendInviteEmail( invite );
                int sleepSecond = rd.Next( 10, 60 );
                Thread.Sleep( sleepSecond * 1000 );
            }
        }

        public void End() {
        }

        private void sendInviteEmail( UserInvite invite ) {

            if (System.Text.RegularExpressions.Regex.IsMatch( invite.ReceiverMail, RegPattern.Email ) == false) {
                inviteService.DeleteInvite( invite );
                logger.Info( lang.get( "exEmailFormat" ) + ": " + invite.Inviter.Name + "[" + invite.ReceiverMail + "]" );
                return;
            }

            MailClient mail = MailClient.Init();

            String title = invite.Inviter.Name + lang.get( "inviteMailTitle" );
            String msg = invite.MailBody;

            Result sentResult = mail.Send( invite.ReceiverMail, title, msg );

            if (sentResult.IsValid) {
                inviteService.SendDone( invite );
                logger.Info( lang.get( "inviteSendDone" ) + ": " + invite.Inviter.Name + "[" + invite.ReceiverMail + "]" );
            }
            else {
                inviteService.SendError( invite );
                logger.Info( lang.get( "inviteSendFailure" ) + ": " + invite.Inviter.Name + "[" + invite.ReceiverMail + "]" );
            }
        }


    }

}
