/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Interface {

    public interface IFollowerService {

        IFriendService friendService { get; set; }
        IUserService userService { get; set; }

        Follower Follow( int userId, int targetId );
        void FollowWithFeedNotification( int userId, int targetId );

        DataPage<User> GetFollowingPage( int userId );
        DataPage<User> GetFollowingPage( int userId, int pageSize );
        DataPage<User> GetFollowersPage( int targetId );

        List<User> GetRecentFollowers( int targetId, int count );
        List<User> GetRecentFollowing( int userId, int count );
        List<User> GetRecentFriendsAndFollowers( int targetId, int count );

        String GetFollowingIds( int userId );

        Boolean IsFollowing( int userId, int targetId );

        Result DeleteFollow( int userId, int targetId );

    }

}
