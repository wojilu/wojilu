/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    public class InviteService : IInviteService {
        
        public IUserService userService { get; set; }

        public InviteService() {
            userService = new UserService();
        }

        public virtual String GetCodeByUser( int userId ) {

            UserInviteCode code = UserInviteCode.find( "User.Id=" + userId ).first();

            if (code == null) {
                return createCode( userId );
            }
            else {
                return code.Code;
            }
        }

        private String createCode( int userId ) {

            UserInviteCode c = new UserInviteCode();
            c.User = new User( userId );
            c.Code = Guid.NewGuid().ToString().Replace( "-", "" );
            c.insert();

            return c.Code;
        }

        public virtual Boolean IsCodeValid( int userId, String code ) {

            UserInviteCode c = UserInviteCode.find( "User.Id=" + userId ).first();
            if (c == null) return false;

            return strUtil.EqualsIgnoreCase( c.Code, code );
        }

        public virtual void AddMail( User inviter, List<String> mailList, String mailBody ) {

            foreach (String mail in mailList) {

                if (hasExist( inviter, mail )) continue;

                UserInvite ui = new UserInvite();
                ui.Inviter = inviter;
                ui.MailBody = mailBody;
                ui.ReceiverMail = mail;
                ui.insert();
            }
        }

        private Boolean hasExist( User inviter, String mail ) {
            return UserInvite.find( "Inviter.Id=" + inviter.Id + " and ReceiverMail=:mail" ).set( "mail", mail ).count() > 0;
        }

        public virtual Result Validate( int friendId, String friendCode ) {

            Result result = new Result();

            User user = userService.GetById( friendId );
            if (user == null) {
                result.Add( lang.get( "exUser" ) );
                return result;
            }

            Boolean isCodeCorrect = IsCodeValid( friendId, friendCode );
            if (!isCodeCorrect) {
                result.Add( lang.get( "exInviteCode" ) );
                return result;
            }

            return result;
        }
        

        public void DeleteInvite( UserInvite invite ) {
            invite.delete();
        }

        public List<UserInvite> GetUnSendEmail() {
            return UserInvite.find( "SendStatus=" + UserInviteEmailStatus.UnSend ).list();
        }

        public void SendDone( UserInvite invite ) {
            invite.SendStatus = UserInviteEmailStatus.Done;
            invite.update();
        }

        public void SendError( UserInvite invite ) {
            invite.SendStatus = UserInviteEmailStatus.Failure;
            invite.update();
        }

    }
}
