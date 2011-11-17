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
using wojilu.Web.Controller.Content.Utils;
using wojilu.Common.AppBase;


namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public partial class ThumbSlideController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IContentCustomTemplateService ctService { get; set; }

        public ThumbSlideController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            ctService = new ContentCustomTemplateService();
        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
            return new List<IPageSettingLink>();
        }

        public void AdminSectionShow( int sectionId ) {
        }

        public void List( int sectionId ) {
            run( new ImgController().List, sectionId );
        }

        public void Show( int id ) {
            run( new ListController().Show, id );
        }
        
        public void SectionShow( int sectionId ) {

            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            if (s == null) {
                throw new Exception( lang( "exDataNotFound" ) + "=>page section:" + sectionId );
            }

            TemplateUtil.loadTemplate( this, s, ctService );


            List<ContentPost> posts = this.postService.GetBySection( ctx.app.Id, sectionId, 4 );
            ContentPost first = posts.Count > 0 ? posts[0] : null;

            bindSectionShow( sectionId, posts, first );
        }

    }

}
