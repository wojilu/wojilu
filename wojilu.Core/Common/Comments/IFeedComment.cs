/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Comments {

    public interface IFeedComment {

        int Id { get; set; }
        User User { get; set; }
        int RootId { get; set; }
        int ParentId { get; set; }
        String Content { get; set; }
        DateTime Created { get; set; }
        String Ip { get; set; }

    }

}
