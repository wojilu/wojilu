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
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Section {


    [App( typeof( ContentApp ) )]
    public partial class ImgController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentImgService imgService { get; set; }

        public ImgController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            imgService = new ContentImgService();
        }

        public void SectionShow( int sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            List<ContentPost> posts = postService.GetBySection( sectionId );
            bindSectionShow( s, posts );
        }

        public void Show( int postId ) {
            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            int currentPage = ctx.route.page;
            DataPage<ContentImg> imgPage = imgService.GetImgPage( postId, currentPage );

            bindShow( post, imgPage );
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

            String cpLink = clink.toSection( sectionId, ctx );
            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            DataPage<ContentPost> posts = postService.GetPageBySection( sectionId, s.ListPostPerPage );

            set( "page", posts.GetPageBar( cpLink, isMakeHtml ) );

            bindPosts( section, posts );
        }


    }
}

