/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;
using wojilu.Common;
using wojilu.Web.Context;

namespace wojilu.Members.Users.Interface {

    public interface ILoginService {

        void Login( User user, LoginTime expiration, String ip, MvcContext ctx );

        void Login( User user, int userConnectId, LoginTime loginTime, string ip, MvcContext ctx );

        void UpdateLastLogin( User user, String ip );


    }

}
