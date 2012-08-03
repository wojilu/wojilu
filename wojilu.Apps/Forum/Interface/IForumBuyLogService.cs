/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Common.Money.Service;
using wojilu.Members.Users.Interface;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Money.Interface;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumBuyLogService {

        IUserIncomeService incomeService { get; set; }
        IUserService userService { get; set; }

        Result Buy( int buyerId, int creatorId, ForumTopic topic );
        int GetBuyerCount( int topicId );
        Boolean HasBuyed( int buyerId, ForumTopic topic );

    }

}
