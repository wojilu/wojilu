using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IUserTagService {

        UserTag GetTagById(long id);
        UserTag GetTagByName( string tName );
        UserTagShip GetById(long id);

        List<UserTagShip> GetPage(long ownerId);
        DataPage<User> GetPageByTag(long tagId);

        void SaveTags(string tagList, long viewerId, User owner);

        void DeleteUserTag( UserTagShip u );

    }

}
