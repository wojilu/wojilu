/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    public partial class ImgController : ControllerBase, IPageAdminSection {

        private void bindSectionShow( int sectionId, List<ContentPost> posts ) {
            set( "addUrl", to( AddListInfo, sectionId ) );
            set( "listUrl", to( AdminList, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );


                this.bindEditCmd( block, post );
                block.Set( "post.PicUrl", post.GetImgThumb() );
                block.Next();
            }
        }

        private void bindEditCmd( IBlock block, ContentPost post ) {
            String lnkEdit = "";

            lnkEdit = to( AddImgList, post.Id );

            block.Set( "post.EditUrl", lnkEdit );
        }

        private void bindAdminList( int sectionId, ContentSection section, DataPage<ContentPost> posts ) {
            set( "moduleName", section.Title );
            set( "addUrl", to( AddListInfo, sectionId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {
                block.Set( "post.Title", post.Title );
                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                block.Set( "post.PicUrl", post.GetImgThumb() );
                this.bindEditCmd( block, post );
                block.Set( "post.DeleteUrl", to( Delete, post.Id ) );
                block.Next();
            }
            set( "page", posts.PageBar );
        }


        private void bindAddList( int postId, ContentPost post, List<ContentImg> imgList ) {
            set( "section.Name", post.SectionName );
            set( "post.EditListInfo", to( EditListInfo, postId ) );
            IBlock block = getBlock( "list" );
            foreach (ContentImg img in imgList) {
                block.Set( "img.Url", img.GetImgOriginal() );
                block.Set( "img.Thumb", img.GetThumbS() );
                block.Set( "img.Description", strUtil.CutString( img.Description, 8 ) );
                block.Set( "img.DeleteUrl", to( DeleteImg, post.Id ) + "?imgId=" + img.Id );
                String setLogoCmd = getSetLogoCmd( post, img );
                block.Set( "img.SetLogo", setLogoCmd );
                block.Next();
            }

            int upCounts = 3;
            IBlock upblock = getBlock( "upList" );
            for (int i = 1; i < (upCounts + 1); i++) {
                upblock.Set( "photoIndex", i );
                upblock.Next();
            }
        }

        private String getSetLogoCmd( ContentPost post, ContentImg img ) {

            if (img.ImgUrl.Equals( post.ImgLink )) {
                return "<span style='font-weight:bold;color:red;'>" + alang( "currentCover" ) + "</span>";
            }

            return string.Format( "<a href='{0}' class=\"putCmd cmd\">" + alang( "setCover" ) + "</a>", to( SetLogo, post.Id ) + "?imgId=" + img.Id );

        }

        private void bindListEdit( int postId, ContentPost post ) {

            set( "section.Name", post.SectionName );
            set( "post.Title", post.Title );
            set( "post.TitleHome", post.TitleHome );

            set( "post.MetaKeywords", post.MetaKeywords );
            set( "post.MetaDescription", post.MetaDescription );

            set( "post.Content", post.Content );
            set( "post.OrderId", post.OrderId );
            set( "post.ImgListUrl", to( AddImgList, postId ) );

            set( "post.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( post.CommentCondition ) ) );

            set( "Content", post.Content );


        }


    }
}

