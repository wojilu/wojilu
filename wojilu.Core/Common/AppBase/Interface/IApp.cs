/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.AppBase.Interface {

    public interface IApp {

        int Id { get; set; }

        int OwnerId { get; set; }
        String OwnerUrl { get; set; }
        String OwnerType { get; set; }

    }

}

