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
    public partial class TalkController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public TalkController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public void SectionShow( int sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            List<ContentPost> posts = this.postService.GetBySection( sectionId );
            bindSectionShow( s, posts );
        }

        public void List( int sectionId ) {
            ContentSection section = this.sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            set( "section.Title", section.Title );
            Page.Title = section.Title;
            DataPage<ContentPost> posts = this.postService.GetPageBySectionAndCategory( section.Id, ctx.GetInt( "categoryId" ) );

            bindPosts( posts );
        }

        public void Show( int postId ) {

            ContentPost post = postService.GetById( postId, ctx.owner.Id );
            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bind( "x", post );
        }

    }
}

