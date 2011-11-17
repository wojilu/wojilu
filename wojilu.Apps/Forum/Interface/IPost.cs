/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IPost {

        int Id { get; set; }
        String Title { get; set; }

        User Creator { get; set; }
        int OwnerId { get; set; }
        String OwnerType { get; set; }

        int AppId { get; set; }

        DateTime Created { get; set; }
        int Hits { get; set; }

    }

}
