/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    public partial class ThumbSlideController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, List<ContentPost> posts, ContentPost first ) {
            set( "sectionId", sectionId );
            IBlock block = getBlock( "nav" );

            int i = 1;
            foreach (ContentPost photo in posts) {
                block.Set( "photo.Number", i );
                block.Set( "photo.ImgUrl", photo.GetImgOriginal() );
                block.Set( "photo.ThumbUrl", photo.GetImgThumb() );
                block.Set( "photo.Link", alink.ToAppData( photo, ctx ) );

                if (strUtil.HasText( photo.TitleHome ))
                    block.Set( "photo.Title", photo.TitleHome );
                else
                    block.Set( "photo.Title", photo.Title );

                block.Bind( "photo", photo );

                block.Next();
                i++;
            }

            IBlock fblock = getBlock( "first" );
            if (first != null) {

                if (strUtil.HasText( first.TitleHome ))
                    fblock.Set( "first.Title", first.TitleHome );
                else
                    fblock.Set( "first.Title", first.Title );


                fblock.Set( "first.ImgUrl", first.GetImgOriginal() );
                fblock.Set( "first.Link", alink.ToAppData( first, ctx ) );


                fblock.Set( "first.Width", first.Width );
                fblock.Set( "first.Height", first.Height );
                fblock.Bind( "first", first );
                fblock.Next();
            }


        }

    }

}
