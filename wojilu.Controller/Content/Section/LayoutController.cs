/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Common;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Section {

    public partial class LayoutController : ControllerBase {

        public override void Layout() {
            set( "lnkSidebar", clink.toSidebar( ctx ) );
            set( "adSidebarTop", AdItem.GetAdById( AdCategory.ArticleSidebarTop ) );
            set( "adSidebarBottom", AdItem.GetAdById( AdCategory.ArticleSidebarBottom ) );
        }

    }

}
