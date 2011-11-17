/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.AppBase.Interface {

    public interface IExtViewService {
        String GetViewById( int topicId, String typeFullName, MvcContext ctx );

    }

}
