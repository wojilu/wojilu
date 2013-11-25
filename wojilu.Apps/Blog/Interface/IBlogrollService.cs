/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {


    public interface IBlogrollService {

        List<Blogroll> GetByApp(long appId, long ownerId);
        Blogroll GetById(long id, long appId);
        void Insert(Blogroll roll, long ownerId, long appId);
        void Update( Blogroll roll );
        void Delete( Blogroll roll );

    }

}

