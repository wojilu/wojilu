using System;
using wojilu.Members.Users.Domain;
namespace wojilu.Members.Users.Interface {

    public interface IUserErrorPicService {

        void AddLog( User user, string ip );

        void ApproveError( string ids, string reviewMsg );
        void ApproveOk( string ids, string reviewMsg );

        int CheckErrorCount( User user );
        string GetLastReviewMsg( User user );
        int GetStatus( User user );
        void UpdateLastUpload( User user, string ip );
    }

}
