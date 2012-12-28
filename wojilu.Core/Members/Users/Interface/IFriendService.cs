/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Web;

using wojilu.Common.Msg.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common.Msg.Interface;

namespace wojilu.Members.Users.Interface {

    public interface IFriendService {

        INotificationService notificationService { get; set; }
        IUserService userService { get; set; }

        Result CanAddFriend( int userId, int targetId );
        Result AddFriend( int userId, int friendId, String msg, String ip );
        void AddInviteFriend( User user, int friendId );

        void Approve( int userId, int friendId );

        List<User> FindFriends( int userId, int count );
        List<User> FindFriendsByFriends( int userId, int count );
        List<int> FindFriendsIdList( int userId );
        String FindFriendsIds( int userId );

        List<FriendShip> GetFriendsAll( int userId );
        DataPage<User> GetFriendsPage( int userId );
        DataPage<User> GetFriendsPage( int userId, int pageSize );

        DataPage<FriendShip> GetPageByCategory( int userId, int categoryId, int pageSize );
        DataPage<FriendShip> GetPageBySearch( int ownerId, string friendName, int pageSize );

        List<User> GetRecentActive( int count, int userId );

        Boolean IsFriend( int userId, int fid );
        Boolean IsWaitingFriendApproving( int userId, int fid );

        void Refuse( int userId, int friendId );

        void DeleteFriend( int userId, int fid );
        void DeleteFriendByBlacklist( int userId, int fid );
        void CancelAddFriend( int userId, int fid );



        void UpdateCategory( int ownerId, int friendId, int categoryId, string friendDescription );

    }

}
