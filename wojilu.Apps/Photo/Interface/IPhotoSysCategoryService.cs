/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Apps.Photo.Interface {

    public interface IPhotoSysCategoryService {

        List<PhotoSysCategory> GetAll();
        PhotoSysCategory GetById( int id );
        List<PhotoSysCategory> GetForDroplist();

    }

}
