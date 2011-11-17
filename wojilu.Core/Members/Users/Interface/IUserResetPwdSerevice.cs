/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IUserResetPwdSerevice {

        UserResetPwd GetByUserAndCode( int userId, String code );
        void Insert( UserResetPwd resetInfo );
        void UpdateResetSuccess( UserResetPwd resetInfo );

    }

}
