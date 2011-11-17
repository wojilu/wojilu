/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;

using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common.AppBase;
using wojilu.ORM;

namespace wojilu.Web.Controller.Content.Section {


    public partial class ImgController : ControllerBase, IPageSection {

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

                block.Set( "post.PicUrl", post.GetImgThumb() );

                block.Bind( "post", post );

                block.Next();
            }
        }

        private void bindShow( ContentPost post, DataPage<ContentImg> imgPage ) {
            ctx.SetItem( "ContentPost", post );
            set( "post.Title", post.Title );
            set( "post.Content", post.Content );
            set( "post.CreateTime", post.Created );
            set( "post.ReplyCount", post.Replies );
            set( "post.Hits", post.Hits );

            set( "post.Source", post.SourceLink );

            if (post.Creator != null) {
                set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
            }
            else {
                set( "post.Submitter", "нч" );
            }

            IBlock block = getBlock( "list" );
            foreach (ContentImg img in imgPage.Results) {
                block.Set( "img.Url", img.GetImgUrl() );
                block.Set( "img.Description", img.Description );
                block.Next();
            }

            String postLink = alink.ToAppData( post );
            String pageBar = ObjectPage.GetPageBarByLink( postLink, imgPage.PageCount, imgPage.Current );

            set( "page", pageBar );

        }

        private void bindPosts( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Name", section.Title );
            ctx.SetItem( "PageTitle", Page.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "img.Url", alink.ToAppData( post ) );

                block.Set( "img.Thumb", post.GetImgThumb() );
                block.Set( "img.Title", post.Title );
                block.Set( "img.Description", strUtil.CutString( post.Content, 8 ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }




    }
}

