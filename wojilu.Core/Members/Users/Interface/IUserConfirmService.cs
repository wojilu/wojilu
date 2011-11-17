/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;

namespace wojilu.Members.Users.Interface {

    public interface IUserConfirmService {

        void AddConfirm( UserConfirm uc );
        //void AddConfirm( String code );

        User Valid( String code );

        Result CanSend( User user );
    }


}
