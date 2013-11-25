/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Groups.Interface {

    public interface IGroupFriendService {

        Result AddFriend( IMember group, String name );
        GroupFriends GetFriend(long groupId, long friendId);
        List<Group> GetFriends(long groupId, int count);

        DataPage<Group> GetPage(long groupId, int pageSize);

        void Delete( GroupFriends gf );

    }

}
