/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPickedService {

        PhotoPostPicked GetByPost( PhotoPost post );

        Boolean IsPicked( PhotoPost post );

        void PickPost( String ids );
        void UnPickPost( String ids );

        void DeletePhoto( String ids );


        List<PhotoPost> GetTop( int count );
        DataPage<PhotoPost> GetAll();
        DataPage<PhotoPost> GetAll( int pageSize );

        DataPage<PhotoPost> GetShowAll( int pageSize );
    }

}
