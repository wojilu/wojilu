/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Reader.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Reader {

    [App( typeof( ReaderApp ) )]
    public class LayoutController : ControllerBase {


        public LayoutController() {
        }

        public override void Layout() {

            set( "listLink", to( new MainController().Index ) );

            set( "categoryLink", to( new SysCategoryController().List ) );
        }


    }

}
