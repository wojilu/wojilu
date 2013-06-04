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

    public partial class TalkController : ControllerBase, IPageAdminSection {
        
        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {
            set( "addUrl", to( Add, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {
                block.Set( "post.Author", post.Author );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", post.SourceLink );
                block.Set( "post.Content", post.Content );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Next();
            }
        }


        private void bindAdminList( ContentSection section, DataPage<ContentPost> posts ) {
            set( "section.Title", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "post.Author", post.Author );
                block.Set( "post.Title", post.Title );
                block.Set( "post.OrderId", post.OrderId );
                block.Set( "post.Content", strUtil.ParseHtml( post.Content, 40 ) );
                block.Set( "post.Created", post.Created );
                block.Set( "post.EditUrl", to( Edit, post.Id ) );
                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        private void bindAddInfo( ContentSection section ) {
            set( "section.Name", section.Title );
            set( "section.Id", section.Id.ToString() );
        }

        private void bindEditInfo( int postId, ContentPost post ) {
            set( "section.Name", post.SectionName );
            set( "section.Id", post.SectionId );
            set( "post.Author", post.Author );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );
            set( "post.OrderId", post.OrderId );
            set( "post.Content", post.Content );


            set( "post.Created", post.Created );
            set( "post.Hits", post.Hits );
            set( "post.OrderId", post.OrderId );

            set( "post.RedirectUrl", post.RedirectUrl );
            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );


            set( "post.Summary", post.Summary );
            set( "post.SourceLink", post.SourceLink );
            set( "post.Style", post.Style );


            set( "post.TagList", post.Tag.TextString );
            String val = AccessStatusUtil.GetRadioList( post.AccessStatus );
            set( "post.AccessStatus", val );
            set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

        }


    }
}

