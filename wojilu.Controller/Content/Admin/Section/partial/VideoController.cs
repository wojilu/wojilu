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


    public partial class VideoController : ControllerBase, IPageSection {

        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {
            set( "addUrl", to( Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );

                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Set( "post.PicUrl", post.ImgLink );
                block.Next();
            }
        }

        private void bindAdminList( int sectionId, ContentSection section, DataPage<ContentPost> posts ) {
            set( "moduleName", section.Title );
            set( "addUrl", to( Add, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.PicUrl", post.ImgLink );
                block.Set( "post.EditLink", to( Edit, post.Id ) );
                block.Set( "post.DeleteLink", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private void bindAddInfo( ContentSection section ) {
            set( "module.Name", section.Title );
            set( "module.Id", section.Id );
        }

        private void bindEditInfo( int postId, ContentPost post ) {
            set( "module.Id", post.PageSection.Id );
            set( "module.Name", post.PageSection.Title );
            set( "post.Title", post.Title );
            set( "post.TitleHome", post.TitleHome );

            set( "post.SourceLink", post.SourceLink );
            set( "post.ImgLink", post.ImgLink );
            set( "post.Style", post.Style );
            set( "post.OrderId", post.OrderId );
            set( "post.DeleteUrl", to( Delete, postId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
        }

    }
}

