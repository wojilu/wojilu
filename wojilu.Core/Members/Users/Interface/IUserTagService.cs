using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IUserTagService {

        UserTag GetTagById( int id );
        UserTag GetTagByName( string tName );
        UserTagShip GetById( int id );

        List<UserTagShip> GetPage( int ownerId );
        DataPage<User> GetPageByTag( int tagId );

        void SaveTags( string tagList, int viewerId, User owner );

        void DeleteUserTag( UserTagShip u );

    }

}
