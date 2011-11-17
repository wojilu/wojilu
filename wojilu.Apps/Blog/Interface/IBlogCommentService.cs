/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogCommentService {

        List<BlogPostComment> GetByPost( int blogPostId );
        void DeleteBatch( String ids );
        DataPage<BlogPostComment> GetPageAll( String condition );

    }

}
