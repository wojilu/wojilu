/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Users.Interface {


    public interface IVisitorService {

        void Visit( long visitorId, User target );
        List<User> GetRecent(int count, long targetId);

        DataPage<User> GetPage( long targetId, int pageSize );

    }

}
