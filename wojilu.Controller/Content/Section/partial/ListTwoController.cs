/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {


    public partial class ListTwoController : ControllerBase, IPageSection {

        private void bindSectionShow( ContentSection section, List<ContentPost> posts ) {
            set( "m.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                BinderUtils.bindPostSingle( block, post, ctx );

                block.Next();
            }
        }

    }
}

