/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IPost {

        long Id { get; set; }
        String Title { get; set; }

        User Creator { get; set; }
        long OwnerId { get; set; }
        String OwnerType { get; set; }

        long AppId { get; set; }

        DateTime Created { get; set; }
        int Hits { get; set; }

    }

}
