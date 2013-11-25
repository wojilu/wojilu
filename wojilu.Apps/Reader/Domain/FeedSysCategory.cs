/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Common.Categories;


namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class FeedSysCategory : CategoryBase {

        public FeedSysCategory() {
        }

        public FeedSysCategory( long id ) {
            this.Id = id;
        }

    }
}
