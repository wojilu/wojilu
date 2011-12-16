/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;


namespace wojilu.Web.Controller.Shop.Admin.Section {

    [App( typeof( ShopApp ) )]
    public class LayoutController : ControllerBase {

        public override void Layout() {

            set( "lnkIndex", to( new ShopController().Index ) );
        }

    }

}
