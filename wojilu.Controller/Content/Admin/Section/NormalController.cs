/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
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
    public partial class NormalController : ControllerBase, IPageAdminSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentImgService imgService { get; set; }

        public NormalController() {
            postService = new ContentPostService();
            imgService = new ContentImgService();
        }

        public virtual List<IPageSettingLink> GetSettingLink( long sectionId ) {
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

        public virtual String GetEditLink( long postId ) {
            return to( new Common.PostController().Edit, postId );
        }

        public virtual String GetSectionIcon( long sectionId ) {
            return "";
        }

        public virtual void AdminSectionShow( long sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            List<ContentPost> posts = postService.GetTopBySectionAndCategory( sectionId, postcat );
            ContentPost img = imgService.GetTopImg( sectionId, imgcat, ctx.app.Id );

            bindSectionShow( sectionId, postcat, imgcat, posts, img );
        }

        public virtual List<ContentPost> GetSectionPosts( long sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            List<ContentPost> posts = postService.GetTopBySectionAndCategory( sectionId, postcat );
            ContentPost img = imgService.GetTopImg( sectionId, imgcat, ctx.app.Id );

            List<ContentPost> list = new List<ContentPost>();
            list.AddRange( posts );
            list.Add( img );
            return list;
        }

    }
}

