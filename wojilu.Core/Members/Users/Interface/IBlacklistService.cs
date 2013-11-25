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

        Blacklist GetById(long id, long ownerId);
        DataPage<Blacklist> GetPage(long ownerId, int pageSize);

        bool IsBlack(long ownerId, long targetId);

        Result Create(long ownerId, string targetUserName);
        Result Delete(long id, long ownerId);

    }

}
