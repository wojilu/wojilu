/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;

namespace wojilu.Common.Comments {

    public interface IComment {

        int Id { get; set; }
        int AppId { get; set; }

        int RootId { get; set; }
        int ParentId { get; set; }

        User Member { get; set; }

        String Author { get; set; }
        String Title { get; set; }
        String Content { get; set; }

        String Ip { get; set; }

        int Replies { get; set; }


        DateTime Created { get; set; }

        Type GetTargetType();

        void AddFeedInfo( String lnkTarget );
        void AddNotification( String lnkTarget );

    }
}

