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

        List<ForumRateLog> GetByPost( int postId );
        ForumRateLog GetByPostAndOperator( int userId, int postId );

        void Insert(long postId, User operateUser, long currencyId, int income, string reason);

        Boolean HasRate( int operatorId, int postId );

    }

}
