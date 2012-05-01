/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class LayoutController : ControllerBase {

        private void bindPosts( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;
            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {

                if (post.PageSection == null) continue;

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Link", alink.ToAppData( post, ctx ) );
                block.Next();
            }
            panel.Next();
        }

        private void bindImgs( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;

            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {
                if (post.PageSection == null) continue;

                block.Set( "post.Img", post.GetImgThumb() );
                String lnk = alink.ToAppData( post, ctx );

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Link", lnk );
                block.Next();
            }
            panel.Next();
        }

        private void bindVideos( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;

            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {

                if (post.PageSection == null) continue;

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Img", post.GetImgThumb() );

                block.Set( "post.Link", alink.ToAppData( post, ctx ) );
                block.Next();
            }
            panel.Next();
        }

    }

}
