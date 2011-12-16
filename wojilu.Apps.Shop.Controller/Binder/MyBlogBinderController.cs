/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Shop.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Shop.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Common;
using wojilu.Apps.Shop.Service;
using wojilu.Web.Controller.Shop.Utils;


namespace wojilu.Web.Controller.Shop.Binder {

    public class MyBlogBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }

        public MyBlogBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {


                block.Set( "d.Created", cvt.ToTimeString(  item.Created ));
                block.Set( "d.Replies", item.Replies );

                block.Set( "d.Title", strUtil.CutString( item.Title, 20 ) );
                block.Set( "d.LinkShow", item.Link );
                block.Set("d.Content", strUtil.ParseHtml(item.Content, 80));

                block.Bind( "d", item );

                block.Next();
            }

        }

    }

}
