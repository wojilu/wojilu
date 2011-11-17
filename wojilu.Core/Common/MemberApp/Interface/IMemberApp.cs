/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;

namespace wojilu.Common.MemberApp.Interface {

    public interface IMemberApp : ISort {

        AppInstaller AppInfo { get; }

        int AppInfoId { get; set; }
        int AppOid { get; set; }

        int OwnerId { get; set; }
        String OwnerType { get; set; }
        String OwnerUrl { get; set; }

        User Creator { get; set; }
        String CreatorUrl { get; set; }


        String Name { get; set; }
        int IsStop { get; set; }
        String Status { get; }

        DateTime Created { get; set; }
        int AccessStatus { get; set; }


    }
}

