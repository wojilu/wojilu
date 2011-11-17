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

    public partial class ListController : ControllerBase, IPageSection {


        private void bindSectionShow( int sectionId, IList posts ) {

            set( "addUrl", to( new PostController().Add , sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );

                block.Set( "post.Url", to( new PostController().Edit, post.Id ) );
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
                    block.Set( "post.EditUrl", to( new PostController().EditImg, post.Id ) );
                else
                    block.Set( "post.EditUrl", to( new PostController().Edit, post.Id ) );

                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }

        //private void bindAddInfo( int sectionId, ContentSection section ) {
        //    set( "section.Title", section.Title );
        //    set( "module.Id", section.Id );
        //    editor( "Content", "", "300px" );

        //    bindUploadLink( sectionId );
        //}


        //private void bindEditInfo( ContentPost post ) {

        //    if (post.PageSection == null) return;

        //    set( "post.DeleteUrl", to( Delete, post.Id ) );

        //    set( "post.Author", post.Author );
        //    set( "post.Title", post.Title );
        //    set( "post.TitleHome", strUtil.EncodeTextarea( post.TitleHome ) );

        //    set( "post.Width", post.Width );
        //    set( "post.Height", post.Height );

        //    editor( "Content", strUtil.Edit( post.Content ), "250px" );

        //    set( "post.Created", post.Created );
        //    set( "post.Hits", post.Hits );
        //    set( "post.OrderId", post.OrderId );

        //    set( "post.RedirectUrl", post.RedirectUrl );
        //    set( "post.MetaKeywords", post.MetaKeywords );
        //    set( "post.MetaDescription", post.MetaDescription );


        //    set( "post.Summary", post.Summary );
        //    set( "post.SourceLink", post.SourceLink );
        //    set( "post.Style", post.Style );

        //    set( "post.ImgLink", post.GetImgUrl() );
        //    set( "post.TagList", post.Tag.TextString );
        //    String val = AccessStatusUtil.GetRadioList( post.AccessStatus );
        //    set( "post.AccessStatus", val );
        //    set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

        //    radioList( "PickStatus", PickStatus.GetPickStatus(), post.PickStatus.ToString() );


        //    bindUploadLink( post.PageSection.Id );

        //    set( "attachmentAdminLink", to( new AttachmentController().AdminList, post.Id ) );
        //}


        //private void bindUploadLink( int sectionId ) {
        //    set( "uploadLink", to( Upload, sectionId ) );
        //    set( "deleteUploadLink", to( DeleteUpload, sectionId ) );
        //}


    }
}

