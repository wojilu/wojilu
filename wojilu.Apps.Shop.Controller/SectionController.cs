/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Shop.Caching;
using wojilu.Web.Context;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Shop
{

    [App( typeof( ShopApp ) )]
    public class SectionController : ControllerBase {

        public IShopItemService postService { get; set; }
        public IShopSectionService sectionService { get; set; }

        public SectionController() {

            LayoutControllerType = typeof( Section.LayoutController );

            postService = new ShopItemService();
            sectionService = new ShopSectionService();
        }

        [CacheAction( typeof( ShopLayoutCache ) )]
        public override void Layout() {
        }

        public void Show( int sectionId ) {

            ShopSection section = sectionService.GetById( sectionId, ctx.app.Id );
            WebUtils.pageTitle( this, section.Title, ctx.app.Name );

            //1)location
            String location = string.Format( "<a href='{0}'>{1}</a> &gt; {2}",
                Link.To(new ShopController().Index),
                ((AppContext)ctx.app).Menu.Name, 
                "索引查看"
            );

            set( "location", location );


            set( "listContent", loadHtml( section.SectionType, "List", sectionId ) );


        }



    }

}
