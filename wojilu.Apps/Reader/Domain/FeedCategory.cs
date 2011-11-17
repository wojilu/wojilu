/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Common.Categories;


namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class FeedCategory : CategoryBase {

        public FeedCategory() {
        }

        public FeedCategory( int id ) {
            this.Id = id;
        }

    }
}
