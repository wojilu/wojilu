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
        List<ForumBoard> GetBoardAll( int forumId, Boolean isLogin );
        ForumBoard GetById( int id, IMember owner );
        Boolean HasChildren( int boardId );

        int CountPost( int forumBoardId );
        int CountTopic( int forumBoardId );

        Result Insert( ForumBoard fb );

        Result Update( ForumBoard fb );
        void UpdateLogo( ForumBoard board, String newLogo );
        void UpdateSecurity( ForumBoard fb, String str );
        void UpdateSecurityAll( ForumApp forum );
        void UpdateStats( ForumBoard forumBoard );

        void Delete( ForumBoard fb );
        void DeleteCategoryOnly( ForumBoard category );
        void DeleteLogo( ForumBoard board );


        void DeletePostCount( int forumBoardId, IMember owner );

        ForumBoard GetById( int id, int ownerId, String typeFullName );

    }

}
