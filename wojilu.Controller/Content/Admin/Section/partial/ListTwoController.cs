/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    public partial class ListTwoController : ControllerBase, IPageAdminSection {


        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {
            set( "addUrl", to( new Common.PostController().Add, sectionId ) );
            set( "listUrl", to( new ListController().AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );


                block.Set( "post.Url", to( new Common.PostController().Edit, post.Id ) );
                block.Next();
            }
        }

    }
}

