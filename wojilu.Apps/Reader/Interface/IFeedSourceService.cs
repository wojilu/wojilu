/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Reader.Domain;
using wojilu.Members.Users.Domain;
using System.Collections.Generic;
using wojilu.Common;

namespace wojilu.Apps.Reader.Interface {

    public interface IFeedSourceService {

        IFeedEntryService entryService { get; set; }
        ISubscriptionService subscriptionService { get; set; }

        List<IBinderValue> GetFeed( String url, int count );

        Subscription Create( String url, FeedCategory category, String name, int orderId, User user );
        FeedSource CreateRss( String url );
        FeedSource CreateRss( String url, String name, FeedSysCategory category );

        void DownloadBlogItems( FeedSource f );
        FeedSource GetByLink( String rssLink );
        FeedSource GetById( int feedId );

        List<FeedSource> GetUnRefreshedList( int minutes );
        DataPage<FeedSource> GetPage( int pageSize );
        List<FeedSource> GetAll( );

        void Update( FeedSource f );
        Result Delete( FeedSource f );


    }

}
