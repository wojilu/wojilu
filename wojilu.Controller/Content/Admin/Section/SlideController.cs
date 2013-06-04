/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public partial class SlideController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public SlideController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().Edit, sectionId );
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
            return BinderUtils.iconPic;
        }

        public void AdminSectionShow( int sectionId ) {

            int imgcat = PostCategory.Img;
            List<ContentPost> posts = GetSectionPosts( sectionId );
            ContentPost first = posts.Count > 0 ? posts[0] : null;

            bindSectionShow( sectionId, imgcat, posts, first );
        }

        public List<ContentPost> GetSectionPosts( int sectionId ) {
            return this.postService.GetBySection( sectionId, 3 );
        }

    }
}
