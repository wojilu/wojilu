/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Common.Money.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ForumRateService : IForumRateService {

        public virtual UserIncomeLog GetByOperatorAndPost( User user, int postId ) {
            return db.find<UserIncomeLog>( "DataId=" + postId + " and OperatorId=" + user.Id + " and ActionId=" + 100 ).first();
        }

        public virtual List<UserIncomeLog> GetByPost( int postId ) {
            return db.find<UserIncomeLog>( "DataId=" + postId + " and ActionId=" + 100 ).list();
        }

        public virtual void Insert( int postId, int userId, int operatorId, String operatorName, int currencyId, int income, String reason ) {

            UserIncomeLog log = new UserIncomeLog();
            log.UserId = userId;
            log.CurrencyId = currencyId;
            log.Income = income;
            log.DataId = postId;
            log.OperatorId = operatorId;
            log.OperatorName = operatorName;
            log.Note = reason;
            log.ActionId = 100;

            db.insert(log);
        }

        public virtual Boolean IsUserRate( User user, int postId ) {
            return (this.GetByOperatorAndPost( user, postId ) != null);
        }
    }
}

