/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Reader.Domain;
using wojilu.Apps.Reader.Interface;

namespace wojilu.Apps.Reader.Service {

    public class FeedSysCategoryService : IFeedSysCategoryService {

        public List<FeedSysCategory> GetAll() {
            return db.find<FeedSysCategory>( " order by OrderId desc, Id desc" ).list();
        }

        public FeedSysCategory GetById( int categoryId ) {
            return db.findById<FeedSysCategory>( categoryId );
        }

    }
}
