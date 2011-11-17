/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps;
using wojilu.ORM;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common;

namespace wojilu.Web.Controller.Content.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IAttachmentService attachmentService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }

        public ListController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachmentService = new AttachmentService();
            ctService = new ContentCustomTemplateService();
        }

        public void AdminSectionShow( int sectionId ) {
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            return new List<IPageSettingLink>();
        }

        public void List( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            Page.Title = section.Title;

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            if (s.ArticleListMode == ArticleListMode.Summary) view( "ListSummary" );

            DataPage<ContentPost> posts = postService.GetBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ), s.ListPostPerPage );

            bindPostList( section, posts, s );
        }

        public void SectionShow( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            TemplateUtil.loadTemplate( this, s, ctService );

            IList posts = postService.GetBySection( ctx.app.Id, sectionId );
            bindSectionPosts( posts );
        }

        public void Show( int id ) {

            ContentPost post = this.postService.GetById( id, ctx.owner.Id );
            if (post == null || post.PageSection == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            postService.AddHits( post );
            
            ctx.SetItem( "ContentPost", post );

            bindDetail( id, post );

            set( "post.AdBody", AdItem.GetAdById( AdCategory.ArticleInner ) );

        }



    }
}

