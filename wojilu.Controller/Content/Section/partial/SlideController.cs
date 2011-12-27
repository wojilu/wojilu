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

    public partial class SlideController : ControllerBase, IPageSection {

        private void bindSectionShow( int sectionId, List<ContentPost> posts, ContentPost first ) {

            IBlock block = getBlock( "nav" );

            int width = first.Width - 20;
            int height = first.Height;
            set( "first.Width", width );
            set( "first.Height", height );

            foreach (ContentPost photo in posts) {

                block.Set( "photo.Width", width );
                block.Set( "photo.Height", height );

                block.Set( "photo.TitleFull", photo.Title );

                if (strUtil.HasText( photo.TitleHome ))
                    block.Set( "photo.Title", photo.TitleHome );
                else
                    block.Set( "photo.Title", photo.Title );


                block.Set( "photo.ImgUrl", photo.GetImgMedium() );
                block.Set( "photo.Link", alink.ToAppData( photo, ctx ) );

                block.Bind( "photo", photo );

                block.Next();
            }




        }

    }

}
