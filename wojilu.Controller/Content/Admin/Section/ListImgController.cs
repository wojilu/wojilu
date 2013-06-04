/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListImgController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentImgService imgService { get; set; }

        public ListImgController() {
            postService = new ContentPostService();
            imgService = new ContentImgService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().EditCount, sectionId );
            links.Add( lnk );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().Edit, sectionId );
            links.Add( lnktmp );

            return links;
        }

        public String GetEditLink( int postId ) {
            return to( new Common.PostController().Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return "";
        }

        public void AdminSectionShow( int sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            List<ContentPost> posts = this.postService.GetTopBySectionAndCategory( sectionId, postcat );
            List<ContentPost> imgs = this.imgService.GetByCategory( sectionId, imgcat, ctx.app.Id );
            
            bindSectionShow( sectionId, postcat, imgcat, posts, imgs );
        }

        public List<ContentPost> GetSectionPosts( int sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            List<ContentPost> posts = this.postService.GetTopBySectionAndCategory( sectionId, postcat );
            List<ContentPost> imgs = this.imgService.GetByCategory( sectionId, imgcat, ctx.app.Id );

            List<ContentPost> list = new List<ContentPost>();
            list.AddRange( posts );
            list.AddRange( imgs );
            return list;
        }

    }
}

