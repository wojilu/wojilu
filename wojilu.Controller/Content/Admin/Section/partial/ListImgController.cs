/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    public partial class ListImgController : ControllerBase, IPageAdminSection {


        private void bindSectionShow( int sectionId, int postcat, int imgcat, List<ContentPost> posts, List<ContentPost> imgs ) {

            set( "postAddUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + postcat );
            set( "postListUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + postcat );

            set( "imgAddUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + imgcat );
            set( "imgListUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + imgcat );


            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );

                block.Set( "post.Url", to( new Common.PostController().Edit, post.Id ) );
                block.Set( "post.Created", post.Created.ToShortTimeString() );
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
                imgBlock.Set( "img.Url", to( new Common.PostController().EditImg, img.Id ) );
                imgBlock.Bind( "img", img );

                imgBlock.Next();
            }
        }


    }
}

