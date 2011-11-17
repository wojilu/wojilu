/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumRateService {

        UserIncomeLog GetByOperatorAndPost( User user, int postId );
        List<UserIncomeLog> GetByPost( int postId );

        void Insert( int postId, int userId, int operatorId, String operatorName, int currencyId, int income, String reason );

        Boolean IsUserRate( User user, int postId );

    }

}
