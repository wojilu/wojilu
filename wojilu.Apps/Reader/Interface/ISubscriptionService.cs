/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Apps.Reader.Interface {

    public interface ISubscriptionService {

        List<Subscription> GetByApp(long appId);
        List<Subscription> GetByCategoryId(List<Subscription> subscriptionList, long categoryId);
        List<Subscription> GetByCategoryId(long categoryId);


        Subscription GetByFeedAndUser(long feedId, long ownerId);

        string GetFeedIdsByAppId(long appId);
        string GetFeedIdsByCategoryId(long categoryId);

        List<FeedSource> GetFeeds( List<Subscription> subscriptionList );



        DataPage<Subscription> GetPage(long feedId);

        Subscription GetById(long id);

        void Update( Subscription subscription );

        void Delete( Subscription subscription );
    }

}
