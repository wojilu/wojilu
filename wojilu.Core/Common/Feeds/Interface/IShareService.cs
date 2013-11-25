/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Feeds.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Feeds.Interface {

    public interface IShareService {

        Share GetById(long id);
        Share GetByIdWithComments(long id);
        ShareComment GetCommentById(long id);

        List<Share> GetByUser(int count, long userId);

        DataPage<Share> GetPageByUser(long userId, int pageSize);
        DataPage<Share> GetFriendsPage(long userId, int pageSize);
        DataPage<Share> GetPageAll();

        Result Create( Share share );
        Result CreateUrl( User creator, String shareLink, String shareDescription );
        Boolean IsShared( Share share );


        void InsertComment( ShareComment c, String shareLink, String parentShareLink );


        Result Delete(long id);
    }

}
