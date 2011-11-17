/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Visitors{

    public interface IDataVisitor {

        void setVisitor( IMember member );
        void setTarget( IAppData data );

        IUser getVisitor();
        Type getTargetType();

    }

}
