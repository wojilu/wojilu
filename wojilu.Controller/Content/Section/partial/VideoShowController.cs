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

    public partial class VideoShowController : ControllerBase, IPageSection {

        private void bindSectionShow( ContentSection section, ContentPost video ) {


            set( "m.Title", section.Title );

            if (video == null) {

                set( "video.Content", "" );
                set( "video.TitleFull", "" );
                set( "video.Title", "" );
                set( "video.Url", "" );

            }
            else {

                String content = WebHelper.GetFlash( video.SourceLink, video.Width, video.Height );
                set( "video.Content", content );
                set( "video.TitleFull", video.Title );

                if (strUtil.HasText( video.TitleHome ))
                    set( "video.Title", video.TitleHome );
                else
                    set( "video.Title", video.Title );

                set( "video.Url", alink.ToAppData( video, ctx ) );
            }
        }

        private void bindPosts( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Name", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {

                //block.Set( "img.Url", to( Show, post.Id ) );
                block.Set( "img.Url", alink.ToAppData( post, ctx ) );

                block.Set( "img.Thumb", post.ImgLink );
                block.Set( "img.Title", strUtil.CutString( post.Title, 8 ) );
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

            String siteUrl = new UrlInfo( post.SourceLink ).SiteUrl;
            set( "post.Source", siteUrl );
            String val = WebHelper.GetFlash( post.SourceLink, 500, 400 );
            set( "post.Content", val );
        }

    }
}

