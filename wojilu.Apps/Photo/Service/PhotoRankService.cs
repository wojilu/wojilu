/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Interface;

namespace wojilu.Apps.Photo.Service {

    public class PhotoRankService : IPhotoRankService {

        // TODO 用户排名
        public virtual List<User> GetTop( int count ) {
            return db.find<User>( "order by Hits desc, id desc" ).list( count );
        }

    }

}
