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

    public partial class ThumbSlideController : ControllerBase, IPageAdminSection {

        private void bindSectionShow( int sectionId, int imgcat, List<ContentPost> posts, ContentPost first ) {

            set( "sectionId", sectionId );
            set( "addUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + imgcat );
            set( "listUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + imgcat );

            IBlock block = getBlock( "nav" );

            int i = 1;
            foreach (ContentPost photo in posts) {
                block.Set( "photo.Number", i );

                if (strUtil.HasText( photo.TitleHome ))
                    block.Set( "photo.Title", photo.TitleHome );
                else
                    block.Set( "photo.Title", photo.Title );

                block.Set( "photo.ImgUrl", photo.GetImgOriginal() );
                block.Set( "photo.ThumbUrl", photo.GetImgThumb() );
                block.Set( "photo.Link", to( new Common.PostController().EditImg, photo.Id ) );
                block.Next();
                i++;
            }

            IBlock fblock = getBlock( "first" );
            if (first != null) {

                fblock.Set( "first.Title", strUtil.SubString( first.Title, 20 ) );
                fblock.Set( "first.ImgUrl", first.GetImgOriginal() );
                fblock.Set( "first.Link", to( new Common.PostController().EditImg, first.Id ) );

                fblock.Set( "first.Width", first.Width );
                fblock.Set( "first.Height", first.Height );
                fblock.Next();
            }

        }

    }
}
