/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Caching;

namespace wojilu.Web.Controller.Content.Admin.Section {


    [App( typeof( ContentApp ) )]
    public partial class ListController : ControllerBase, IPageAdminSection {

        public virtual IContentPostService postService { get; set; }
        public virtual IContentSectionService sectionService { get; set; }
        public virtual IAttachmentService attachService { get; set; }

        public ListController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachService = new AttachmentService();
        }


        public virtual List<IPageSettingLink> GetSettingLink( long sectionId ) {
            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().EditCount, sectionId );
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
            return "";
        }

        public virtual void AdminSectionShow( long sectionId ) {
            List<ContentPost> posts = GetSectionPosts( sectionId );
            bindSectionShow( sectionId, posts );
        }

        public virtual void AdminList( long sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            DataPage<ContentPost> posts = postService.GetPageBySectionAndCategory( section.Id, ctx.GetLong( "categoryId" ) );

            bindAdminList( section, posts );
        }

        public virtual List<ContentPost> GetSectionPosts( long sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            return postService.GetBySection( sectionId, s.ListCount );
        }

    }
}

