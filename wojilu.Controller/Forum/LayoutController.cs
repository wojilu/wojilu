/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum {

    public class LayoutController : ControllerBase {

        public override void Layout() {

            ForumApp app = ctx.app.obj as ForumApp;

            set( "adminUrl", to( new Admin.ForumController().Index ) );
            set( "appUrl", to( new ForumController().Index ) );

            set( "adminCheckUrl", t2( new wojilu.Web.Controller.SecurityController().CanAppAdmin, app.Id ) + "?appType=" + typeof( ForumApp ).FullName );

        }
    }

}
