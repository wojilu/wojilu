/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IBlacklistService {

        IFriendService friendService { get; set; }
        IUserService userService { get; set; }

        Blacklist GetById( int id, int ownerId );
        DataPage<Blacklist> GetPage( int ownerId, int pageSize );

        bool IsBlack( int ownerId, int targetId );

        Result Create( int ownerId, string targetUserName );
        Result Delete( int id, int ownerId );

    }

}
