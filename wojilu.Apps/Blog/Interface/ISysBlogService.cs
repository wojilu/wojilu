/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Apps.Blog.Interface {

    public interface ISysBlogService {

        List<BlogPost> GetSysHit( int count );
        List<BlogPost> GetSysNew(long categoryId, int count);

        DataPage<BlogPost> GetSysPage( int size );
        DataPage<BlogPost> GetSysPageByCategory(long categoryId, int size);
        DataPage<BlogPost> GetSysPageBySearch( String condition );
        DataPage<BlogPost> GetSysPageTrash();
        List<BlogPost> GetSysReply( int count );

        int GetSystemDeleteCount();

        void SystemDelete( BlogPost post );
        void SystemUnDelete( BlogPost post );

        void Delete( string ids );
        void DeleteTrue( string ids );
        void UnDelete( string ids );

        List<BlogPost> GetByCategory(long categoryId, int count);
    }

}
