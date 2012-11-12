/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoAlbumService {

        PhotoAlbum GetById( int albumId );
        PhotoAlbum GetById( int id, int ownerId );
        PhotoAlbum GetByIdWithDefault( int id, int ownerId );
        List<PhotoAlbum> GetListByApp( int appId );
        List<PhotoAlbum> GetListByUser( int ownerId );

        Result Create( PhotoAlbum album );
        Result Update( PhotoAlbum album );
        void Delete( PhotoAlbum album );

    }

}
