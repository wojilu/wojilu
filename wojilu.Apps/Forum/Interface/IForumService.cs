/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumService {

        IForumBoardService fbService { get; set; }
        IModeratorService moderatorService { get; set; }

        ForumApp GetById( int id );
        List<ForumTopic> GetStickyTopics( ForumApp forum );

        void SetOnlineUser( ForumApp forum );

        void StickyMoveDown( ForumApp forum, int topicId );
        void StickyMoveUp( ForumApp forum, int topicId );

        void Update( ForumApp forum );
        void UpdateSecurity( ForumApp forum, String str );

    }

}
