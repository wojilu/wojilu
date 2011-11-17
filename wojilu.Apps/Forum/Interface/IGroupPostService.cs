/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IGroupPostService {

        List<ForumTopic> GetHotTopic( int count );
        List<ForumPost> GetRecent( int count );

        DataPage<ForumTopic> GetTopicAll( String condition );
        DataPage<ForumPost> GetPostAll( String condition );

        List<ForumTopic> GetMyTopic( int userId, String groupIds, int count );
        DataPage<ForumTopic> GetMyTopicPage( int userId, String groupIds, int count );

    }

}
