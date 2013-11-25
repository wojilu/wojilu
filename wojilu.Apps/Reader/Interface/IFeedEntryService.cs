/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Reader.Domain;
namespace wojilu.Apps.Reader.Interface {

    public interface IFeedEntryService {

        FeedEntry GetByLink( String entryLink );


        DataPage<FeedEntry> GetPage(long feedId);
        DataPage<FeedEntry> GetPage( String feedIds );

        FeedEntry GetById(long id);

        void AddHits( FeedEntry entry );
    }

}
