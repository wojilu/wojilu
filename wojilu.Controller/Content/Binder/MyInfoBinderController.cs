/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Apps.Content.Interface;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Users;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyInfoBinderController : ControllerBase, ISectionBinder {

        public void Bind( ContentSection section, IList serviceData ) {

            User user = ctx.owner.obj as User;
            if (user == null) return;

            bind( "user", new UserVo( user ) );
        }

    }

}
