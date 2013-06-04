/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public partial class SlideController : ControllerBase, IPageAdminSection {

        private void bindSectionShow( int sectionId, int imgcat, List<ContentPost> posts, ContentPost first ) {

            set( "sectionId", sectionId );
            set( "addUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + imgcat );
            set( "listUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + imgcat );

            int slideWidth = first == null ? 300 : first.Width-30;
            int slideHeight = first == null ? 220 : first.Height;

            set( "slideWidth", slideWidth );
            set( "slideHeight", slideHeight );

            IBlock block = getBlock( "nav" );

            foreach (ContentPost photo in posts) {

                block.Set( "photo.TitleFull", photo.Title );

                if (strUtil.HasText( photo.TitleHome ))
                    block.Set( "photo.Title", photo.TitleHome );
                else
                    block.Set( "photo.Title", photo.Title );

                block.Set( "photo.ImgUrl", photo.GetImgMedium() );
                String lnk = photo.HasImg() ? to( new Common.PostController().EditImg, photo.Id ) : "#";
                block.Set( "photo.Link", lnk );
                block.Next();

            }
        }


    }
}
