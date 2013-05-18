/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common;
using wojilu.Common.AppBase;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public ListController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        [Data( typeof( ContentSection ) )]
        public void List( int sectionId ) {

            ContentSection section = ctx.Get<ContentSection>();
            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            DataPage<ContentPost> posts = postService.GetPageBySection( sectionId, s.ListPostPerPage );

            bindListCommon( sectionId, section, s, posts );

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            String listLink = clink.toSection( sectionId, ctx );
            set( "page", posts.GetPageBar( listLink, isMakeHtml ) );

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

            IList posts = postService.GetBySection( sectionId, s.ListCount );
            bindSectionPosts( posts );
        }

        public void Show( int id ) {

            ContentPost post = this.postService.GetById( id, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            ctx.SetItem( "ContentPost", post );

            bindDetail( id, post );

            set( "post.AdBody", AdItem.GetAdById( AdCategory.ArticleInner ) );

        }



    }
}

