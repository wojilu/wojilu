using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IInviteService {

        string GetCodeByUser( int userId );
        bool IsCodeValid( int userId, string code );

        void AddMail( User inviter, List<string> mailList, string mailBody );

        Result Validate( int friendId, string friendCode );


        List<UserInvite> GetUnSendEmail();

        void SendDone( UserInvite invite );
        void SendError( UserInvite invite );
        void DeleteInvite( UserInvite invite );

    }

}
