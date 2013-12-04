/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;

namespace wojilu.Apps.Reader.Service {

    public class FeedCategoryService : IFeedCategoryService {

        public virtual List<FeedCategory> GetByApp(long appId) {
            return db.find<FeedCategory>( "AppId=" + appId + " order by OrderId desc, Id desc" ).list();
        }

        public virtual FeedCategory GetById(long categoryId) {
            return db.findById<FeedCategory>( categoryId );
        }

    }
}
