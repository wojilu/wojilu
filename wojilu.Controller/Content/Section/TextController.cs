/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public class TextController : ControllerBase, IPageSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }

        public TextController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        public virtual void SectionShow( long sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            ContentPost posts = this.postService.GetFirstPost( ctx.app.Id, sectionId );
            if (posts != null) {
                set( "post.Content", posts.Content );
            }
            else {
                set( "post.Content", string.Empty );
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

