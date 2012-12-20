/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;

using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class PickedImgController : PickImgBaseController<BlogPickedImg> {

        public override IPageList GetPage() {
            int imgCount = 6;
            return ndb.findPage( typeof( BlogPickedImg ), "", imgCount );
        }

    }

}
