/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;


namespace wojilu.Apps.Forum.Service {

    public class ForumBuyLogService : IForumBuyLogService {

        public virtual IUserService userService { get; set; }
        public virtual IUserIncomeService incomeService { get; set; }

        public ForumBuyLogService() {
            userService = new UserService();
            incomeService = new UserIncomeService();
        }

        public virtual Result Buy( int buyerId, int creatorId, ForumTopic topic ) {

            if (topic == null) throw new ArgumentNullException( "ForumBuyLogService.Buy" );

            Result result = new Result();
            if (topic.Price <= 0) {
                result.Add( "topic.price <=0" );
                return result;
            }

            if (incomeService.HasEnoughKeyIncome( buyerId, topic.Price ) == false) {
                result.Add( String.Format( alang.get( typeof( ForumApp ), "exIncome" ), KeyCurrency.Instance.Name ) );
                return result;
            }

            // 购买日志
            ForumBuyLog log = new ForumBuyLog();
            log.UserId = buyerId;
            log.TopicId = topic.Id;
            log.insert();

            String msg = string.Format( "访问需要购买的帖子 <a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
            incomeService.AddKeyIncome( buyerId, -topic.Price, msg );

            String msg2 = string.Format( "销售帖子 <a href=\"{0}\">{1}</a>", alink.ToAppData( topic ), topic.Title );
            incomeService.AddKeyIncome( creatorId, topic.Price, msg2 );

            return result;
        }

        public virtual int GetBuyerCount( int topicId ) {
            return db.count<ForumBuyLog>( "TopicId=" + topicId );
        }

        public virtual Boolean HasBuyed( int buyerId, ForumTopic topic ) {
            return (db.count<ForumBuyLog>( "TopicId=" + topic.Id + " and UserId=" + buyerId ) > 0);
        }

    }
}

