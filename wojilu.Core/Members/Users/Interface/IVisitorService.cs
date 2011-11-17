/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Members.Users.Interface {


    public interface IVisitorService {

        void Visit( int visitorId, User target );
        List<User> GetRecent( int count, int targetId );

        DataPage<User> GetPage( int targetId, int pageSize );

    }

}
