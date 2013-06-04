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
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Htmls;

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

                block.Set( "post.Url", alink.ToAppData( post, ctx ) );

                block.Set( "post.PicUrl", post.GetImgThumb() );

                block.Bind( "post", post );

                block.Next();
            }
        }

        private void bindShow( ContentPost post, DataPage<ContentImg> imgPage ) {
            ctx.SetItem( "ContentPost", post );

            bind( "post", post );

            IBlock block = getBlock( "list" );
            foreach (ContentImg img in imgPage.Results) {
                block.Set( "img.Url", img.GetImgOriginal() );
                block.Set( "img.Description", img.Description );
                block.Next();
            }

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            String postLink = alink.ToAppData( post, ctx );
            String pageBar = PageHelper.GetSimplePageBar( postLink, imgPage.Current, imgPage.PageCount, isMakeHtml );

            set( "page", pageBar );
        }

        private void bindPosts( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Name", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "img.Url", alink.ToAppData( post, ctx ) );

                block.Set( "img.Thumb", post.GetImgThumb() );
                block.Set( "img.Title", post.Title );
                block.Set( "img.Description", strUtil.CutString( post.Content, 8 ) );
                block.Next();
            }
        }




    }
}

