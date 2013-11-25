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

        Result Buy(long buyerId, long creatorId, ForumTopic topic);
        int GetBuyerCount(long topicId);
        bool HasBuyed(long buyerId, ForumTopic topic);

    }

}
