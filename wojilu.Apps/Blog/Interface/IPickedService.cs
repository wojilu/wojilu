/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {

    public interface IPickedService {

        DataPage<BlogPost> GetAll();
        BlogPostPicked GetByPost( BlogPost post );
        List<BlogPost> GetTop( int count );
        Boolean IsPicked( BlogPost post );

        void PickPost( String ids );
        void UnPickPost( String ids );

    }

}
