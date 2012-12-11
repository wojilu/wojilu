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

using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Caching;

namespace wojilu.Web.Controller.Content.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }

        public ListController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            ctService = new ContentCustomTemplateService();
        }

        public void AdminSectionShow( int sectionId ) {
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            return new List<IPageSettingLink>();
        }

        [Data( typeof( ContentSection ) )]
        public void List( int sectionId ) {

            ContentSection section = ctx.Get<ContentSection>();
            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            DataPage<ContentPost> posts = postService.GetPageBySection( sectionId, s.ListPostPerPage );

            bindListCommon( sectionId, section, s, posts );

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            set( "page", posts.GetRecentPage( clink.toSection( sectionId, ctx ), clink.toArchive( sectionId, ctx ), 3, isMakeHtml ) );

        }


        [Data( typeof( ContentSection ) )]
        public void Archive( int sectionId ) {
            view( "List" );

            ContentSection section = ctx.Get<ContentSection>();
            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            DataPage<ContentPost> posts = postService.GetPageBySectionArchive( sectionId, s.ListPostPerPage );

            bindListCommon( sectionId, section, s, posts );

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            set( "page", posts.GetArchivePage( clink.toSection( sectionId, ctx ), clink.toArchive( sectionId, ctx ), 3, isMakeHtml ) );
        }

        private void bindListCommon( int sectionId, ContentSection section, ContentSetting s,
            DataPage<ContentPost> posts ) {
            Page.Title = section.Title;
            if (s.ArticleListMode == ArticleListMode.Summary) view( "ListSummary" );
            bindPostList( section, posts, s );
        }


        public void SectionShow( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            TemplateUtil.loadTemplate( this, s, ctService );

            IList posts = postService.GetBySection( sectionId );
            bindSectionPosts( posts );
        }

        public void Show( int id ) {

            ContentPost post = this.postService.GetById( id, ctx.owner.Id );
            if (post == null) {
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

