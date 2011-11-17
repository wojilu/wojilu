/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class VideoController : ControllerBase, IPageSection {

        private void bindSectionShow( ContentSection section, List<ContentPost> posts ) {
            set( "m.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );

                block.Set( "post.Url", alink.ToAppData( post ) );

                block.Set( "post.PicUrl", post.ImgLink );

                block.Bind( "post", post );

                block.Next();
            }
        }

        private void bindPosts( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Name", section.Title );
            ctx.SetItem( "PageTitle", Page.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "img.Url", alink.ToAppData( post ) );

                block.Set( "img.Thumb", post.ImgLink );
                block.Set( "img.Title", post.Title );
                block.Set( "img.Description", strUtil.CutString( post.Content, 8 ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private void bindShow( ContentPost post ) {
            ctx.SetItem( "ContentPost", post );
            set( "post.Title", post.Title );
            set( "post.CreateTime", post.Created );
            set( "post.ReplyCount", post.Replies );
            set( "post.Source", post.SourceLink );
            set( "post.Hits", post.Hits );

            if (post.Creator != null) {
                set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
            }
            else {
                set( "post.Submitter", "нч" );
            }

            String siteUrl = new UrlInfo( post.SourceLink ).SiteUrl;
            set( "post.Source", siteUrl );
            String val = WebHelper.GetFlash( post.SourceLink, 500, 400 );
            set( "post.Content", val );
        }

    }
}

