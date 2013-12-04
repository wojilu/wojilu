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
    public partial class ThumbSlideController : ControllerBase, IPageAdminSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public ThumbSlideController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public virtual List<IPageSettingLink> GetSettingLink( long sectionId ) {
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

        public virtual String GetEditLink( long postId ) {
            return to( new Common.PostController().Edit, postId );
        }

        public virtual String GetSectionIcon( long sectionId ) {
            return BinderUtils.iconPic;
        }

        public virtual void AdminSectionShow( long sectionId ) {

            int imgcat = PostCategory.Img;
            List<ContentPost> posts = GetSectionPosts( sectionId );
            ContentPost first = posts.Count > 0 ? posts[0] : null;

            bindSectionShow( sectionId, imgcat, posts, first );
        }

        public virtual List<ContentPost> GetSectionPosts( long sectionId ) {
            return postService.GetBySection( sectionId, 4 );
        }

    }
}
