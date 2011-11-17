/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    public partial class VideoShowController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, ContentPost video ) {

            set( "addUrl", to( Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );

            if (video == null) {
                set( "post.Content", "" );
                set( "post.Title", "" );
            }
            else {
                String content = WebHelper.GetFlash( video.SourceLink, video.Width, video.Height );
                set( "post.Content", content );

                if (strUtil.HasText( video.TitleHome ))
                    set( "post.Title", video.TitleHome );
                else
                    set( "post.Title", video.Title );
            }


            IBlock editlinkBlock = getBlock( "editlink" );
            if (video != null) {
                editlinkBlock.Set( "post.EditUrl", to( Edit, video.Id ) );
                editlinkBlock.Next();
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

            set( "post.Width", post.Width );
            set( "post.Height", post.Height );

            set( "post.SourceLink", post.SourceLink );
            set( "post.ImgLink", post.ImgLink );
            set( "post.Style", post.Style );
            set( "post.OrderId", post.OrderId );
            set( "post.DeleteUrl", to( Delete, postId ) + "?categoryId=" + ctx.GetInt( "categoryId" ) );
        }

    }
}

