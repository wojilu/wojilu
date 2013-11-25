/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {


    public interface IBlogCategoryService {

        List<BlogCategory> GetByApp(long appId);
        BlogCategory GetById(long id, long ownerId);
        void Insert( BlogCategory category );
        void Delete( BlogCategory category );

        void RefreshCache( BlogCategory category );

    }
}

