/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Photo.Interface {


    public interface IPhotoRankService {

        List<User> GetTop( int count );
    }


}
