/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class NormalController : ControllerBase, IPageSection {

        private void bindSectionShow( List<ContentPost> posts, ContentPost img ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                BinderUtils.bindPostSingle( block, post, ctx );

                block.Next();
            }

            IBlock imgBlock = getBlock( "img" );
            if (img != null) {

                imgBlock.Set( "ipost.TitleCss", img.Style );

                String content = strUtil.HasText( img.Summary ) ? img.Summary : strUtil.ParseHtml( img.Content, 50 );
                imgBlock.Set( "ipost.Content", content );

                int width = img.Width <= 0 ? 120 : img.Width;
                int height = img.Height <= 0 ? 90 : img.Height;

                imgBlock.Set( "ipost.Width", width );
                imgBlock.Set( "ipost.Height", height );

                imgBlock.Set( "ipost.Url", alink.ToAppData( img, ctx ) );

                imgBlock.Bind( "ipost", img );

                imgBlock.Next();
            }
        }

    }
}
