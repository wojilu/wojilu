using System;
using wojilu.Members.Users.Domain;
namespace wojilu.Members.Users.Interface {

    public interface IUserErrorPicService {

        void AddLog( User user, string ip );
        void AddLogAndPass( User user, string ip );

        void ApproveError( string ids, string reviewMsg, int isNextApprove, int isDelete );
        void ApproveOk( string ids, string reviewMsg );

        int CheckErrorCount( User user );
        string GetLastReviewMsg( User user );
        int GetStatus( User user );
        void UpdateLastUpload( User user, string ip );


        UserErrorPic GetLastLog( User user );

    }

}
