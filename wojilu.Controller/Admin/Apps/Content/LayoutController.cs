/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;


namespace wojilu.Web.Controller.Admin.Apps.Content {

    [App( typeof( ContentApp ) )]
    public class LayoutController : ControllerBase {

        public SysPostService sysblogService { get; set; }

        public LayoutController() {
            sysblogService = new SysPostService();
        }

        public override void Layout() {
            set( "listLink", to( new MainController().Index ) );
            set( "trashLink", to( new MainController().Trash ) );
            set( "commentLink", to( new CommentController().List ) );

            int trashCount = sysblogService.GetDeleteCount();
            set( "trashCount", trashCount );
        }


    }

}
