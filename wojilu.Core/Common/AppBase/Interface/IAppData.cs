/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;

namespace wojilu.Common.AppBase.Interface {

    public interface IAppData {

        int Id { get; set; }
        int AppId { get; set; }

        User Creator { get; set; }
        String CreatorUrl { get; set; }
        DateTime Created { get; set; }

        int OwnerId { get; set; }
        String OwnerType { get; set; }
        String OwnerUrl { get; set; }

        String Title { get; set; }

        int AccessStatus { get; set; }
        String Ip { get; set; }

    }
}

