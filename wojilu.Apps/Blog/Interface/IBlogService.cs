/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogService {

        void AddHits( BlogApp app );
        List<BlogApp> GetBlogAppAll();
        void UpdateCommentCount( int appId );
        void UpdateCommentCount( IApp blogApp );
        void UpdateCount( BlogApp blog );

    }

}
