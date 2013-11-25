/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumLinkService {

        List<ForumLink> GetByApp(long appId, long ownerId);
        ForumLink GetById(long id, IMember owner);

        Result Insert( ForumLink link );
        Result Update( ForumLink link );

        void Delete( ForumLink link );

    }

}
