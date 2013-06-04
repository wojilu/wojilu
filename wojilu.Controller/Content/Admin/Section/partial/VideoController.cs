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


    public partial class VideoController : ControllerBase, IPageAdminSection {

        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {

            int vWidth = 320;
            int vHeight = 240;

            set( "addUrl", to( Add, sectionId ) + "?width=" + vWidth + "&height=" + vHeight );
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
            set( "section.Name", section.Title );
            set( "section.Id", section.Id );
            set( "created", DateTime.Now );

            set( "width", ctx.GetInt( "width" ) );
            set( "height", ctx.GetInt( "height" ) );
        }

        private void bindEditInfo( int postId, ContentPost post ) {
            set( "section.Id", post.SectionId );
            set( "section.Name", post.SectionName );

            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );

            set( "post.SourceLink", post.SourceLink );
            set( "post.ImgLink", post.ImgLink );
            set( "post.Style", post.Style );

            set( "post.Author", post.Author );
            set( "post.Title", post.Title );
            set( "post.TitleHome", strUtil.EncodeTextarea( post.TitleHome ) );

            set( "post.Width", post.Width );
            set( "post.Height", post.Height );

            set( "post.Created", post.Created );
            set( "post.Hits", post.Hits );
            set( "post.OrderId", post.OrderId );

            set( "post.RedirectUrl", post.RedirectUrl );
            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );

            set( "post.Summary", post.Summary );
            set( "post.Style", post.Style );

            set( "post.TagList", post.Tag.TextString );
            String val = AccessStatusUtil.GetRadioList( post.AccessStatus );
            set( "post.AccessStatus", val );
            set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

        }

    }
}

