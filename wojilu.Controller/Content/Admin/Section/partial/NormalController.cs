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


    public partial class NormalController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, int postcat, int imgcat, List<ContentPost> posts, ContentPost img ) {

            set( "postAddUrl", Link.To( new PostController().Add, sectionId ) + "?categoryId=" + postcat );
            set( "postListUrl", Link.To( new ListController().AdminList, sectionId ) + "?categoryId=" + postcat );

            set( "imgAddUrl", Link.To( new PostController().Add, sectionId ) + "?categoryId=" + imgcat );
            set( "imgListUrl", Link.To( new ListController().AdminList, sectionId ) + "?categoryId=" + imgcat );


            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );


                block.Set( "post.Url", Link.To( new PostController().Edit, post.Id ) );
                block.Set( "post.Created", post.Created.ToShortTimeString() );
                block.Next();
            }

            IBlock imgBlock = getBlock( "img" );
            if (img != null) {
                imgBlock.Set( "ipost.ImgUrl", img.GetImgThumb() );
                imgBlock.Set( "ipost.TitleCss", img.Style );
                imgBlock.Set( "ipost.TitleFull", img.Title );

                if (strUtil.HasText( img.TitleHome ))
                    imgBlock.Set( "ipost.Title", img.TitleHome );
                else
                    imgBlock.Set( "ipost.Title", img.Title );


                String content = strUtil.HasText( img.Summary ) ? img.Summary : strUtil.ParseHtml( img.Content, 50 );
                imgBlock.Set( "ipost.Content", content );
                imgBlock.Set( "ipost.Width", img.Width );
                imgBlock.Set( "ipost.Height", img.Height );

                imgBlock.Set( "ipost.EditLink", Link.To( new PostController().EditImg, img.Id ) );
                imgBlock.Next();
            }
        }

    }
}

