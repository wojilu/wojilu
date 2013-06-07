/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller {


    public class LinkInstaller : BaseInstaller {

        public void AddBlog( MvcContext ctx, String lnkName, String fUrl ) {
            base.AddMenu( ctx, lnkName, lnkFull( ctx, Link.To( new Blog.MainController().Index ) ), fUrl );
        }

        public void AddPhoto( MvcContext ctx, String lnkName, String fUrl ) {
            base.AddMenu( ctx, lnkName, lnkFull( ctx, Link.To( new Photo.MainController().Index ) ), fUrl );
        }

        public void AddUser( MvcContext ctx, String lnkName, String fUrl ) {
            base.AddMenu( ctx, lnkName, lnkFull( ctx, Link.To( new Users.MainController().Index ) ), fUrl );
        }

        public void AddGroup( MvcContext ctx, String lnkName, String fUrl ) {
            base.AddMenu( ctx, lnkName, lnkFull( ctx, Link.To( new Web.Controller.Groups.MainController().Index ) ), fUrl );
        }

        public void AddTag( MvcContext ctx, String lnkName, String fUrl ) {
            base.AddMenu( ctx, lnkName, lnkFull( ctx, Link.To( new TagController().Index ) ), fUrl );
        }

    }
}
