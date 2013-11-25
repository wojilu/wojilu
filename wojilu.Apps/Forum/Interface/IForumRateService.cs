/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumRateService {

        List<ForumRateLog> GetByPost(long postId);
        ForumRateLog GetByPostAndOperator(long userId, long postId);

        void Insert(long postId, User operateUser, long currencyId, int income, string reason);

        bool HasRate(long operatorId, long postId);

    }

}
