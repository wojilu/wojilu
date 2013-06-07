/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Context;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Interface;

namespace wojilu.Common.AppBase.Interface {

    public interface IAppInstaller {
        IMemberApp Install( MvcContext ctx, IMember owner, String appName, AccessStatus accessStatus, String themeId, String friendlyUrl );
    }

}
