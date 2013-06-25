/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class SectionController : ControllerBase {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public SectionController() {

            LayoutControllerType = typeof( Section.LayoutController );

            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        [CacheAction( typeof( ContentLayoutCache ) )]
        public override void Layout() {
        }

        [Data( typeof( ContentSection ) )]
        public void Show( int sectionId ) {
            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            if (section == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }
            set( "listContent", loadHtml( section.SectionType, "List", sectionId ) );
            showInfo( section );
        }

        private void showInfo( ContentSection section ) {

            //1)location
            String location = string.Format( "<a href='{0}'>{1}</a> &gt; {2}",
                alink.ToApp( ctx.app.obj as IApp, ctx ),
                ctx.app.Name,
                "分类查看"
            );

            set( "location", location );

            bindMetaInfo( section );
        }

        private void bindMetaInfo( ContentSection section ) {

            ctx.Page.SetTitle( section.Title, ctx.app.Name );

            if (strUtil.HasText( section.MetaKeywords )) {
                ctx.Page.Keywords = section.MetaKeywords;
            }
            else {
                ctx.Page.Keywords = section.Title;
            }

            ctx.Page.Description = section.MetaDescription;
        }


    }

}
