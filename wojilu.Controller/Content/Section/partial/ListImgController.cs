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


    public partial class ListImgController : ControllerBase, IPageSection {

        private void bindSectionShow( List<ContentPost> posts, List<ContentPost> imgs ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                BinderUtils.bindPostSingle( block, post, ctx );

                block.Next();
            }

            IBlock imgBlock = getBlock( "imgs" );
            foreach (ContentPost img in imgs) {
                imgBlock.Set( "img.TitleFull", img.Title );
                imgBlock.Set( "img.TitleCss", img.Style );

                if (strUtil.HasText( img.TitleHome ))
                    imgBlock.Set( "img.Title", img.TitleHome );
                else
                    imgBlock.Set( "img.Title", img.Title );


                imgBlock.Set( "img.Thumb", img.GetImgThumb() );
                imgBlock.Set( "img.Url", alink.ToAppData( img, ctx ) );

                imgBlock.Bind( "img", img );

                imgBlock.Next();
            }
        }

    }
}

