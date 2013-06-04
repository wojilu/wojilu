/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public partial class TextController : ControllerBase, IPageAdminSection {


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
                block.Set( "post.Content", getContent( post ) );
                block.Set( "post.OrderId", post.OrderId );
                block.Set( "post.PubDate", post.Created );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private String getContent( ContentPost post ) {

            String result = strUtil.ParseHtml( post.Content, 100 );
            if (strUtil.HasText( result )) return result;
            return post.Created.ToShortDateString();
        }



    }
}

