/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Interface {

    public interface IAdminLog {

        User User { get; set; }

        int CategoryId { get; set; }
        String Message { get; set; }

        String DataInfo { get; set; }
        String DataType { get; set; }

        String Ip { get; set; }

        DateTime Created { get; set; }

    }

}
