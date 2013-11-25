/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumBoardService {

        IForumService forumService { get; set; }

        void ClearSecurity( ForumBoard fb );
        void Combine( ForumBoard fbSrc, ForumBoard fbTarget );
        List<ForumBoard> GetBoardAll(long forumId, bool isLogin);
        ForumBoard GetById(long id, IMember owner);
        bool HasChildren(long boardId);

        int CountPost(long forumBoardId);
        int CountTopic(long forumBoardId);

        Result Insert( ForumBoard fb );

        Result Update( ForumBoard fb );
        void UpdateLogo( ForumBoard board, String newLogo );
        void UpdateSecurity( ForumBoard fb, String str );
        void UpdateSecurityAll( ForumApp forum );
        void UpdateStats( ForumBoard forumBoard );

        void Delete( ForumBoard fb );
        void DeleteCategoryOnly( ForumBoard category );
        void DeleteLogo( ForumBoard board );


        void DeletePostCount(long forumBoardId, IMember owner);

        ForumBoard GetById(long id, long ownerId, string typeFullName);

    }

}
