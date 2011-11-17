/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;


namespace wojilu.Apps.Forum.Interface {

    public interface IForumCategoryService {

        List<ForumCategory> GetByBoard( int forumBoardId );
        ForumCategory GetById( int id, IMember owner );
        List<ForumCategory> GetDropList( int boardId );

        int CountByBoard( int boardId );

        Result Insert( ForumCategory category );
        Result Update( ForumCategory category );
        void Delete( ForumCategory category );

    }

}
