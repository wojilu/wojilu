/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Enum;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public partial class ListController : ControllerBase, IPageAdminSection {


        private void bindSectionShow( int sectionId, IList posts ) {

            set( "addUrl", to( new Common.PostController().Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );

                block.Set( "post.Url", to( new Common.PostController().Edit, post.Id ) );
                block.Next();
            }
        }


        private void bindAdminList( ContentSection section, DataPage<ContentPost> posts ) {

            set( "section.Title", section.Title );
            IBlock block = getBlock( "list" );

            String icon = string.Format( "<img src=\"{0}\"/> ", strUtil.Join( sys.Path.Img, "img.gif" ) );

            foreach (ContentPost post in posts.Results) {

                String imgIcon = post.HasImg() ? icon : "";
                block.Set( "post.ImgIcon", imgIcon );

                block.Set( "post.Title", strUtil.SubString( post.Title, 50 ) );
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                block.Set( "post.OrderId", post.OrderId );
                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Link", strUtil.CutString( post.SourceLink, 100 ) );
                block.Set( "post.PubDate", post.Created );

                String attachments = post.Attachments > 0 ? "<img src='" + strUtil.Join( sys.Path.Img, "attachment.gif" ) + "'/>" : "";
                block.Set( "post.Attachments", attachments );

                if (post.HasImg())
                    block.Set( "post.EditUrl", to( new Common.PostController().EditImg, post.Id ) );
                else
                    block.Set( "post.EditUrl", to( new Common.PostController().Edit, post.Id ) );

                block.Set( "post.DeleteUrl", to( new Common.PostController().Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

    }
}

