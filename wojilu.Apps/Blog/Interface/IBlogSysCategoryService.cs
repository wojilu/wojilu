/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogSysCategoryService {

        List<BlogSysCategory> GetAll();
        BlogSysCategory GetById( int id );
        List<BlogSysCategory> GetForDroplist();

    }

}
