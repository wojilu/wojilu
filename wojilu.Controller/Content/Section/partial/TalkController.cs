/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class TalkController : ControllerBase, IPageSection {

        private void bindSectionShow( ContentSection section, IList posts ) {
            set( "m.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Description", post.Content );

                block.Bind( "post", post );
                block.Next();
            }
        }


        private void bindPosts( DataPage<ContentPost> posts ) {
            ctx.SetItem( "PageTitle", Page.Title);
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Description", post.Content );
                block.Set( "post.Created", post.Created );

                block.Next();
            }
            set( "page", posts.PageBar );
        }

    }
}

