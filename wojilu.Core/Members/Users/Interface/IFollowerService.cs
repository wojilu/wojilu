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

        Follower Follow(long userId, long targetId, string ip);
        void FollowWithFeedNotification(long userId, long targetId, string ip);

        DataPage<User> GetFollowingPage(long userId);
        DataPage<User> GetFollowingPage(long userId, int pageSize);
        DataPage<User> GetFollowersPage(long targetId);

        List<User> GetRecentFollowers(long targetId, int count);
        List<User> GetRecentFollowing(long userId, int count);
        List<User> GetRecentFriendsAndFollowers(long targetId, int count);

        string GetFollowingIds(long userId);

        bool IsFollowing(long userId, long targetId);

        Result DeleteFollow(long userId, long targetId);

    }

}
