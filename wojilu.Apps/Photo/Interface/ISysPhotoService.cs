/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;
using wojilu.Common;

namespace wojilu.Apps.Photo.Interface {

    public interface ISysPhotoService {

        List<IBinderValue> GetNewAll( int count );

        List<PhotoPost> GetSysHits( int count );
        DataPage<PhotoPost> GetSysPostPage( int categoryId, int pageSize );
        DataPage<PhotoPost> GetSysPostTrashPage( int pageSize );
        int GetSystemDeleteCount();

        List<PhotoPost> GetSysTop( int categoryId, int count );

        void SystemDelete( PhotoPost post );
        void SystemUnDelete( PhotoPost post );

        void SystemDeleteList( string ids );
        void SystemUnDeleteList( string ids );

        void SystemDeleteListTrue( string ids );

    }

}
