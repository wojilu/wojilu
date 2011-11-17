/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public partial class TextController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, ContentPost textPost ) {
            String content = textPost == null ? "" : textPost.Content;
            set( "post.Content", content );

            IBlock editlinkBlock = getBlock( "editlink" );
            if (textPost != null) {
                editlinkBlock.Set( "post.EditUrl", to( Edit, textPost.Id ) );
                editlinkBlock.Next();
            }

            set( "addUrl", to( Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );
        }

        private void bindAdminList( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                //block.Set( "post.Content", strUtil.CutString( post.Content, 100 ) );
                block.Set( "post.Content", strUtil.ParseHtml( post.Content, 100 ) );
                block.Set( "post.OrderId", post.OrderId );
                block.Set( "post.PubDate", post.Created );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private void bindAddInfo( ContentSection section ) {
            set( "section.Title", section.Title );
            editor( "Content", "", "300px" );
        }

        private void bindEditInfo( ContentPost post ) {
            set( "section.Title", post.PageSection.Title );
            editor( "Content", post.Content, "300px" );
        }


    }
}

