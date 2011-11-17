/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Apps.Content.Domain;

namespace wojilu.Apps.Content.Interface {

    public interface ISysPostService {

        ContentPost GetById_ForAdmin( int id );
        DataPage<ContentPost> GetPage();
        DataPage<ContentPost> GetPageTrash();
        int GetDeleteCount();

        void Delete( String ids );
        void DeleteTrue( String ids );
        void UnDelete( ContentPost post );

    }

}
