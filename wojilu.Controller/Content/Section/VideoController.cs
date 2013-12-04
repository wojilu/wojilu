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
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public partial class VideoController : ControllerBase, IPageSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public VideoController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public virtual void SectionShow( long sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            List<ContentPost> posts = this.postService.GetBySection( sectionId );

            bindSectionShow( s, posts );
        }

        public virtual void List( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            Page.Title = section.Title;

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetLong( "categoryId" ), s.ListVideoPerPage );

            bindPosts( section, posts );
        }

        public virtual void Show( long id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            bindShow( post );
        }

    }
}