/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Apps.Reader.Interface {

    public interface ISubscriptionService {

        List<Subscription> GetByApp( int appId );
        List<Subscription> GetByCategoryId( List<Subscription> subscriptionList, int categoryId );
        List<Subscription> GetByCategoryId( int categoryId );


        Subscription GetByFeedAndUser( int feedId, int ownerId );

        String GetFeedIdsByAppId( int appId );
        String GetFeedIdsByCategoryId( int categoryId );

        List<FeedSource> GetFeeds( List<Subscription> subscriptionList );



        DataPage<Subscription> GetPage( int feedId );

        Subscription GetById( int id );

        void Update( Subscription subscription );

        void Delete( Subscription subscription );
    }

}
