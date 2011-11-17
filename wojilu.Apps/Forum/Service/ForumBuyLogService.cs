/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Service;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Service {


    public class ForumBuyLogService : IForumBuyLogService {

        public virtual IUserService userService { get; set; }
        public virtual IUserIncomeService userIncomeService { get; set; }


        public ForumBuyLogService() {
            userService = new UserService();
            userIncomeService = new UserIncomeService();
        }

        public virtual Result Buy( int buyerId, int creatorId, ForumTopic topic ) {

            Result result = new Result();
            if (userIncomeService.HasEnoughKeyIncome( buyerId, topic.Price ) == false) {
                result.Add( String.Format( alang.get( typeof( ForumApp ), "exIncome" ), KeyCurrency.Instance.Name ) );
                return result;
            }
            
            // 日志：买方减少收入
            UserIncomeLog log = new UserIncomeLog();
            log.UserId = buyerId;
            log.CurrencyId = KeyCurrency.Instance.Id;
            log.Income = -topic.Price;
            log.DataId = topic.Id;
            log.ActionId = actionId;
            db.insert( log );

            // 日志：卖方增加收入
            UserIncomeLog log2 = new UserIncomeLog();
            log2.UserId = creatorId;
            log2.CurrencyId = KeyCurrency.Instance.Id;
            log2.Income = topic.Price;
            log2.DataId = topic.Id;
            log2.ActionId = actionId;
            db.insert( log2 );

            userIncomeService.AddKeyIncome( buyerId, -topic.Price );
            userIncomeService.AddKeyIncome( creatorId, topic.Price );

            return result;
        }

        public virtual int GetBuyerCount( int topicId ) {
            return db.count<UserIncomeLog>( "DataId=" + topicId + " and ActionId=" + actionId );
        }

        public virtual Boolean HasBuyed( int buyerId, ForumTopic topic ) {
            return ( db.count<UserIncomeLog>( "DataId=" + topic.Id + " and UserId=" + buyerId + " and ActionId=" + actionId ) > 0);
        }

        public static int actionId = 101; // 0x65


    }
}

