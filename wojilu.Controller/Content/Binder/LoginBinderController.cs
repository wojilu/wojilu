/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Users.Admin;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class LoginBinderController : ControllerBase, ISectionBinder{

        public void Bind( ContentSection section, IList serviceData ) {

            set( "loginScriptLink", t2( new MainController().LoginScript ) );
        }

    }
}
