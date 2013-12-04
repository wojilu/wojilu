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
    public partial class SummaryController : ControllerBase, IPageSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public SummaryController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public virtual void SectionShow( long sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            DataPage<ContentPost> data = postService.GetPageBySection( sectionId, s.ListCount );
            bindSectionPosts( data.Results );

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            String listLink = clink.toSection( sectionId, ctx );
            set( "lnkList", listLink );
        }


        private void bindSectionPosts( IList posts ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                BinderUtils.bindPostSingle( block, post, ctx );

                block.Next();
            }
        }


        public virtual void List( long sectionId ) {
            run( new ListController().List, sectionId );
        }

        public virtual void Show( long id ) {
            run( new ListController().Show, id );
        }



    }
}

