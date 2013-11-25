/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoAlbumService {

        PhotoAlbum GetById(long albumId);
        PhotoAlbum GetById(long id, long ownerId);
        PhotoAlbum GetByIdWithDefault(long id, long ownerId);
        List<PhotoAlbum> GetListByApp(long appId);
        List<PhotoAlbum> GetListByUser(long ownerId);

        Result Create( PhotoAlbum album );
        Result Update( PhotoAlbum album );
        void Delete( PhotoAlbum album );

    }

}
