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

    public partial class TalkController : ControllerBase, IPageSection {
        
        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {
            set( "addUrl", to( Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Description", strUtil.ParseHtml( post.Content, 50 ) );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Next();
            }
        }


        private void bindAdminList( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.OrderId", post.OrderId );
                block.Set( "post.Description", strUtil.ParseHtml( post.Content, 40 ) );
                block.Set( "post.PubDate", post.Created );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private void bindAddInfo( ContentSection section ) {
            set( "module.Name", section.Title );
            set( "module.Id", section.Id.ToString() );
        }

        private void bindEditInfo( int postId, ContentPost post ) {
            set( "post.DeleteUrl", Link.To( new ListController().Delete, postId ) );
            set( "module.Name", post.PageSection.Title );
            set( "module.Id", post.PageSection.Id );
            set( "post.Title", post.Title );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );
            set( "post.OrderId", post.OrderId );
            set( "post.Content", post.Content );
        }


    }
}

