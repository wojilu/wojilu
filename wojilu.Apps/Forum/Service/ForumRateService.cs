/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Common.Money.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Apps.Forum.Service {


    public class ForumRateService : IForumRateService {

        public virtual List<ForumRateLog> GetByPost(long postId) {
            return db.find<ForumRateLog>( "PostId=" + postId ).list();
        }

        public virtual ForumRateLog GetByPostAndOperator(long userId, long postId) {
            return db.find<ForumRateLog>( "PostId=" + postId + " and UserId=" + userId ).first();
        }

        public virtual void Insert(long postId, User operateUser, long currencyId, int income, string reason) {

            ForumRateLog log = new ForumRateLog();

            log.User = operateUser;

            log.PostId = postId;

            log.CurrencyId = currencyId;
            log.Income = income;

            log.Reason = reason;

            db.insert( log );
        }

        public virtual bool HasRate(long operatorId, long postId) {
            return GetByPostAndOperator( operatorId, postId ) != null;
        }


    }
}

