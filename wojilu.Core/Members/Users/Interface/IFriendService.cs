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

        Result CanAddFriend( long userId, long targetId );
        Result AddFriend( long userId, long friendId, String msg, String ip );
        void AddInviteFriend( User user, long friendId );

        void Approve( long userId, long friendId );

        List<User> FindFriends( long userId, int count );
        List<User> FindFriendsByFriends( long userId, int count );
        List<long> FindFriendsIdList(long userId);
        String FindFriendsIds( long userId );

        List<FriendShip> GetFriendsAll( long userId );
        DataPage<User> GetFriendsPage( long userId );
        DataPage<User> GetFriendsPage( long userId, int pageSize );

        DataPage<FriendShip> GetPageByCategory( long userId, long categoryId, int pageSize );
        DataPage<FriendShip> GetPageBySearch( long ownerId, string friendName, int pageSize );

        List<User> GetRecentActive( int count, long userId );

        Boolean IsFriend( long userId, long fid );
        Boolean IsWaitingFriendApproving( long userId, long fid );

        void Refuse( long userId, long friendId );

        void DeleteFriend( long userId, long fid, String ip );
        void DeleteFriendByBlacklist( long userId, long fid );
        void CancelAddFriend( long userId, long fid );



        void UpdateCategory( long ownerId, long friendId, long categoryId, string friendDescription );

    }

}
