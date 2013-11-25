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
        string GetViewById(long topicId, string typeFullName, MvcContext ctx);

    }

}
