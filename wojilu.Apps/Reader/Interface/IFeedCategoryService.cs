/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Apps.Reader.Interface {

    public interface IFeedCategoryService {

        List<FeedCategory> GetByApp( int appId );
        FeedCategory GetById( int categoryId );
    }

}
