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

        public void Show( int sectionId ) {

            ContentSection section = sectionService.GetById( sectionId, ctx.app.Id );
            WebUtils.pageTitle( this, section.Title, ctx.app.Name );

            //1)location
            String location = string.Format( "<a href='{0}'>{1}</a> &gt; {2}", 
                Link.To( new ContentController().Index ),
                ((AppContext)ctx.app).Menu.Name, 
                "分类查看"
            );

            set( "location", location );


            set( "listContent", loadHtml( section.SectionType, "List", sectionId ) );


        }



    }

}
