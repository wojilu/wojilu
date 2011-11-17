/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class LayoutController : ControllerBase {

        public ISysBlogService sysblogService { get; set; }

        public LayoutController() {
            sysblogService = new SysBlogService();
        }

        public override void Layout() {
            set( "listLink", to( new MainController().Index ) );
            set( "pickedLink", to( new MainController().Picked ) );
            set( "trashLink", to( new MainController().Trash ) );
            set( "commentLink", to( new CommentController().List )+"?type=" + typeof(BlogPostComment).FullName );

            int trashCount = sysblogService.GetSystemDeleteCount();
            set( "trashCount", trashCount );
        }


    }

}
