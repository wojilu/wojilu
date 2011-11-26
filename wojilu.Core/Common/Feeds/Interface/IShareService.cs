/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Feeds.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Feeds.Interface {

    public interface IShareService {

        Share GetById( int id );
        Share GetByIdWithComments( int id );
        ShareComment GetCommentById( int id );

        List<Share> GetByUser( int count, int userId );

        DataPage<Share> GetPageByUser( int userId, int pageSize );
        DataPage<Share> GetFriendsPage( int userId, int pageSize );
        DataPage<Share> GetPageAll();

        Result Create( Share share );
        Result CreateUrl( User creator, String shareLink, String shareDescription );
        Boolean IsShared( Share share );


        void InsertComment( ShareComment c, String shareLink, String parentShareLink );


        Result Delete( int id );
    }

}
