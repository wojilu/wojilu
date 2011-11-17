/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    public class UserResetPwdSerevice : IUserResetPwdSerevice {

        public virtual UserResetPwd GetByUserAndCode( int userId, String code ) {
            return db.find<UserResetPwd>( "User.Id=userId and Code=:code and IsSet=0" )
                .set( "userId", userId )
                .set( "code", code )
                .first();
        }

        public virtual void Insert( UserResetPwd resetInfo ) {
            db.insert( resetInfo );
        }


        public virtual void UpdateResetSuccess( UserResetPwd resetInfo ) {
            resetInfo.IsSet = 1;
            db.update( resetInfo, "IsSet" );
        }

    }

}
