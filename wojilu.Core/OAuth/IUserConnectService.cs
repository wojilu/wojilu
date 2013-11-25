using System;
using wojilu.Members.Users.Domain;

namespace wojilu.OAuth {

    public interface IUserConnectService {

        UserConnect GetById(long id);

        bool HasBind(long userId, string typeFullName);
        Result Create( User user, string connectType, AccessToken token );

        UserConnect GetConnectInfo(long userId, string connectType);
        UserConnect GetConnectInfo( string connectUid, string connectType );

        Result UnBind(long userId, string connectType);
        Result Sync(long userId, string connectType, int isSync);


    }

}
