/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {


    public interface IBlogrollService {

        List<Blogroll> GetByApp( int appId, int ownerId );
        Blogroll GetById( int id, int appId );
        void Insert( Blogroll roll, int ownerId, int appId );
        void Update( Blogroll roll );
        void Delete( Blogroll roll );

    }

}

