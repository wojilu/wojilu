/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Service;

namespace wojilu.Web.Controller.Shop.Binder {

    public class ImgBinderController : ControllerBase, ISectionBinder {

        public IShopCustomTemplateService ctService { get; set; }
        public ImgBinderController() {
            ctService = new ShopCustomTemplateService();
        }

        public void Bind( ShopSection section, IList serviceData ) {

            IBlock block = base.getBlock( "list" );

            foreach (IBinderValue item in serviceData) {

                block.Set( "binder.Title", strUtil.SubString( item.Title, 10 ) );
                block.Set("binder.Url", item.Link);
                block.Set("binder.PicUrl", item.PicUrl);

                block.Bind("binder", item);

                block.Next();
            }

        }

    }

}
