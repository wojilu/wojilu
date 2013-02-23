using System;
using wojilu.Members.Users.Domain;

namespace wojilu.OAuth {

    public interface IUserConnectService {

        UserConnect GetById( int id );

        bool HasBind( int userId, string typeFullName );
        Result Create( User user, string connectType, AccessToken token );

        UserConnect GetConnectInfo( int userId, string connectType );
        UserConnect GetConnectInfo( string connectUid, string connectType );

        Result UnBind( int userId, string connectType );
        Result Sync( int userId, string connectType, int isSync );


    }

}
