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
    public partial class SummaryController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public IAttachmentService attachService { get; set; }

        public SummaryController() {
            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachService = new AttachmentService();
        }


        public List<IPageSettingLink> GetSettingLink( int sectionId ) {
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

        public String GetEditLink( int postId ) {
            return to( new Common.PostController().Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return "";
        }

        public void AdminSectionShow( int sectionId ) {
            List<ContentPost> posts = GetSectionPosts( sectionId );
            bindSectionShow( sectionId, posts );
        }

        private void bindSectionShow( int sectionId, IList posts ) {

            set( "addUrl", to( new Common.PostController().Add, sectionId ) );
            set( "listUrl", to( new ListController().AdminList, sectionId ) );

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                block.Set( "post.Url", to( new Common.PostController().Edit, post.Id ) );

                block.Bind( "post", post );
                block.Next();
            }
        }



        public List<ContentPost> GetSectionPosts( int sectionId ) {
            ContentSection s = sectionService.GetById( sectionId, ctx.app.Id );
            return postService.GetBySection( sectionId, s.ListCount );
        }

    }
}

