/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Apps.Forum.Interface {

    public interface IModeratorService {

        IUserService UserService { get; set; }

        void AddModerator( ForumBoard fb, String moderatorName );

        List<User> GetModeratorList( ForumBoard fb );
        String GetModeratorText( String moderatorRawPerperty );
        String GetModeratorHtml( ForumBoard fb );
        String[] GetModeratorNames( ForumBoard fb );
        String GetModeratorJson( ForumBoard fb );

        Boolean IsModerator( int appId, String memberName );
        Boolean IsModerator( ForumBoard fb, String memberName );
        Boolean IsModerator( ForumBoard fb, User user );


        void DeleteModerator( ForumBoard fb, String moderatorName );

    }

}
